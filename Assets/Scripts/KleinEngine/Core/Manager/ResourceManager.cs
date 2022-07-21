using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AppLogic;

namespace KleinEngine
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UObject = UnityEngine.Object;

    public class ResourceManager : Singleton<ResourceManager>
    {
        private AssetBundleManifest manifest;
        private AssetBundleManifest dynamicManifest;
        private Dictionary<string, AssetBundle> bundleDic = new Dictionary<string, AssetBundle>();
        private Dictionary<Action<UObject>, List<AssetRequestInfo>> loadingAssetList = new Dictionary<Action<UObject>, List<AssetRequestInfo>>();
        List<Action<UObject>> removeKeyList = new List<Action<UObject>>();

        private bool isInit = false;

#if UNITY_EDITOR
        private bool isUpdate = false;
#endif
        class AssetRequestInfo
        {
            public AssetBundleRequest request;
            public string assetName;
        }

        public ResourceManager()
        {
            //使用Application.temporaryCachePath来操作文件操作方式跟上面Application.persistentDataPath类似。除了在IOS上不能被iCloud自动备份
        }

        /// <summary>
        /// 版本控制用
        /// </summary>
        /// <param name="update"></param>
        public void init(bool update = false)
        {
#if UNITY_EDITOR
            //isInit = true;
            isUpdate = update;
            //if (!isUpdate) return;
#endif
            //Application.streamingAssetsPath这个路径下使用File.Exists判断.bundle是可以判断到的，其他文件格式（已经测试过的为.txt）查找在android下不支持,其他系统如windows,ios都支持
            string filePath = Path.Combine(GlobalObject.ASSET_PATH, "StreamingAssets");
            //string filePath = GlobalObject.ASSET_PATH;
            if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, "StreamingAssets");
            AssetBundle assetbundle = AssetBundle.LoadFromFile(filePath);
            if (null != assetbundle)
            {
                Debug.Log("@Louis init2:" + assetbundle.name);
                manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                Debug.Log("@Louis init3:" + manifest.name);

                assetbundle.Unload(false);
                isInit = true;
            }
            //}
            //else
            //{
            //    Debug.LogWarning("Cant Find Path:" + filePath);
            //}
            //filePath = Path.Combine(GlobalObject.ASSET_PATH, "Dynamic_StreamingAssets");
            //if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, "Dynamic_StreamingAssets");
            //if (File.Exists(filePath))
            //{
            //    AssetBundle assetbundle = AssetBundle.LoadFromFile(filePath);
            //    if (null != assetbundle)
            //    {
            //        dynamicManifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //        assetbundle.Unload(false);
            //        isInit = true;
            //    }
            //}
            //else
            //{
            //    Debug.LogWarning("Cant Find Path:" + filePath);
            //}
        }

        private void HanldeUnloadUnityAssets(EventObject obj)
        {
            bundleDic.Clear();
        }

        public void update()
        {
            if (loadingAssetList.Count == 0) return;
            //            Debug.Log("update:" + loadingAssetList.Count);
            removeKeyList.Clear();
            foreach (var item in loadingAssetList)
            {
                int len = item.Value.Count;
                //i值这里只做循环用，不需要修改数值
                for (int i = 0; i < len;)
                {
                    AssetRequestInfo requestInfo = item.Value[i];
                    if (requestInfo.request.isDone)
                    {
                        //                        Debug.Log("加载OK:" + requestInfo.assetName);
                        item.Key(requestInfo.request.asset);
                        len--;
                        item.Value.RemoveAt(i);
                    }
                    else break;
                }
                if (0 == len) removeKeyList.Add(item.Key);
            }
            foreach (var key in removeKeyList)
            {
                loadingAssetList.Remove(key);
            }
        }

        //异步加载
        public void loadAssetAsync<T>(string abname, string assetname, Action<UObject> callBack = null) where T : UObject
        {
            //if (!isInit) return;
            abname = abname.ToLower();

#if UNITY_EDITOR
            if (!isUpdate)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abname, assetname);
                if (assetPaths.Length > 0)
                {
                    T asset = AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
                    if (null != asset && null != callBack) callBack(asset);
                }
                //return;
            }
#endif
            AssetBundle bundle = null;
            if (GlobalObject.ASSET_PATH.Equals(GlobalObject.OLD_ASSET_PATH))
            {
                bundle = loadAssetBundle(true, abname);
            }
            else
            {
                bundle = loadAssetBundle(false, abname);
            }

            if (bundle != null)
            {
                if (!bundle.Contains(assetname)) return;
                if (!loadingAssetList.ContainsKey(callBack)) loadingAssetList.Add(callBack, new List<AssetRequestInfo>());
                Debug.Log(loadingAssetList.Count + "@count");
                List<AssetRequestInfo> list = loadingAssetList[callBack];
                int len = list.Count;
                bool repeatFlag = false;
                AssetRequestInfo rqInfo = null;
                for (int i = 0; i < len; ++i)
                {
                    rqInfo = list[i];
                    if (rqInfo.assetName == assetname)
                    {
                        repeatFlag = true;
                        if ((len - 1) == i) break;
                        list.RemoveAt(i);
                        list.Add(rqInfo);
                        break;
                    }
                }
                if (!repeatFlag)
                {
                    AssetBundleRequest request = bundle.LoadAssetAsync<T>(assetname);
                    rqInfo = new AssetRequestInfo();
                    rqInfo.assetName = assetname;
                    rqInfo.request = request;
                    list.Add(rqInfo);
                }
            }
        }

        //同步加载
        public T loadAsset<T>(string abname, string assetname) where T : UObject
        {
            //if(!isInit) return null;
            abname = abname.ToLower();

#if UNITY_EDITOR
            if (!isUpdate)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abname, assetname);
                if (assetPaths.Length > 0) return AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
                //return null;
            }

#endif
            AssetBundle bundle = loadAssetBundle(abname);
#if UNITY_EDITOR_WIN
            if (null != bundle)
            {
                T asset = bundle.LoadAsset<T>(assetname);
                if (asset is GameObject)
                {
                    Renderer[] meshSkinRenderer = (asset as GameObject).GetComponentsInChildren<Renderer>();
                    for (int i = 0; i < meshSkinRenderer.Length; i++)
                    {
                        meshSkinRenderer[i].sharedMaterial.shader = Shader.Find(meshSkinRenderer[i].sharedMaterial.shader.name);
                    }
                }
                else if(asset is Material)
                {
                    Material mat = asset as Material;
                    mat.shader = Shader.Find(mat.shader.name);
                }
                return asset;
            }
#else
            if (null != bundle) return bundle.LoadAsset<T>(assetname);
#endif
            return null;
        }
        string oldFilePath = null;

        AssetBundle LoadAssetBundle2(string abname)
        {
            AssetBundle bundle = null;
            //loadDependencies(abname);
            string filePath = Path.Combine(GlobalObject.ASSET_PATH, abname);
            //Debug.Log("loadAssetbundlefilePath:"+filePath);
            if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, abname);
            Debug.Log("@Louis loadAssetBundlePath:" + filePath);
            bundle = AssetBundle.LoadFromFile(filePath);
            return bundle;
        }

        AssetBundle loadAssetBundle(bool isOld,string abname)
        {
            Debug.Log("loadAssetBundle...");
            AssetBundle bundle = null;
            Debug.Log("abName:" + abname);
            if (!bundleDic.ContainsKey(abname))
            {
                Debug.Log("@Louis loadAssetBundleName:" + abname);
                //loadDependencies(abname);
                string filePath = Path.Combine(GlobalObject.ASSET_PATH, abname);
                //Debug.Log("loadAssetbundlefilePath:"+filePath);
                if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, abname);
                Debug.Log("@Louis loadAssetBundlePath:" + filePath);

                //if (!File.Exists(filePath))
                //{
                //    Debug.LogWarning("文件不存在:" + filePath);
                //    return null;
                //}
                bundle = AssetBundle.LoadFromFile(filePath);

                bundleDic.Add(abname, bundle);
                GlobalObject.OLD_ASSET_PATH = GlobalObject.ASSET_PATH;
            }
            else
            {
                //Debug.Log(oldFilePath + "/////" + GlobalObject.ASSET_PATH);
                if(!isOld)
                {
                    string filePath = Path.Combine(GlobalObject.ASSET_PATH, abname);
                    Debug.Log("@louis2:" + filePath);
                    if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, abname);
                    unloadBundle(abname);
                    bundle = AssetBundle.LoadFromFile(filePath);
                    bundleDic.Add(abname, bundle);
                    GlobalObject.OLD_ASSET_PATH = GlobalObject.ASSET_PATH;
                }
            }
            //foreach (var item in bundleDic[abname].AllAssetNames())
            //{
            //    Debug.Log("abName...."+ item.ToString());

            //}
            return bundleDic[abname];
        }

        AssetBundle loadAssetBundle(string abname)
        {
            Debug.Log("loadAssetBundle...");
            AssetBundle bundle = null;
            Debug.Log("abName:" + abname);
            if (!bundleDic.ContainsKey(abname))
            {
                Debug.Log("@Louis loadAssetBundleName:" + abname);
                //loadDependencies(abname);
                string filePath = Path.Combine(GlobalObject.ASSET_PATH, abname);
                //Debug.Log("loadAssetbundlefilePath:"+filePath);
                if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, abname);
                Debug.Log("@Louis loadAssetBundlePath:" + filePath);

                //if (!File.Exists(filePath))
                //{
                //    Debug.LogWarning("文件不存在:" + filePath);
                //    return null;
                //}
                bundle = AssetBundle.LoadFromFile(filePath);

                bundleDic.Add(abname, bundle);
                oldFilePath = GlobalObject.ASSET_PATH;
            }
            else
            {
                Debug.Log(oldFilePath + "/////" + GlobalObject.ASSET_PATH);
                //if (oldFilePath != GlobalObject.ASSET_PATH)
                {
                    string filePath = Path.Combine(GlobalObject.ASSET_PATH, abname);
                    Debug.Log("@louis2:" + filePath);
                    if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, abname);
                    unloadBundle(abname);
                    bundle = AssetBundle.LoadFromFile(filePath);
                    bundleDic.Add(abname, bundle);
                    oldFilePath = GlobalObject.ASSET_PATH;
                }
              
            }
            //foreach (var item in bundleDic[abname].AllAssetNames())
            //{
            //    Debug.Log("abName...."+ item.ToString());

            //}
            return bundleDic[abname];
        }

        private void loadDependencies(string name)
        {
            Debug.Log("@Louis Dependencies1:" + name);
            Debug.Log("@Louis dependencies:" + manifest == null ? 0 : 1);
            string[] dependencies = manifest.GetAllDependencies(name);
            if (dependencies == null) return;
            Debug.Log("@Louis Dependencies2:" + dependencies.Length);
            int len = dependencies.Length;
            if (len == 0)
            {
                dependencies = dynamicManifest.GetAllDependencies(name);
                len = dependencies.Length;
                if (len == 0) return;
            }
            for (int i = 0; i < len; ++i)
            {
                string depenName = dependencies[i];
                if (!bundleDic.ContainsKey(depenName))
                {
                    string filePath = Path.Combine(GlobalObject.ASSET_PATH, depenName);
                    if (!File.Exists(filePath)) filePath = Path.Combine(Application.streamingAssetsPath, depenName);
                    if (!File.Exists(filePath))
                    {
                        Debug.LogWarning("文件不存在:" + filePath + " 原始资源:" + name);
                    }
                    else
                    {
                        AssetBundle bundle = AssetBundle.LoadFromFile(filePath);
                        bundleDic.Add(depenName, bundle);
                    }
                }
            }
        }

        public void unloadBundle(string abname, bool flag = false)
        {
            abname = abname.ToLower();
            AssetBundle bundle;
            if (bundleDic.TryGetValue(abname, out bundle))
            {
                //Debug.Log("卸载AB:" + abname);
                bundleDic.Remove(abname);
                bundle.Unload(flag);
                Resources.UnloadUnusedAssets();
            }
        }
    }
}