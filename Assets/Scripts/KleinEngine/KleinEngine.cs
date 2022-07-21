using UnityEngine;
using System;

namespace KleinEngine
{
    //设置必要的组件（比如AudioSource）把UI也创建出来
    public class AppEngine : Singleton<AppEngine>
    {
        public void StartCommand(Type startCommand)
        {
            AppFacade.GetInstance().registerCommand(AppFacade.STARTUP, startCommand);
            GameObject obj = GameObject.Find("KleinEngine");
            if (null == obj)
            {
                obj = new GameObject("KleinEngine");
                obj.AddComponent<KleinEngine>();
            }
            else
            {
                KleinEngine engine = obj.GetComponent<KleinEngine>();
                if(null == engine) obj.AddComponent<KleinEngine>();
            }            
        }

        class KleinEngine : MonoBehaviour
        {
            void Awake()
            {
                //整个游戏期间一直存在
                DontDestroyOnLoad(gameObject);
                //Application.runInBackground = true;
                //Application.targetFrameRate = 60;
                //Screen.sleepTimeout = SleepTimeout.NeverSleep;
                // 关闭游戏多点触控
                //Input.multiTouchEnabled = false;
                Debug.Log("~KleinEngine Start");                
            }

            void Start()
            {
                //gameObject.AddComponent<AudioListener>();
                AudioSource audio = gameObject.AddComponent<AudioSource>();
                audio.loop = true;
                SoundManager.GetInstance().setAudioSource(audio);
                AppFacade.GetInstance().startUp();
            }

            void Update()
            {
                AppFacade.GetInstance().update();
            }

            private void LateUpdate()
            {
                AppFacade.GetInstance().lateUpdate();
            }

            void OnDestroy()
            {
                AppFacade.GetInstance().shutDown();
                Debug.Log("~KleinEngine ShutDown");
            }
        }
    }
}
