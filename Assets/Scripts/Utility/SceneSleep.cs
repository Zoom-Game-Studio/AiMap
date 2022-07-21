using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSleep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//手机屏幕常亮
        //Screen.sleepTimeout = SleepTimeout.SystemSetting;  //关闭手机屏幕常亮(按照手机设置)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
