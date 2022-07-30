using System.Collections.Generic;
using Data;
using UnityEngine;
using WeiXiang;

namespace Architecture
{
    /// <summary>
    /// 地块资源缓存
    /// </summary>
    public class TileAssetBundles
    {
        public Dictionary<string, AssetBundle> map = new Dictionary<string, AssetBundle>();
        public AssetBundle main;
        public readonly string Name;

        private LayerXml layer;
        private IconXml icon;
        private ModelXml model;
        private ModuleXml module;

        /// <summary>
        /// layer 配置表
        /// </summary>
        public LayerXml LayerXml
        {
            get
            {
                if (layer == null)
                {
                    layer = LayerXml.Convert(LazyLoad("layer").text);
                }
                return layer;
            }
        }
        
        /// <summary>
        /// icon 配置表
        /// </summary>
        public IconXml IconXml
        {
            get
            {
                if (icon == null)
                {
                    icon = IconXml.Convert(LazyLoad("Icon").text);
                }
                return icon;
            }
        }
        
        /// <summary>
        /// model 配置表
        /// </summary>
        public ModelXml ModelXml
        {
            get
            {
                if (model == null)
                {
                    model = ModelXml.Convert(LazyLoad("model").text);
                }
                return model;
            }
        }
        
        /// <summary>
        /// model 配置表
        /// </summary>
        public ModuleXml ModuleXml
        {
            get
            {
                if (module == null)
                {
                    module = ModuleXml.Convert(LazyLoad("model").text);
                }
                return module;
            }
        }

        TextAsset LazyLoad(string name)
        {
            if (map.TryGetValue("data", out var assetBundle))
            {
                return assetBundle.LoadAsset<TextAsset>(name);
            }
            return null;
        }

        public TileAssetBundles(string name)
        {
            Name = name;
        }

        void LoadFromMemory()
        {
        }

        /// <summary>
        /// 从本地加载
        /// </summary>
        public void LoadFromPath()
        {
            var fullPath = PathConvert.GetFullPath(Name) + "/StreamingAssets";
            if (!main)
            {
                main = AssetBundle.LoadFromFile(fullPath);
            }

            if(main == null)
            {
                Console.Error("加载ab包失败：" + fullPath);
                return;
            }
            var manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            var all = manifest.GetAllAssetBundles();
            foreach (var name in all)
            {
                var path = PathConvert.GetPath(Name, name);
                if (!map.TryGetValue(name, out var assetBundle) || !assetBundle)
                {
                    assetBundle = AssetBundle.LoadFromFile(path);
                }

                if (map.ContainsKey(name))
                {
                    map[name] = assetBundle;
                }
                else
                {
                    map.Add(name, assetBundle);
                }
            }
        }

        /// <summary>
        /// 根据manifest中的名字加载ab包
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AssetBundle GetAssetBundle(string name)
        {
            map.TryGetValue(name, out var assetBundle);
            if (!assetBundle)
            {
                foreach (var kv in map)
                {
                    if (kv.Key.EndsWith(name))
                    {
                        return kv.Value;
                    }
                }
            }
            return assetBundle;
        }

        /// <summary>
        /// 根据地块的名字加载地块资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TileAssetBundles LoadTileAssetBundles(string name)
        {
            var tileAssetBundles = new TileAssetBundles(name);
            tileAssetBundles.LoadFromPath();
            return tileAssetBundles;
        }
    }
}