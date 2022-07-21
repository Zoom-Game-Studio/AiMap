using System;
using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;

namespace AppLogic
{
    public class SceneEnterCommand : BaseCommand
    {
        public override void onExecute(object param)
        {
            Debug.Log("SceneEnterCommand onExecute...");
            string sceneName = param.ToString();
            switch (sceneName)
            {
                case SCENE_NAME.AIMAPSCENE:
                    {
                        //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.PICK_PART);
                        onCommonScene();
                    }
                    break;
                case SCENE_NAME.MAINSCENE:
                    {
                        //这些模块应该由前一个场景模块去关闭，基于目前只有两个场景，暂时这样做
                        //sendModuleEvent(ModuleEventType.MODULE_EXIT, MEDIATOR.SKY_BOX);
                        //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MAIN);

                        //if (getProxy<CarProxy>().getStoreFlag()) getProxy<CarProxy>().setStoreFlag(false);
                        //StoreInfo storeInfo = getProxy<CarProxy>().getStoreScheme();
                        //if (null != storeInfo)
                        //{
                        //    sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.STORE);
                        //    sendModuleEvent(ModuleEventType.MODULE_EXIT, MEDIATOR.MAIN);
                        //    sendModuleEvent(ModuleEventType.STORE_INFO_GET, storeInfo);
                        //}
                    }
                    break;
            }
            Debug.Log("场景进入:" + sceneName);
        }

        void onCommonScene()
        {
            Debug.Log("onCommonScene...");
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.CAR_SCENE);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.SCENE_OPERATE);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.SOCIAL);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MAIN);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MODE_SWITCH);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.LOCALIZE);
            //sendModuleEvent(ModuleEventType.MODULE_EXIT, MEDIATOR.MODE_SWITCH);
        }

        void onLoadAssetCallBackStore(bool succFlag)
        {
            if (succFlag)
            {
               
            }
        }

        void onLoadAssetCallBack(bool succFlag)
        {
            if (succFlag)
            {

            }
        }
    }
}
