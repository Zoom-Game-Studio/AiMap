using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;

namespace AppLogic
{

    public class GameManager : MonoBehaviour
    {
        public bool isDebug = false;
        private void Awake()
        {
            Debug.unityLogger.logEnabled = isDebug;
            gameObject.AddComponent<SDKManager>();
            gameObject.AddComponent<DownloadManager>();
            //运行期间一直存在
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            AppEngine.GetInstance().StartCommand(typeof(StartupCommand));
            SDKManager.PhoneMethodForUnityInitComplete();
        }
    }
}
