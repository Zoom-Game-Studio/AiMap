using System.Collections.Generic;
using QFramework;
using QFramework.Config;
using UnityEngine;
using WeiXiang;

namespace Architecture
{
    public interface ITileResLoader : IUtility
    {
        /// <summary>
        /// 加载地块的资源
        /// </summary>
        /// <param name="url"></param>
        TileAssetBundles LoadTile(string url);
    }

    public class TileLoader : ITileResLoader
    {
        public Dictionary<string, TileAssetBundles> resCache = new Dictionary<string, TileAssetBundles>();

        public readonly string dataName = "data";
        private static TileLoader _instance;

        public static TileLoader instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TileLoader();
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

    public static class TileBuilder
    {
        public static void Instantiate(string tileName, TileAssetBundles tileAssetBundles)
        {
            var modeData = tileAssetBundles.ModelXml.ConvertItemsToData();
            foreach (var propData in modeData)
            {
                var names = propData.abName.Split("/");
                var ab = names[0].ToLower();
                var prefab = names[1];
                var assetBundle = tileAssetBundles.GetAssetBundle(ab);
                var asset = assetBundle.LoadAsset<GameObject>(prefab);
                Transform root = LocalizationConvert.GetModelRoot(tileName);
                var go = Object.Instantiate(asset, root, true);
                go.name = propData.name;
                var ro = Quaternion.Euler(propData.eulerangle);
                go.transform.rotation = root.rotation * ro;
                go.transform.localPosition = propData.pos;
            }
        }

        /// <summary>
        /// 实例化地块
        /// </summary>
        /// <param name="tileName">地块的名字</param>
        public static void Instantiate(string tileName)
        {
            string url = null;
            if (ModelRootConfig.Instance.Chengdu_indoor.tileName.Equals(tileName))
            {
                url = ModelRootConfig.Instance.Chengdu_indoor.abUrl;
            }
            else if (ModelRootConfig.Instance.ShangHai.tileName.Equals(tileName))
            {
                url = ModelRootConfig.Instance.ShangHai.abUrl;
            }

            if (!string.IsNullOrEmpty(url))
            {
                if (!TileLoader.instance.resCache.ContainsKey(url))
                {
                    var tile = TileLoader.instance.LoadTile(url);
                    Instantiate(tileName, tile);
                }
                else
                {
                    Console.Warning(tileName +" build has done");
                }

            }
            else
            {
                Console.Error("不能从ModelRootConfig获取到url："+ tileName);
            }

        }
    }
}