using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAppSDK
{
    void GetCurrentView(string path);
    void LoadingAreaAssetsDone();
    void UnityInitComplete();
    void OpenUrlFromUnity(string json);
    void ProcessDone(string json);
    void SendParallelverseInfo(string json);
}
