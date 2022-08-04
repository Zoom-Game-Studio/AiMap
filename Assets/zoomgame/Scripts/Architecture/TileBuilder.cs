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
        public static Dictionary<string, TileAssetBundles> ResCache = new Dictionary<string, TileAssetBundles>();
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
            if (ResCache.ContainsKey(assetInfoItemId))
            {
                return ResCache[assetInfoItemId];
            }
            else
            {
                var tile = TileAssetBundles.LoadTileAssetBundles(assetDirPath, assetInfoItemId);
                if (tile != null)
                {
                    ResCache.Add(assetInfoItemId, tile);
                }
                return tile;
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
                var names = propData.abName.Split("/");
                var ab = names[0].ToLower();
                var prefab = names[1];
                var assetBundle = tileAssetBundles.GetAssetBundle(ab);
                var asset = assetBundle.LoadAsset<GameObject>(prefab);

                Transform root = LocalizationConvert.Origin;
                var go = Object.Instantiate(asset, root);
                go.name = propData.name;
                //xxx 写死了相对姿态
                go.transform.localPosition = propData.pos;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }
}