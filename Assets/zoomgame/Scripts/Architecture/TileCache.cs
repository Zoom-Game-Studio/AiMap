using System.Collections.Generic;
using QFramework;
using QFramework.Config;
using UnityEngine;
using WeiXiang;

namespace Architecture
{
    public interface ITileResCache : IUtility
    {
        /// <summary>
        /// 加载地块的资源
        /// </summary>
        /// <param name="url"></param>
        TileAssetBundles LoadTile(string url);
    }

    /// <summary>
    /// 缓存加载过的ab包
    /// </summary>
    public class TileCache : ITileResCache
    {
        public Dictionary<string, TileAssetBundles> resCache = new Dictionary<string, TileAssetBundles>();

        public readonly string dataName = "data";
        private static TileCache _instance;

        public static TileCache instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TileCache();
                }

                return _instance;
            }
        }

        public TileAssetBundles LoadTile(string url)
        {
            if (resCache.ContainsKey(url))
            {
                return resCache[url];
            }
            else
            {
                var tile = TileAssetBundles.LoadTileAssetBundles(url);
                resCache.Add(url,tile);
                return tile;
            }
        }
    }

    /// <summary>
    /// 实例化地块资源
    /// </summary>
    public static class TileBuilder
    {
        public static void Instantiate(string tileAssetBundleName, TileAssetBundles tileAssetBundles)
        {
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

        /// <summary>
        /// 实例化地块
        /// </summary>
        /// <param name="tileAssetBundleName">地块资源的名字</param>
        public static void Instantiate(string tileAssetBundleName)
        {
            try
            {
                if (!string.IsNullOrEmpty(tileAssetBundleName))
                {
                    if (!TileCache.instance.resCache.ContainsKey(tileAssetBundleName))
                    {
                        var tile = TileCache.instance.LoadTile(tileAssetBundleName);
                        Instantiate(tileAssetBundleName, tile);
                    }
                    else
                    {
                        Console.Warning(tileAssetBundleName + " build has done");
                    }
                }
                else
                {
                    Console.Error("TileAssetBundleName error：" + tileAssetBundleName);
                }
            }
            catch (System.Exception e)
            {
                Console.Error($"Fail to load, tileName: {tileAssetBundleName}");
                Console.Error(e.Message);
                Console.Error(e.StackTrace);
            }
        }
    }
}