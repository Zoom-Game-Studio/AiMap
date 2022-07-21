using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;
using System;
using System.IO;

public class DownloadMediator : BaseMediator
{
    public Dictionary<string, string> assetsPathDic = new Dictionary<string, string>();
    public string currentAssetPath;
    public string currentAssetID;
    private DownloadFiles currentDownloadFiles = new DownloadFiles();
    private int downloadAssetsCount = 0;

    public DownloadMediator()
    {
        m_mediatorName = MEDIATOR.DOWNLOAD;
    }
    protected override void onInit()
    {
        addModuleEvent(ModuleEventType.START_DOWNLOAD, HandleStartDownload);
        addModuleEvent(ModuleEventType.CHECK_ASSETS_LIST, HandleCheckAssetsList);
    }

    /// <summary>
    /// 检查本地文件是否存在 监听
    /// </summary>
    /// <param name="ev"></param>
    private void HandleCheckAssetsList(EventObject ev)
    {
        Debug.Log("HandleCheckAssetsList");
        DownloadFiles files = ev.param as DownloadFiles;
        if (files == null) { return; }
        CheckLocalFilesExist(files);
    }

    /// <summary>
    /// 判断文件是否在本地存在
    /// </summary>
    /// <param name="files"></param>
    private void CheckLocalFilesExist(DownloadFiles files)
    {
        Debug.Log("CheckLocalFilesExist...");
        if (files == null) { return; }
        if (files.downloadFilePath.Count == 0)
        {
            //AREventUtil.DispatchEvent(GlobalOjbects.GET_CHECK_FILE_EXIST, false);
            return;
        }
        for (int i = 0; i < files.downloadFilePath.Count; i++)
        {
        }
    }

    /// <summary>
    /// 开始下载多文件监听
    /// </summary>
    /// <param name="ev"></param>
    private void HandleStartDownload(EventObject ev)
    {
        Debug.Log("HandleStartDownload...");
        DownloadFiles files = ev.param as DownloadFiles;
        if (files == null) { return; }
        currentDownloadFiles = files;
        downloadAssetsCount = 0;
        for (int i = 0; i < files.downloadFilePath.Count; i++)
        {
            string fileName = UnityUtilties.GetFileNameFromUrl(files.downloadFilePath[i].downloadPath);
            //string folderPath = GlobalOjbects.LOCAL_FILE_PATH + AppManager.Instance.currentSceneID + "/";
            //currentDownloadFiles.downloadFilePath[i].localFilePath = folderPath + fileName;

        }
    }


    /// <summary>
    /// 多文件下载单个回调
    /// </summary>
    private void DownloadProgress()
    {
        Debug.Log("Download one success");
        downloadAssetsCount += 1;
        if (downloadAssetsCount.Equals(currentDownloadFiles.downloadFilePath.Count))
            OnDownloadAssetCompleted();
    }

    /// <summary>
    /// 下载完成回调
    /// </summary>
    private void OnDownloadAssetCompleted()
    {
        if (currentDownloadFiles != null)
        {
            //AREventUtil.DispatchEvent(GlobalOjbects.GET_CHECK_FILE_EXIST, currentDownloadFiles);
        }
    }

    private void DownloadAAsset(string sceneID, string downloadPath, Action callBack)
    {
        HttpDownLoad download = new HttpDownLoad();
        string fileName = UnityUtilties.GetFileNameFromUrl(downloadPath);
        //string folderPath = GlobalOjbects.LOCAL_FILE_PATH + sceneID + "/";
        //download.DownLoad(downloadPath, folderPath, fileName, callBack);
    }

    /// <summary>
    /// 检测待下载资源列表中是否完整被下载
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public bool CheckAssetsList(DownloadFiles files)
    {
        Debug.Log("CheckAssets...");
        if (files == null) { return false; }
        if (files.downloadFilePath == null) { return false; }
        for (int i = 0; i < files.downloadFilePath.Count; i++)
        {
        }
        return false;
    }

    /// <summary>
    /// 检测资源是否存在
    /// </summary>
    public bool CheckAsset(string sceneId, string downloadPath)
    {
        //if (sceneId == null && downloadPath == null) { Debug.Log("assetContent is not right"); return false; }
        ////currentModelContent = assetContent;
        //string fileName = UnityUtilties.GetFileNameFromUrl(downloadPath);
        ////string folderPath = GlobalOjbects.LOCAL_FILE_PATH + sceneId + "/";
        ////currentAssetPath = Path.Combine(folderPath, fileName);
        //bool isExist = FileHelper.IsFile(folderPath, fileName);
        //return isExist;
        return false;
        //HttpDownLoad download = new HttpDownLoad();
        //download.JudgeExistence(currentAssetPath, assetContent.downloadPath, CheckAssetVersion);
    }
    protected override void onButtonClick(EventObject ev)
    {
        base.onButtonClick(ev);
    }
}
