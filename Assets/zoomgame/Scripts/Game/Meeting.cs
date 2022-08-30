using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Architecture;
using NRKernal;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using WeiXiang;
using Console = WeiXiang.Console;

namespace zoomgame
{
    public class Meeting : MonoBehaviour
    {
        public Button btn_Unzip;
        public Button btn_load;

        public Button qiu1;
        public Button qiu2;
        public Text tooltip;
        public string zipName = "World_android.zip";
        public string assetId;


        private void Start()
        {
            btn_Unzip.onClick.AddListener(UnZip);
            btn_load.onClick.AddListener(LoadMeetingAssetBundle);
        }

        [Button]
        void FindFunc()
        {
            var trigger1 = GameObject.Find("qiu").GetComponent<EventTrigger>();
            qiu1.onClick.RemoveAllListeners();
            qiu1.onClick.AddListener(() => trigger1.OnPointerClick(null));

            var trigger2 = GameObject.Find("qiu_1").GetComponent<EventTrigger>();
            qiu2.onClick.RemoveAllListeners();
            qiu2.onClick.AddListener(() => trigger2.OnPointerClick(null));
        }

        void UnZip()
        {
            StartCoroutine(CopyAssetInfoFile());
        }

        private IEnumerator CopyAssetInfoFile()
        {
            Debug.Log("Copying AssetInfo.bytes File !!!");
            WWW www = new WWW(Path.Combine(Application.streamingAssetsPath, zipName));
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Copying AssetInfo.bytes should not error!!!");
                www.Dispose();
            }
            else
            {
                var zipPath = Path.Combine(Application.persistentDataPath, zipName);
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                Debug.Log(zipPath);
                //把AssetInfo.bytes写到persitentPath下面去
                File.WriteAllBytes(zipPath, www.bytes);
                www.Dispose();
                ZipUtility.UnzipFile(zipPath, Path.Combine(Application.persistentDataPath, assetId));
                tooltip.text = $"解压成功";
            }
        }

        private void LoadMeetingAssetBundle()
        {
            try
            {
                // TileBuilder.Instantiate(Application.persistentDataPath, assetId);
                var main = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "World_android",
                    "StreamingAssets"));

                var manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                var all = manifest.GetAllAssetBundles();
                var map = new Dictionary<string, AssetBundle>();
                foreach (var name in all)
                {
                    var path = Path.Combine(Application.persistentDataPath, "World_android", name);
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
                
                foreach (var kv in map)
                {
                    Debug.Log(kv.Key);
                }

                var kejihiutang = map["kejihiutang"];
                var kejizhangjiang_topic = kejihiutang.LoadAsset<GameObject>("kejizhangjiang_topic");
                
                Transform root =TileBuilder.GetParent("kejihiutang");
                var go = GameObject.Instantiate(kejizhangjiang_topic, root);
                go.name = "kejihiutang";
                //xxx 写死了相对姿态
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = new Vector3(1, -1, 1);

                tooltip.text = "加载成功\n" + DateTime.Now.ToString();
                // Invoke(nameof(FindFunc),0.1f);
                // ShowScene();
            }
            catch (Exception e)
            {
                tooltip.text = $"{e.Message}\n" + DateTime.Now.ToString();
                throw;
            }
        }

        [Button]
        void ShowScene()
        {
            var robot = GameObject.Find("主题机器人切换");
            robot.SetActive(false);
            robot.transform.parent.Find("kejizhangjiang_WAIC").gameObject.SetActive(true);
        }

        void LoopLog(Transform node)
        {
            Console.Warning(node.name);
            if (node.childCount > 0)
            {
                for (int i = 0; i < node.childCount; i++)
                {
                    LoopLog(node.GetChild(i));
                }
            }
        }
    }
}