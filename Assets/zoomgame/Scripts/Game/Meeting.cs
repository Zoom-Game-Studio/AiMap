using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using Architecture;
using NRKernal;
using TMPro;
using UnityEngine;
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
        public Text tooltip;
        public string zipName = "World_android.zip";
        public string assetId;
        private void Start()
        {
            btn_Unzip.onClick.AddListener(UnZip);
            btn_load.onClick.AddListener(LoadMeetingAssetBundle);
        }

        void UnZip()
        {
            StartCoroutine(CopyAssetInfoFile());
        }
        
        private IEnumerator CopyAssetInfoFile()
        {
            Debug.Log ("Copying AssetInfo.bytes File !!!");
            WWW www = new WWW (Path.Combine(Application.streamingAssetsPath,zipName));
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log ("Copying AssetInfo.bytes should not error!!!");
                www.Dispose ();
            }
            else
            {
                var zipPath = Path.Combine(Application.persistentDataPath,zipName);
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                Debug.Log(zipPath);
                //把AssetInfo.bytes写到persitentPath下面去
                File.WriteAllBytes (zipPath, www.bytes);
                www.Dispose ();
                ZipUtility.UnzipFile(zipPath, Path.Combine(Application.persistentDataPath,assetId));
                tooltip.text = $"解压成功";
            }
        }

        private void LoadMeetingAssetBundle()
        {
            try
            {
                TileBuilder.Instantiate(Application.persistentDataPath, assetId);
                tooltip.text = "加载成功\n" + DateTime.Now.ToString();
                LoopLog(LocalizationConvert.Origin);
            }
            catch (Exception e)
            {
                tooltip.text = $"{e.Message}\n" + DateTime.Now.ToString();
                throw;
            }
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