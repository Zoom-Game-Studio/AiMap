using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class AppTools : EditorWindow
{
    static string PlatForm = string.Empty;

    static string KeyDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version");

    static string VersionDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version");
    static string VersionDirBak = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "VersionBak");
    const string AppVersionFile = "version.xml";
    const string AssetVersionFile = "asset.xml";
    static string AppVersionFilePath = Path.Combine(VersionDir, AppVersionFile);
    static string AssetVersionFilePath = Path.Combine(VersionDir, AssetVersionFile);

    static Queue<string> updateFileQueue = new Queue<string>();
    static Queue<string> updateDynamicFileQueue = new Queue<string>();
    static string BUCKET = "app-dynamic";

    [MenuItem("AppTools/打包AssetBundle", false, 2)]
    public static void BuildVersion()
    {
        CreateAssetBundle();
        return;
        //Caching.CleanCache ();

        if (!IsCanBuildVersion()) return;

        VersionDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/TestVersion/Src");
        VersionDirBak = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/TestVersion/Bak");
        AppVersionFilePath = Path.Combine(VersionDir, AppVersionFile);
        AssetVersionFilePath = Path.Combine(VersionDir, AssetVersionFile);

        //版本重要文件恢复
        if (!VersionRevert()) return;

        //生成AssetBundle
        CreateAssetBundle();

        //生成asset版本
        if (!CreateAssetVersion()) return;

        //生成app版本
        CreateAppVersion();

        //上传版本
        if (!UploadVersion()) return;

        //版本重要文件备份
        VersionBackup();

        //后期优化，可以添加一个删除原始AssetBundle文件的功能
        //ClearAssetBundle();

        EditorUtility.DisplayDialog("提示", "打包完成", "OK");
    }

    [MenuItem("AppTools/防止误操作", false, 1)]
    public static void onVoid()
    {

    }

    public static bool IsCanBuildVersion()
    {
        //检测网络状况
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            EditorUtility.DisplayDialog("提示", "没有网络，请联网再发布版本", "OK");
            return false;
        }
        string keyFilePath = Path.Combine(KeyDir, "key");
        if (!File.Exists(keyFilePath))
        {
            EditorUtility.DisplayDialog("提示", "密钥文件不存在，无法进行版本发布", "OK");
            return false;
        }
        return true;
    }

    //[MenuItem("AppTools/正式版本/发布")]
    public static void OfficialBuildVersion()
    {
        if (!IsCanBuildVersion()) return;

        VersionDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/OfficialVersion/Src");
        VersionDirBak = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/OfficialVersion/Bak");
        AppVersionFilePath = Path.Combine(VersionDir, AppVersionFile);
        AssetVersionFilePath = Path.Combine(VersionDir, AssetVersionFile);

        //版本重要文件恢复
        if (!VersionRevert()) return;

        //生成AssetBundle
        CreateAssetBundle();

        //生成asset版本
        if (!CreateAssetVersion()) return;

        //生成app版本
        CreateAppVersion();
        //上传版本
        if (!UploadVersion("official")) return;

        //版本重要文件备份
        VersionBackup();

        EditorUtility.DisplayDialog("提示", "打包完成", "OK");
    }

    //[MenuItem("AppTools/审核版本/发布")]
    public static void CheckBuildVersion()
    {
        if (!IsCanBuildVersion()) return;

        VersionDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/CheckVersion/Src");
        VersionDirBak = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version/CheckVersion/Bak");
        AppVersionFilePath = Path.Combine(VersionDir, AppVersionFile);
        AssetVersionFilePath = Path.Combine(VersionDir, AssetVersionFile);

        //版本重要文件恢复
        if (!VersionRevert()) return;

        //生成AssetBundle
        CreateAssetBundle();

        //生成asset版本
        if (!CreateAssetVersion()) return;

        //生成app版本
        CreateAppVersion();
        //上传版本
        if (!UploadVersion("check")) return;

        //版本重要文件备份
        VersionBackup();

        EditorUtility.DisplayDialog("提示", "打包完成", "OK");
    }

    static bool VersionRevert()
    {
        string bakAppVersionFilePath = Path.Combine(VersionDirBak, AppVersionFile);
        if (File.Exists(bakAppVersionFilePath))
            File.Copy(bakAppVersionFilePath, AppVersionFilePath, true);
        string bakAssetVersionFilePath = Path.Combine(VersionDirBak, AssetVersionFile);
        if (File.Exists(bakAssetVersionFilePath))
            File.Copy(bakAssetVersionFilePath, AssetVersionFilePath, true);
        return true;
    }

    static void CreateIOSAssetBundle()
    {
        string resPath = Application.streamingAssetsPath;
        if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
        AssetDatabase.Refresh();
        BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle,BuildTarget.iOS);
        AssetDatabase.Refresh();
    }

    static void CreateAssetBundle()
    {
        string resPath = Application.streamingAssetsPath;
        if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
        AssetDatabase.Refresh();
        BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, GetBuildTarget());
        AssetDatabase.Refresh();
    }

    static bool CreateAssetVersion()
    {
        updateFileQueue.Clear();

        //检测生成版本文件夹
        if (!Directory.Exists(VersionDir)) Directory.CreateDirectory(VersionDir);

        IDictionary localAssetCfgs = null;
        if (File.Exists(AssetVersionFilePath))
        {
            string cfgStr = File.ReadAllText(AssetVersionFilePath);
            //            localAssetCfgs = ConfigManager.GetConfigs<AssetConfigInfo>(cfgStr);
        }

        //设置进度条
        //EditorUtility.DisplayProgressBar("读取中", "正在读取AssetName名称...", 0.50f);
        //EditorUtility.ClearProgressBar();

        //筛选出AssetBundle文件
        List<System.IO.FileInfo> abFilesList = new List<System.IO.FileInfo>();
        DirectoryInfo direction = new DirectoryInfo(Application.streamingAssetsPath);
        System.IO.FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {

            //if (files[i].Name.Contains(".meta")) continue;
            //if (files[i].Name.Contains(".") && (files[i].Name.Contains(".dat") || files[i].Name.Contains(".xml")))
            //{
            //    abFilesList.Add(files[i]);
            //}
            //else
            //    continue;
            if (files[i].Name.Contains(".")) continue;
            abFilesList.Add(files[i]);
            //if ( files[i].Name.Contains(".meta") || 
            //(files[i].Name.Contains(".") && (!files[i].Name.Contains(".dat") ||!files[i].Name.Contains(".xml")))) continue;//可以优化，利用direction.GetFiles("*"...通配符过滤

        }

        string pathTemp = Application.streamingAssetsPath + "/";
#if UNITY_EDITOR_WIN
        //if (!SystemInfo.operatingSystem.Contains("Mac"))
        pathTemp = (Application.streamingAssetsPath + "/").Replace("/", "\\");
#endif

        //比对出需要更新拷贝的文件
        XElement[] xeGroup = new XElement[abFilesList.Count];
        for (int i = 0; i < xeGroup.Length; i++)
        {
            bool needUpdate = true;
            uint crc;
            string name = abFilesList[i].FullName.Replace(pathTemp, "");
            string saveName = name;
            //if (!SystemInfo.operatingSystem.Contains("Mac"))
            //    saveName = saveName.Replace("\\", "/");
#if UNITY_EDITOR_WIN
            saveName = saveName.Replace("\\", "/");
#endif
            if (!BuildPipeline.GetCRCForAssetBundle(abFilesList[i].FullName, out crc))
            {
                EditorUtility.DisplayDialog("提示", "获取文件CRC出错:" + saveName, "OK");
                return false;
            }
            if (null != localAssetCfgs)
            {
                //AssetConfigInfo cfgInfo = localAssetCfgs[saveName] as AssetConfigInfo;
                //if (null != cfgInfo)
                {
                    //if (cfgInfo.crc.Equals(crc.ToString())) needUpdate = false;
                }
            }
            if (needUpdate)
            {
                if (name.Equals("StreamingAssets")) name = "Dynamic_StreamingAssets";
                string copyFileName = Path.Combine("asset", name);
                updateFileQueue.Enqueue(copyFileName);
                string copyPath = Path.Combine(VersionDir, copyFileName);
                string copyDir = Path.GetDirectoryName(copyPath);
                if (!Directory.Exists(copyDir)) Directory.CreateDirectory(copyDir);
                File.Copy(abFilesList[i].FullName, copyPath, true);
            }
            if (saveName.Equals("StreamingAssets"))
                saveName = "Dynamic_StreamingAssets";
            XElement xe = new XElement("item", new XAttribute("id", saveName),
                                               new XAttribute("crc", crc),
                                               new XAttribute("length", abFilesList[i].Length >> 10));
            xeGroup[i] = xe;
        }

        if (updateFileQueue.Count > 0)
        {
            updateFileQueue.Enqueue(AssetVersionFile);
            XElement xElement = new XElement(new XElement("config", xeGroup));
            saveXML(AssetVersionFilePath, xElement);
            Debug.Log("生成asset.xml已完成");
        }
        return true;
    }

    static void CreateAppVersion()
    {
        bool isNeedAppVersion = false;
        string appVersion = Application.version;
        int assetVersion = AppVersionXML.ASSETVERSION;
        string md5 = "";
        if (!File.Exists(AppVersionFilePath))
        {
            isNeedAppVersion = true;
            //            md5 = EngineUtility.GetFileMd5(AssetVersionFilePath);
        }
        else
        {
            //            string cfgStr = File.ReadAllText(AppVersionFilePath);
            //            IDictionary localVersionCfgs = ConfigManager.GetConfigs<AppConfigInfo>(cfgStr);
            //            localVersionCfgs = ConfigManager.GetConfigs<VersionConfigInfo>(cfgStr);
            //            if (null != localVersionCfgs)
            //            {
            //                VersionConfigInfo  versionCfgInfo = localVersionCfgs["asset"] as VersionConfigInfo;
            //                if (null != versionCfgInfo) assetVersion = int.Parse(versionCfgInfo.value);
            //                versionCfgInfo = localVersionCfgs["md5"] as VersionConfigInfo;
            //                if (null != versionCfgInfo) md5 = versionCfgInfo.md5;
            //            }
            //            if (updateFileQueue.Count > 0)
            //            {
            //                assetVersion += 1;
            //                md5 = EngineUtility.GetFileMd5(AssetVersionFilePath);
            //                isNeedAppVersion = true;
            //            }

        }

        if (isNeedAppVersion)
        {
            updateFileQueue.Enqueue(AppVersionFile);

            XElement xElement = new XElement(
             new XElement("config",
                          new XElement("item", new XAttribute("id", AppVersionXML.ASSETID), new XAttribute("value", assetVersion), new XAttribute("md5", md5))
                         )
            );
            saveXML(AppVersionFilePath, xElement);
            Debug.Log("生成version.xml已完成");
        }
    }

    static bool UploadVersion(string uploadPath = "")
    {
        //        if (updateFileQueue.Count > 0)
        //        {
        //            Debug.Log("需要上传的文件数:" + updateFileQueue.Count);
        //            string keyFile = Path.Combine(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Version"), "key");
        //            string[] keyList = File.ReadAllLines(keyFile);
        //            if (null == keyFile || keyFile.Length < 2)
        //            {
        //                EditorUtility.DisplayDialog("提示", "密钥文件有错误", "OK");
        //                return false;
        //            }
        //            string AccessKey = keyList[0];
        //            string SecretKey = keyList[1];
        //            Mac mac = new Mac(AccessKey, SecretKey);
        //            PutPolicy putPolicy = new PutPolicy();
        //            putPolicy.SetExpires(3600);
        //            putPolicy.DeleteAfterDays = 0;
        //            Config config = new Config();
        //            config.Zone = Zone.ZONE_CN_East;
        //            FormUploader target = new FormUploader(config);
        //
        //            while (updateFileQueue.Count > 0)
        //            {
        //                string fileKey = updateFileQueue.Dequeue();
        //                string localfile = Path.Combine(VersionDir, fileKey);
        //                fileKey = Path.Combine(PlatForm, fileKey);
        //                fileKey = Path.Combine(uploadPath, fileKey);
        //#if UNITY_EDITOR_WIN
        //                fileKey = fileKey.Replace("\\", "/");
        //#endif
        //                putPolicy.Scope = BUCKET + ":" + fileKey;
        //                string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //                Debug.Log(localfile);
        //                HttpResult result = target.UploadFile(localfile, fileKey, token, null);
        //                if (result.Code != (int)HttpCode.OK)
        //                {
        //                    Debug.Log("form upload result: " + result.ToString());
        //                    EditorUtility.DisplayDialog("提示", "上传文件失败:" + fileKey, "OK");
        //                    return false;
        //                }
        //                Debug.Log("上传文件成功: " + fileKey);
        //            }
        //        }
        //
        //        Debug.Log("上传云服务器阶段已完成");
        return true;
    }

    static void VersionBackup()
    {
        if (!Directory.Exists(VersionDirBak)) Directory.CreateDirectory(VersionDirBak);
        if (File.Exists(AppVersionFilePath))
        {
            string bakAppVersionFilePath = Path.Combine(VersionDirBak, AppVersionFile);
            File.Copy(AppVersionFilePath, bakAppVersionFilePath, true);
        }
        if (File.Exists(AssetVersionFilePath))
        {
            string bakAssetVersionFilePath = Path.Combine(VersionDirBak, AssetVersionFile);
            File.Copy(AssetVersionFilePath, bakAssetVersionFilePath, true);
        }
        Debug.Log("版本重要文件备份阶段已完成");
    }

    public struct AppVersionXML
    {
        public const string ASSETID = "asset";
        public const int ASSETVERSION = 1;
    }

    static void saveXML(string filePath, XElement element)
    {
        //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = new UTF8Encoding(false);
        settings.Indent = true;
        XmlWriter xw = XmlWriter.Create(filePath, settings);
        element.Save(xw);
        //写入文件
        xw.Flush();
        xw.Close();
    }

    static private BuildTarget GetBuildTarget()
    {
        BuildTarget target = BuildTarget.StandaloneWindows;
#if UNITY_STANDALONE
		target = BuildTarget.StandaloneWindows;
        PlatForm = "pc";
#elif UNITY_IPHONE
        target = BuildTarget.iOS;
        PlatForm = "ios";
#elif UNITY_ANDROID
        target = BuildTarget.Android;
        PlatForm = "android";
#elif UNITY_WSA
        target = BuildTarget.WSAPlayer;
        PlatForm = "uwp";
#endif
        Debug.Log("BuildTarget " + target);
        return target;
    }


}