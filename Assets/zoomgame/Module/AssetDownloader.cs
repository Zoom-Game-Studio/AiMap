using System;
using System.Collections;
using System.IO;
using BestHTTP;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using AssetList = System.Collections.Generic.List<Waku.Module.AssetInfoItem>;

namespace Waku.Module
{
    public class AssetDownloader : MonoBehaviour, IResDownload<AssetList>
    {
        public string url = @"https://aimap.newayz.com/aimap/ora/v1/scenes?device_type=android";
        public string token = "ade820503cc6069ff507346a6ef7fec3";

#if UNITY_EDITOR
        public string fullPath => Path.Combine(Application.streamingAssetsPath, nameof(AssetDownloader));
#elif UNITY_ANDROID
        public string fullPath => Path.Combine(Application.persistentDataPath, nameof(AssetDownloader));
#endif

        public AssetList ServerList { get; set; }
        public AssetList ClientList { get; set; }
        public AssetList DownloadList { get; set; }

        public void Init()
        {
            var path = fullPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //读取本地资源列表
            ClientList = LoadListFromPath(nameof(ClientList), path);
            RequestSeverList();
        }

        [Button]
        private void RequestSeverList()
        {
            var request = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnFinishRequestServerList);
            request.Send();
        }

        private void OnFinishRequestServerList(HTTPRequest request, HTTPResponse response)
        {
            if (request != null && response != null && request.State == HTTPRequestStates.Finished)
            {
                try
                {
                    var data = response.DataAsText;
                    ServerList = JsonConvert.DeserializeObject<AssetList>(data);
                    Debug.Log("Server list load complete");
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    ServerList = new AssetList();
                    Debug.LogError("Server list load fail");
                }
            }
        }

        /// <summary>
        /// 把服务器资源信息添加到下载列表
        /// </summary>
        /// <param name="serverItemInfo"></param>
        void AddToDownloadList(AssetInfoItem serverItemInfo)
        {
            //需要下载的item
            var item = serverItemInfo.Clone();
            //获取本地存在的记录
            var local = ClientList.Find(e => e.id.Equals(item.id));
            if (local != null)
            {
                //本地存在记录
                var localFiles = local.package.files;
                var serverFiles = item.package.files;
                for (var x = serverFiles.Count - 1; x >= 0; x--)
                {
                    for (var y = localFiles.Count - 1; y >= 0; y--)
                    {
                        var serverFile = serverFiles[x];
                        var localFile = localFiles[y];
                        //移除下载列表中本地已经存在的file，此文件不需要更新
                        if (serverFile.updateTime.Equals(localFile.updateTime))
                        {
                            serverFiles.RemoveAt(x);
                        }
                        //移除本地列表中已经存在的文件，此文件需要更新
                        else
                        {
                            localFiles.RemoveAt(y);
                        }
                    }
                }
            }
            DownloadList.Add(item);
        }

        void RemoveFile()
        {
            System.IO.Directory.Delete();
        }


        AssetList LoadListFromPath(string name, string path)
        {
            AssetList list;
            var listPath = Path.Combine(path, name);
            if (System.IO.File.Exists(listPath))
            {
                using var sr = new StreamReader(listPath);
                try
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetList>(sr.ReadToEnd());
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    list = new AssetList();
                }
            }
            else
            {
                list = new AssetList();
            }

            return list;
        }
    }
}