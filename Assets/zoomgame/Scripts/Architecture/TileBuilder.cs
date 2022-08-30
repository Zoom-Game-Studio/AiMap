using System;
using System.Collections.Generic;
using QFramework;
using QFramework.Config;
using UnityEngine;
using WeiXiang;
using Console = WeiXiang.Console;
using Object = UnityEngine.Object;

namespace Architecture
{
    public interface ITileResCache : IUtility
    {
    }

    /// <summary>
    /// 实例化地块资源
    /// </summary>
    public class TileBuilder : ITileResCache
    {
        public static TileAssetBundles ResCache;
        public static readonly string DataName = "data";
        private static List<GameObject> goList = new List<GameObject>();

        /// <summary>
        /// 实例化地块
        /// </summary>
        /// <param name="assetDirPath">Path.Combine(Application.persistentDataPath, nameof(AssetDownloader));</param>
        /// <param name="assetInfoItemId"></param>
        public static void Instantiate(string assetDirPath, string assetInfoItemId)
        {
            try
            {
                var tile = LoadTile(assetDirPath, assetInfoItemId);
                Instantiate(tile, assetInfoItemId);
            }
            catch (System.Exception e)
            {
                Console.Error($"Fail to instantiate tile asset: {assetDirPath}/{assetInfoItemId},because {e.Message}");
                Debug.LogError(e.StackTrace);
            }
        }

        public static void UnLoadAssetBundle()
        {
            if (ResCache != null)
            {
                foreach (var go in goList)
                {
                    if (go)
                    {
                        Object.Destroy(go);
                    }
                }
                goList.Clear();
                AssetBundle.UnloadAllAssetBundles(true);
                ResCache = null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assetDirPath"></param>
        /// <param name="assetInfoItemId"></param>
        /// <returns></returns>
        public static TileAssetBundles LoadTile(string assetDirPath, string assetInfoItemId)
        {
            if (ResCache != null && ResCache.assetInfoItemId.Equals(assetInfoItemId))
            {
                return ResCache;
            }
            else
            {
                UnLoadAssetBundle();
                ResCache = TileAssetBundles.LoadTileAssetBundles(assetDirPath, assetInfoItemId);
                return ResCache;
            }
        } 

        /// <summary>
        /// 获取场地的节点
        /// </summary>
        /// <param name="placeName"></param>
        /// <param name="localPos"></param>
        /// <param name="localAngle"></param>
        /// <param name="localScale"></param>
        /// <returns></returns>
        static Transform TryGetOrCreate(string placeName, Vector3 localPos, Vector3 localAngle, Vector3 localScale)
        {
            var node = LocalizationConvert.Origin.Find(placeName);
            if (!node)
            {
                node = new GameObject(placeName).transform;
                node.SetParent(LocalizationConvert.Origin);
                node.localPosition = localPos;
                node.localScale = localScale;
                node.localEulerAngles = localAngle;
            }

            return node;
        }

        /// <summary>
        /// 通过接口获得的id，获取模型的根节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Transform GetParent(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var rootData = ModelRootConfig.Instance.nodeList.Find(e=> e.id.Equals(id));
                if (rootData != null)
                {
                    return TryGetOrCreate(id, rootData.localPos, rootData.localAngle,rootData.localScale);
                }

                Debug.LogWarning($"没有{id}场景节点的特殊配置，使用Origin节点");
            }

            return LocalizationConvert.Origin;
        }

        public static void Instantiate(TileAssetBundles tileAssetBundles, string assetInfoItemId)
        {
            if (tileAssetBundles == null)
            {
                throw new NullReferenceException("TileAssetBundles can not be null,check it is load complete?");
            }

            var modeData = tileAssetBundles.ModelXml.ConvertItemsToData();
            Transform root = GetParent(assetInfoItemId);
            foreach (var propData in modeData)
            {
                try
                {
                    var names = propData.abName.Split("/");
                    var ab = names[0].ToLower();
                    var prefab = names[1];
                    var assetBundle = tileAssetBundles.GetAssetBundle(ab);
                    var asset = assetBundle.LoadAsset<GameObject>(prefab);
                    var go = Object.Instantiate(asset, root);
                    go.name = propData.name;
                    //xxx 写死了相对姿态
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localEulerAngles = Vector3.zero;
                    go.transform.localScale = new Vector3(1, -1, 1);
                    goList.Add(go);
                }
                catch (Exception e)
                {
                    Debug.LogError($"实例化资源失败：{propData?.name},{propData?.abName},{e.Message}");
                }
            }
        }
    }
}