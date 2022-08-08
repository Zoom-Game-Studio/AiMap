using System;
using System.Collections.Generic;
using QFramework;
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
                Instantiate(tile);
            }
            catch (System.Exception e)
            {
                Console.Error($"Fail to instantiate tile asset: {assetDirPath}/{assetInfoItemId},because {e.Message}");
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
                if (ResCache != null)
                {
                    var pp = LocalizationConvert.Origin;
                    for (int i = 0; i < pp.childCount; i++)
                    {
                        Object.Destroy(pp.GetChild(i).gameObject);
                    }

                    AssetBundle.UnloadAllAssetBundles(true);
                    ResCache = null;
                }

                ResCache = TileAssetBundles.LoadTileAssetBundles(assetDirPath, assetInfoItemId);
                return ResCache;
            }
        }

        public static void Instantiate(TileAssetBundles tileAssetBundles)
        {
            if (tileAssetBundles == null)
            {
                throw new NullReferenceException("TileAssetBundles can not be null,check it is load complete?");
            }

            var modeData = tileAssetBundles.ModelXml.ConvertItemsToData();
            foreach (var propData in modeData)
            {
                try
                {
                    var names = propData.abName.Split("/");
                    var ab = names[0].ToLower();
                    var prefab = names[1];
                    var assetBundle = tileAssetBundles.GetAssetBundle(ab);
                    var asset = assetBundle.LoadAsset<GameObject>(prefab);

                    Transform root = LocalizationConvert.Origin;
                    var go = Object.Instantiate(asset, root);
                    go.name = propData.name;
                    //xxx 写死了相对姿态
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localEulerAngles = Vector3.zero;
                    go.transform.localScale = new Vector3(1, -1, 1);
                }
                catch (Exception e)
                {
                    Debug.LogError($"实例化资源失败：{propData?.name},{propData?.abName},{e.Message}");
                }
            }
        }
    }
}