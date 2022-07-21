using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSDK_Android : IAppSDK
{
    #region 调用外部API
    public void PhoneMethodForSendParallelverseInfo(string json)
    {
        Debug.Log("PhoneMethodForParallelverseInfo====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForSendParallelverseInfo", json);
#endif
    }

    public void PhoneMethodForProcessDone(string json)
    {
        Debug.Log("PhoneMethodForProcessDone====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForProcessDone", json);
#endif
    }

    public void PhoneMethodForOpenUrlFromUnity(string json)
    {
        Debug.Log("PhoneMethodForUnloadAssetsComplete====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForOpenUrlFromUnity",json);
#endif
    }

    public void PhoneMethodForUnloadAssetsComplete()
    {
        Debug.Log("PhoneMethodForUnloadAssetsComplete====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForUnloadAssetsComplete");
#endif
    }

    public void PhoneMethodForUnityInitComplete()
    {
        Debug.Log("PhoneMethodForUnityInitComplete====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForUnityInitComplete");
#endif
    }

    public void PhoneMethodForLoadingAreaAssetsDone()
    {
        Debug.Log("PhoneMethodForLoadingAreaAssetsDone====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForLoadingAreaAssetsDone");
#endif
    }
    public void PhoneMethodForGetCurrentView(string path)
    {
        Debug.Log("PhoneMethodForGetCurrentView====");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject jo = new AndroidJavaObject("com.unity3d.player.UnityPlayerActivity");
        string callBackStr = jo.CallStatic<string>("PhoneMethodForGetCurrentView", path);
#endif
    }

    public void SendParallelverseInfo(string json)
    {
        PhoneMethodForSendParallelverseInfo(json);
    }

    public void GetCurrentView(string path)
    {
        PhoneMethodForGetCurrentView(path);
    }

    public void LoadingAreaAssetsDone()
    {
        PhoneMethodForLoadingAreaAssetsDone();
    }

    public void UnityInitComplete()
    {
        PhoneMethodForUnityInitComplete();
    }

    public void UnloadAssetsComplete()
    {
        PhoneMethodForUnloadAssetsComplete();
    }

    public void OpenUrlFromUnity(string json)
    {
        PhoneMethodForOpenUrlFromUnity(json);
    }

    public void ProcessDone(string json)
    {
        PhoneMethodForProcessDone(json);
    }

    #endregion
}