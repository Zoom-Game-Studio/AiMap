using KleinEngine;
using UnityEngine;
using System.Collections;

namespace AppLogic
{
    public class InitCommand : BaseCommand
    {
        public override void onExecute(object param)
        {
            //ResourceManager.GetInstance().init(true);
            //if(AppUtil.IS_STOP_SERVICE)
            //{
            //    string str = ConfigManager.GetLanguage("stop_service");
            //    sendModuleEvent(ModuleEventType.UPDATE_FAIL, str);
            //    return;
            //}
            //AppFacade.GetInstance().removeMediator(MEDIATOR.VERSION);

            //注意：执行顺序不要随意调整
            //initManager();
            initSetting();

            //注册控制模块
            AppFacade.GetInstance().registerMediator(new ControlMediator());

            AppFacade.GetInstance().registerMediator(new LoadingMediator());

            //注册公共Proxy
            //registerProxy<AppProxy>();
            //registerProxy<UserProxy>();
            //registerProxy<CarProxy>(); 
            //registerProxy<VideoProxy>();
            //registerProxy<PhotoProxy>();
            //registerProxy<AssetProxy>();
            //registerProxy<SchemeProxy>();
            //registerProxy<RankProxy>();
            //registerProxy<StoreProxy>();

            //初始化预开启模块
            InitModule();

            removeCommand(ModuleEventType.APP_INIT);
        }

        void initManager()
        {
            //TextAsset key = ResourceManager.GetInstance().loadAsset<TextAsset>("data", "key");
            //CodecManager.GetInstance().initRSA(key.text);
            //CodecManager.GetInstance().initAES();
        }

        void initSetting()
        {
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            // 多点触控
            Input.multiTouchEnabled = true;
        }

        void InitModule()
        {
            Debug.Log("InitModule...");

            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_AIMAP_POI);//进入POI模块

            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_BULLET_SCREEN);//进入3D文字模块


            //AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_VISIBLE_DISTANCE);

            //IDictionary parallelverses = ConfigManager.GetConfigs<ParallelverseConfigInfo>();
            //Debug.Log(parallelverses.Count);
            //foreach (ParallelverseConfigInfo parallelverse in parallelverses.Values)
            //{
            //    Debug.Log(parallelverse.id);
            //    Debug.Log(parallelverse.name);
            //    Debug.Log(parallelverse.remark);
            //    if (parallelverse.parallelverse == null) continue;
            //    foreach (var item in parallelverse.parallelverse)
            //    {
            //        Debug.Log(item);
            //    }
            //}
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.LOCALIZE);

            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.LOADING);

            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MODE_SWITCH);

            //sendModuleEvent(ModuleEventType.LOADING_SCENE, SCENE_NAME.AIMAPSCENE);

            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MAIN);

            //            if (AppUtil.IS_TEST)
            //            {
            //                sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.INFO);
            //            }

            //#if UNITY_EDITOR
            //            if (AppUtil.DEBUG_OPEN) sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.DEBUG);
            //#endif

            //            sendModuleEvent(ModuleEventType.NET_LOGIN);
        }
    }
}
