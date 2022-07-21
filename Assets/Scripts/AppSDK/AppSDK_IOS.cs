using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_IOS
public class AppSDK_IOS : IAppSDK
{
    #region 调用外部API

    [DllImport("_Internal")]
    private static extern void PhoneMethodForGetCurrentView(string path);
    [DllImport("_Internal")]
    private static extern void PhoneMethodForUnityInitComplete();
    [DllImport("_Internal")]
    private static extern void PhoneMethodForUnloadAssetsComplete();
    [DllImport("_Internal")]
    private static extern void PhoneMethodForLoadingAreaAssetsDone();
    [DllImport("_Internal")]
    private static extern void PhoneMethodForOpenUrlFromUnity(string json);
    [DllImport("_Internal")]
    private static extern void PhoneMethodForProcessDone(string json);
    [DllImport("_Internal")]
    private static extern void PhoneMethodForSendParallelverseInfo(string json);

    #endregion


    #region 调用外部API
    public void SendParallelverseInfo(string json)
    {
#if !UNITY_EDITOR
        PhoneMethodForSendParallelverseInfo(json);
#endif
    }

    public void ProcessDone(string json)
    {
#if !UNITY_EDITOR
        PhoneMethodForProcessDone(json);
#endif
    }

    public void UnloadAssetsComplete()
    {
#if !UNITY_EDITOR
        PhoneMethodForUnloadAssetsComplete();
#endif
    }

    public void GetCurrentView(string path)
    {
#if !UNITY_EDITOR
        PhoneMethodForGetCurrentView(path);
#endif
    }

    public void UnityInitComplete()
    {
#if !UNITY_EDITOR
        PhoneMethodForUnityInitComplete();
#endif
    }

    public void LoadingAreaAssetsDone()
    {
#if !UNITY_EDITOR
        PhoneMethodForLoadingAreaAssetsDone();
#endif
    }

    public void OpenUrlFromUnity(string json)
    {
#if !UNITY_EDITOR
        PhoneMethodForOpenUrlFromUnity(json);
#endif
    }

    #endregion

}
#endif