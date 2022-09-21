using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Architecture;
using BestHTTP;
using C_ScriptsTest;
using Newtonsoft.Json;
using NRKernal;
using QFramework.UI;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions.Must;
using WeiXiang;
using zoomgame.Scripts.Architecture.TypeEvent;
using AssetList = System.Collections.Generic.List<Waku.Module.AssetInfoItem>;

namespace Waku.Module
{

    public class AssetDownloader : MonoBehaviour
    {
        public static AssetDownloader Instance { private set; get; }
        public string url = @"https://aimap.newayz.com/aimap/ora/v1/scenes?device_type=android";
        public string token = "ade820503cc6069ff507346a6ef7fec3";
        public string fullPath => _fullPath;
        private string _fullPath;


        public AssetList ServerList { get; set; }
        public AssetList ClientList { get; set; } 
        public AssetList DownloadList { get; set; }

        /// <summary>
        /// 下载完成的列表
        /// </summary> 
        private Queue<AssetInfoItem> doneList { get; set; }

        /// <summary>
        /// 构件目标
        /// </summary>
        private AssetInfoItem buildTarget { get; set; }
        
        class DownloadItem
        {
            public  AssetInfoItem item;
            public bool isDownload = false;
        }

        private void Awake()
        {
            Instance = this;
#if UNITY_EDITOR
            _fullPath = Path.Combine(Application.streamingAssetsPath, nameof(AssetDownloader));
#elif UNITY_ANDROID
            _fullPath = Path.Combine(Application.persistentDataPath, nameof(AssetDownloader));
#endif
        }

        private void Start()
        {
            Init();
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(OnBuildListHasItem).AddTo(this);
        }

        void Init()
        {
            var path = fullPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //读取本地资源列表
            ClientList = LoadListFromPath(nameof(ClientList), path);Debug.LogWarning("读取本地");
            DownloadList = new  ();
            doneList = new ();
            RequestSeverList();
        }

        /// <summary>
        /// 在构建列表不为空时执行构建
        /// </summary>
        /// <param name="_"></param>
        void OnBuildListHasItem(long _)
        {
            if (doneList.Count <= 0)
            {
                return;
            }
            foreach (var info in doneList)
            {
                if (buildTarget!= null && buildTarget.id.Equals(info.id))
                {
                    MessageBroker.Default.Publish(new BuildTileEvent()
                    {
                        name = info.name,
                        place = info.place,
                    });
                    TileBuilder.Instantiate(fullPath, info.id);
                    break;
                }
            }
            doneList.Clear();
        }

        /// <summary>
        /// 通过gps构建场景模型
        /// </summary>
        /// <param name="gps"></param>
        /// <returns></returns>
        public bool TryBuildAssetByGps(Vector2 gps)
        {
            var info = FindAssetByGpsInServerList(gps);

            if (info != null)
            {
                this.AddToDownloadList(info);
                return true;
            }

            Debug.LogWarning("没有找到Gps所在区域的资源：" + gps);
            return false;
        }

        AssetInfoItem FindAssetByGpsInServerList(Vector2 gps)
        {
            foreach (var info in ServerList)
            {
                var list = info.boundary.coordinates[0];
                var boundary = GetVector3List(list);
                if (IsPointInPolygon(gps, boundary))
                {
                    return info;
                }
            }

            return null;
        }

        List<Vector2> GetVector3List(List<List<float>> data)
        {
            List<Vector2> vs = new List<Vector2>();
            for (var i = 0; i < data.Count; i++)
            {
                var p = data[i];
                var v = new Vector2()
                {
                    x = p[0],
                    y = p[1],
                };
                vs.Add(v);
            }

            return vs;
        }

        /// <summary>
        /// 点是否在多边形范围内
        /// </summary>
        /// <param name="p">点</param>
        /// <param name="vertexs">多边形顶点列表</param>
        /// <returns></returns>
        public bool IsPointInPolygon(Vector2 p, List<Vector2> vertexs)
        {
            int crossNum = 0;
            int vertexCount = vertexs.Count;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector2 v1 = vertexs[i];
                Vector2 v2 = vertexs[(i + 1) % vertexCount];

                if (((v1.y <= p.y) && (v2.y > p.y))
                    || ((v1.y > p.y) && (v2.y <= p.y)))
                {
                    if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
                    {
                        crossNum += 1;
                    }
                }
            }

            if (crossNum % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RequestSeverList()
        {
            var request = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnFinishRequestServerList);
            request.AddHeader("token", token);
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
                    // RailTest.ShowBoundary(ServerList);
                    MessageBroker.Default.Publish(new UpdateServerListEvent()
                    {
                        infoList = ServerList,
                    });
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
        /// <param name="callback"></param>
        public void AddToDownloadList(AssetInfoItem serverItemInfo,Action<HttpDownLoad> actLoader=null)
        {
            buildTarget = serverItemInfo;
            var hasInDownloadList = DownloadList.Find(e=>  e.id.Equals(serverItemInfo.id));
            if (hasInDownloadList != null)
            {
                Debug.LogWarning("已经加入下载列表："+serverItemInfo.id);
            }
            else
            {
                //需要下载的item
                var item = serverItemInfo.Clone();
                Debug.Log("准备加载：" + item.id);
                //获取本地存在的记录
                var local = ClientList.Find(e => e.id.Equals(item.id));
                if (local != null)
                {
                    var needUpdate = item.updateTime != local.updateTime;
                    if (!needUpdate)
                    {
                        Debug.LogWarning("跳过更新，没有变更：" + local.id);
                        doneList.Enqueue(local);
                        return;
                    }
                    else
                    {
                        Debug.LogWarning("需要更新本地资源：" + item.id);
                    }

                    //需要更新，删除本地文件
                    DeleteADirectory(Path.Combine(fullPath, local.id));
                }

                DownloadList.Add(item);
                StartDownLoad(item, () =>
                {
                    var find = DownloadList.Find(e => e.id.Equals(item.id));
                    if (find != null)
                    {
                        DownloadList.Remove(find);
                    }
                    doneList.Enqueue(item);
                },actLoader);
            }
        }

        /// <summary>
        /// zip保存的路径为fullpath + info.id + file.name;
        /// </summary>
        void StartDownLoad(AssetInfoItem info , Action callback = null, Action<HttpDownLoad> actLoader = null)
        {
            var file = info.package.files[0];
            Debug.Log("开始下载:" + file.link);
            var loader = new HttpDownLoad();
            var localFullPath = Path.Combine(fullPath, info.id);
            var zipName = file.name ?? "default.zip";
            loader.DownLoad(file.link, localFullPath, zipName, () =>
            {
                Debug.Log("完成下载:" + file.link);
                Debug.Log("解压中：" + zipName);
                var zipFilePath = Path.Combine(localFullPath, zipName);
                var unzipPath = localFullPath;
                ZipUtility.UnzipFile(zipFilePath, unzipPath);
                Debug.Log("解压完成：" + file.name + "," + unzipPath);
                Debug.Log("清理压缩包:" + file.name);
                File.Delete(zipFilePath);
                //更新本地资源列表
                var localItem = ClientList.Find(e => e.id == info.id);
                if (localItem != null)
                {
                    ClientList.Remove(localItem);
                }

                ClientList.Add(info);
                SaveClientList();
                callback?.Invoke();
            });
            actLoader?.Invoke(loader);
            MessageBroker.Default.Publish(new DownloadEvent()
            {
                resLoader = loader,
            });

        }

        /// <summary>
        /// 保存客户端列表
        /// </summary>
        void SaveClientList()
        {
            var str = JsonConvert.SerializeObject(ClientList);
            using var sw = new StreamWriter(Path.Combine(fullPath, nameof(ClientList)));
            sw.Write(str);
        }

        /// <summary>  
        /// 解决删除目录提示：System.IO.IOException: 目录不是空的。  
        /// 删除一个目录，先遍历删除其下所有文件和目录（递归）  
        /// </summary>  
        /// <param name="strPath">绝对路径</param>  
        /// <returns>是否已经删除</returns>  
        public static bool DeleteADirectory(string strPath)
        {
            string[] strTemp;
            try
            {
                //先删除该目录下的文件  
                strTemp = System.IO.Directory.GetFiles(strPath);
                foreach (string str in strTemp)
                {
                    System.IO.File.Delete(str);
                }

                //删除子目录，递归  
                strTemp = System.IO.Directory.GetDirectories(strPath);
                foreach (string str in strTemp)
                {
                    DeleteADirectory(str);
                }

                //删除该目录  
                System.IO.Directory.Delete(strPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 加载本地文件清单
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        AssetList LoadListFromPath(string listName, string path)
        {
            AssetList list;
            var listPath = Path.Combine(path, listName);
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