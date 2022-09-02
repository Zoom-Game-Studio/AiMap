using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;

namespace KleinEngine
{
    public class LoadManager:Singleton<LoadManager>
    {
        private List<LoadItem> waitLoadList = new List<LoadItem>();
        private List<LoadItem> loadingList = new List<LoadItem>();
        Tick loadTick;

        public LoadManager()
        {
            loadTick = TickManager.GetInstance().createTick(0, null, handleLoad);
        }

        public LoadItem createLoader(string path,int timeOut,string name = "")   //createLoader(string path,string type,int outTime)
        {
            LoadItem loader = new UnityWebLoader(path,timeOut,name);
            initLoader(loader);
            return loader;
        }

        public void initLoader(LoadItem loader)
        {
            loader.addEvent(LoadItem.LOAD_START, handleLoadStart);
            waitLoadList.Add(loader);
        }

        void handleLoadStart(EventObject ev)
        {
            LoadItem loader = ev.param as LoadItem;
            if(null != loader)
            {
                loader.removeEvent(LoadItem.LOAD_START, handleLoadStart);
                if (waitLoadList.Contains(loader))
                {
                    waitLoadList.Remove(loader);
                    loadingList.Add(loader);
                    loadTick.start();
                }
            }
        }

        void handleLoad()
        {
            int len = loadingList.Count;
            if(len > 0)
            {
                for(int i = len - 1; i >= 0; i--)
                {
                    LoadItem loader = loadingList[i];
                    if(loader.isDone())
                    {
                        loadingList.RemoveAt(i);
                    }
                }
            }
            else
            {
                loadTick.stop();
            }
        }
    }

    public class LoadItem : EventDispatcher
    {
        public const string LOAD_START = "load_start";
        public const string LOAD_UPDATE = "load_update";
        public const string LOAD_COMPLETE = "load_complete";
        protected string loadPath = string.Empty;
        protected int outTime = 0;
        protected int retryCount = 3;
        string loadName = string.Empty;

        public LoadItem(string path, int timeOut, string name)
        {
            loadPath = path;
            outTime = timeOut;
            loadName = name;
            if (outTime < 2) outTime = 2;
        }

        public string getName()
        {
            return loadName;
        }

        public virtual void onStart()
        {
            dispatchEvent(LOAD_START,this);
        }

        public virtual bool isDone()
        {
            return true;
        }

        public virtual byte[] getData()
        {
            return null;
        }

        public virtual string getText()
        {
            return string.Empty;
        }

        public virtual AssetBundle getAssetBundle()
        {
            return null;
        }

        public virtual void onDispose()
        {

        }
    }

    public class UnityWebLoader : LoadItem
    {
        UnityWebRequestAsyncOperation loadAsync;
        float progress = 0;        

        public UnityWebLoader(string path,int timeOut,string name):base(path,timeOut,name)
        {
            //Debug.Log("UnityWebLoader:" + loadPath + ":::" + timeOut);
        }

        public override void onStart()
        {
            //Unity会自动将下载的AssetBundles缓存在本地存储上。如果下载的AssetBundle是LZMA压缩的，
            //则AssetBundle将以未压缩或重新压缩为LZ4（取决于Caching.compressionEnabled设置）的形式存储在缓存中，
            //以便将来加载更快。如果下载的捆绑包压缩了LZ4，
            //则AssetBundle将被压缩存储。如果缓存填满，Unity将从缓存中删除最近最少使用的AssetBundle。
            //设置超时，若m_webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true
            //webRequest.downloadHandler = new DownloadHandlerAssetBundle(string.Empty,0);
            ////AssetBundle ab = DownloadHandlerAssetBundle.GetContent(assetLoadAsync.webRequest);
            UnityWebRequest webRequest = UnityWebRequest.Get(loadPath);
            webRequest.timeout = outTime;
            loadAsync = webRequest.SendWebRequest();
            retryCount = 3;
            base.onStart();
        }

        public override bool isDone()
        {
            if(null == loadAsync) return true;
            if (loadAsync.isDone)
            {
                if (loadAsync.webRequest.isNetworkError || loadAsync.webRequest.isHttpError)
                {
                    Debug.Log("Download Error:" + loadAsync.webRequest.error + "   Path:" + loadPath);
                    retryCount--;
                    if (retryCount <= 0)
                    {
                        dispatchEvent(LOAD_COMPLETE, false);
                    }
                    else
                    {
                        if (null != loadAsync.webRequest) loadAsync.webRequest.Dispose();
                        UnityWebRequest webRequest = UnityWebRequest.Get(loadPath);
                        webRequest.timeout = outTime;
                        loadAsync = webRequest.SendWebRequest();
                        return false;
                    }
                }
                else
                {
                    dispatchEvent(LOAD_COMPLETE, true);
                }
                return true;
            }
            else
            {
                if(loadAsync.progress > progress)
                {
                    progress = loadAsync.progress;
                    dispatchEvent(LOAD_UPDATE, progress);
                }
            }
            return false;
        }

        public override byte[] getData()
        {
            if (null != loadAsync)
                return loadAsync.webRequest.downloadHandler.data;
            return null;
        }

        public override string getText()
        {
            if (null != loadAsync)
                return loadAsync.webRequest.downloadHandler.text;
            return string.Empty;
        }

        public override void onDispose()
        {
            if (null != loadAsync)
            {
                if(null != loadAsync.webRequest) loadAsync.webRequest.Dispose();
                loadAsync = null;
            }
        }
    }

    //支持断点续传
    public class StreamLoader : LoadItem
    {
        UnityWebRequestAsyncOperation loadAsync;
        float progress = 0;
        string filePath = string.Empty;
        long fileLen = 0;
        long totalLength = 0;
        FileStream fileStream = null;
        bool haveContentLen = false;
        int index = 0;

        public StreamLoader(string path, string saveFilePath, int timeOut, string name) : base(path, timeOut, name)
        {
            filePath = saveFilePath;
            if (!File.Exists(filePath))
            {
                string dirName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            }
            fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            fileLen = fileStream.Length;
            fileStream.Seek(fileLen, SeekOrigin.Begin);
            LoadManager.GetInstance().initLoader(this);
        }

        public override void onStart()
        {
            UnityWebRequest headRequest = UnityWebRequest.Head(loadPath);
            loadAsync = headRequest.SendWebRequest();
            retryCount = 3;
            base.onStart();
        }

        public override bool isDone()
        {
            if (null == loadAsync) return true;
            if (loadAsync.isDone)
            {
                if (loadAsync.webRequest.isNetworkError || loadAsync.webRequest.isHttpError)
                {
                    Debug.Log("Download Error:" + loadAsync.webRequest.error + "   Path:" + loadPath);
                    retryCount--;
                    if (retryCount <= 0)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        dispatchEvent(LOAD_COMPLETE, false);
                    }
                    else
                    {
                        if (null != loadAsync.webRequest) loadAsync.webRequest.Dispose();
                        if(haveContentLen)
                        {
                            fileLen = fileStream.Length;
                            index = 0;
                            var request = UnityWebRequest.Get(loadPath);
                            request.SetRequestHeader("Range", "bytes=" + fileLen + "-" + totalLength);
                            loadAsync = request.SendWebRequest();
                        }
                        else
                        {
                            UnityWebRequest headRequest = UnityWebRequest.Head(loadPath);
                            loadAsync = headRequest.SendWebRequest();
                        }
                        return false;
                    }
                }
                else
                {
                    if(!haveContentLen)
                    {
                        totalLength = long.Parse(loadAsync.webRequest.GetResponseHeader("Content-Length"));
                        //Debug.Log("下载文件长度：" + totalLength);
                        haveContentLen = true;
                        loadAsync.webRequest.Dispose();

                        retryCount = 3;

                        var request = UnityWebRequest.Get(loadPath);
                        request.SetRequestHeader("Range", "bytes=" + fileLen + "-" + totalLength);
                        loadAsync = request.SendWebRequest();

                        return false;
                    }
                    else
                    {
                        saveFile();
                        fileStream.Close();
                        fileStream.Dispose();
                        dispatchEvent(LOAD_COMPLETE, true);
                    }
                }
                return true;
            }
            else
            {
                if(haveContentLen)
                {
                    saveFile();
                    if (loadAsync.progress > progress)
                    {
                        progress = loadAsync.progress;
                        dispatchEvent(LOAD_UPDATE, progress);
                    }
                }
            }
            return false;
        }

        void saveFile()
        {
            var buff = getData();
            if (buff != null)
            {
                var length = buff.Length - index;
                if(length > 0)
                {
                    fileStream.Write(buff, index, length);
                    //fileStream.Flush(true);
                    index += length;
                }
            }
        }

        public override byte[] getData()
        {
            if (null != loadAsync)
                return loadAsync.webRequest.downloadHandler.data;
            return null;
        }

        public override string getText()
        {
            if (null != loadAsync)
                return loadAsync.webRequest.downloadHandler.text;
            return string.Empty;
        }

        public override void onDispose()
        {
            if (null != loadAsync)
            {
                if (null != loadAsync.webRequest) loadAsync.webRequest.Dispose();
                loadAsync = null;
            }
        }
    }

}
