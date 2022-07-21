using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidRuntimePermission
{
    private AndroidRuntimePermission() { }

    private static AndroidJavaObject GetActivity()
    {
        var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        
          return UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        //}
        //using (var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        //{
        //    return UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        //}
    }

    private static int GetAndroidSDKVersion()
    {
        var VERSION = new AndroidJavaClass("android.os.Build$VERSION");
       
            return VERSION.GetStatic<int>("SDK_INT");
     
        //using (var VERSION = new AndroidJavaClass("android.os.Build$VERSION"))
        //{
        //    return VERSION.GetStatic<int>("SDK_INT");
        //}
    }

    public static bool HasPermission(string permission)
    {
        if (GetAndroidSDKVersion() >= 23)
        {
            var currentActivity = GetActivity();
            return currentActivity.Call<int>("checkSelfPermission", permission) == 0;
        }

        return true;
    }

    public static bool ShouldShowRequestPermissionRationale(string permission)
    {
        if (GetAndroidSDKVersion() >= 23)
        {
            var currentActivity = GetActivity();
            return currentActivity.Call<bool>("shouldShowRequestPermissionRationale", permission);
        }

        return false;
    }

    public static void RequestPermissions(string[] permissiions)
    {
        if (GetAndroidSDKVersion() >= 23)
        {
            var currentActivity = GetActivity();
            currentActivity.Call("requestPermissions", permissiions, 0);
        }
    }
}