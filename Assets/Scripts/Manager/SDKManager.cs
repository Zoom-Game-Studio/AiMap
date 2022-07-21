using LitJson;
using UnityEngine;
using KleinEngine;
using System.IO;

namespace AppLogic
{
    public class SDKManager : MonoBehaviour
    {
#if UNITY_IOS
        static AppSDK_IOS appSDK = new AppSDK_IOS();
#else
        static AppSDK_Android appSDK = new AppSDK_Android();
#endif
        //public static AppSDK_Android appSDK;
        private void Awake()
        {
            //#if UNITY_IOS
            //            appSDK=new AppSDK_IOS();
            //#else
            //            appSDK = new AppSDK_Android();
            //#endif
        }


        #region 提供为原生开放的方法

        /// <summary>
        /// 切换平行宇宙资源（同场景同位置不同资源）
        /// </summary>
        public void SwitchParallelUniverse(string id)
        {
            if (id == null) return;
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SWITCH_PARALLEL_UNIVERSE, id);
        }


        /// <summary>
        /// 是否开启融合定位检测功能
        /// </summary>
        /// <param name="enable"></param>
        public void EnableConsistencyCheck(string enable)
        {
            Debug.Log("EnableConsistencyCheck..." + enable);
            bool isEnable = true;
            if (enable.Equals("1"))
                isEnable = true;
            else
                isEnable = false;

            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ENABLE_CONSISTENCY_CHECK, isEnable);
        }

        /// <summary>
        /// 可见距离变化
        /// </summary>
        public void VisibleDistanceChanged(string distance)
        {
            Debug.Log("VisibleDistanceChanged...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.VISIBLE_DISTANCE_CHANGED, distance);
        }

        /// <summary>
        /// 获取当前地点已有的3d字体
        /// </summary>
        /// <param name="json"></param>
        public void GetThreeDText(string json)
        {
            Debug.Log("GetThreeDText...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.GET_THREE_D_TEXT, json);
        }

        /// <summary>
        /// 根据用户输入，创建一个3d字体
        /// </summary>
        /// <param name="json"></param>
        public void CreateAThreeDText(string json)
        {
            Debug.Log("CreateAThreeDText...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.CREATE_A_THREE_D_TEXT, json);

        }

        /// <summary>
        /// 显示fps 、隐藏fps
        /// </summary>
        public void SetFPSVisible(string isShow)
        {
            bool isActive = isShow.Equals("1") ? true : false;
            FPSDisplay disPlay = Camera.main.GetComponent<FPSDisplay>();
            if (disPlay)
                disPlay.enabled = isActive;
        }

        /// <summary>
        /// 从安卓端接收信息
        /// </summary>
        public void ReceiveMessageFromPhone(string json)
        {
            Debug.Log("ReceiveMessageFromPhone..." + json);
            if (json == null) return;
            UrlFromUnity urlFromUnity = JsonMapper.ToObject<UrlFromUnity>(json);
            if (urlFromUnity == null) return;
            Debug.Log("json type:" + urlFromUnity.type);
            switch (urlFromUnity.type)
            {
                case "1":
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_QUESTION, urlFromUnity);
                    break;
                case "2":
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.WAYZ9L_NEW_YEAR_SEND_ANSWER, urlFromUnity);
                    break;
                case "3":
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_RESULT, urlFromUnity);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 卸载当前加载进入场景的资源
        /// </summary>
        public void UnloadUnityAssets()
        {
            Debug.Log("UnloadUnityAssets...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.UNLOAD_UNITY_ASSETS);
        }

        /// <summary>
        /// 从原生端获取POI的信息
        /// </summary>
        /// <param name="json"></param>
        public void GetPOIInfoFromPhone(string json)
        {
            Debug.Log("GetPOIInfoFromPhone..." + json);
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.GET_POI_INFO_FROM_PHONE, json);
        }

        /// <summary>
        /// 获取定位结果
        /// </summary>
        /// <param name="json"></param>
        public void GetPostionResultFromPhone(string json)
        {
            Debug.Log("GetPostionResult:" + json);
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.FOUND_MAPS, json);
        }
        bool inited = false;

        public void GetCurrentAreaAssets(string json)
        {
            Debug.Log("GetCurrentAreaAssets:" + json);
            LocalAreaAssetsFile assetsFile = JsonMapper.ToObject<LocalAreaAssetsFile>(json);
            if (assetsFile == null) return;
            GlobalObject.ASSET_PATH = assetsFile.contentPath;
            GlobalObject.SCREEN_ORIENTATION = assetsFile.screenOrientation;
            Debug.Log("Asset_Path:" + GlobalObject.ASSET_PATH);
            string filePath = Path.Combine(GlobalObject.ASSET_PATH, "data");
            Debug.Log("filePath:" + filePath);

            if (inited)
            {
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.GET_AREA_ASSETS_FILES);
                return;
            }
            //AppEngine.GetInstance().StartCommand(typeof(StartupCommand));

            OnInitLocalizeMediator();
            //AppFacade.GetInstance().dispatchEvent(ModuleEventType.GET_AREA_ASSETS_FILES, json);
            //AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_AIMAP_POI);
        }


        private void OnInitLocalizeMediator()
        {
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.LOCALIZE);
            inited = true;
        }

        public void SendCurrentViewToAndroid()
        {
            Debug.Log("SendCurrentViewToAndroid...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_POSITION);
        }

        #endregion

        #region 调用原生方法

        /// <summary>
        /// 发送平行宇宙数据给原生端
        /// </summary>
        /// <param name="json"></param>
        public static void PhoneMethodForSendParallelverseInfo(string json)
        {
            Debug.Log("PhoneMethodForSendParallelverseInfo...");
            appSDK.SendParallelverseInfo(json);
        }


        /// <summary>
        /// 3d操作完成，返回给原生端结果
        /// </summary>
        /// <param name="json"></param>
        public static void PhoneMethodForProcessDone(string json)
        {
            Debug.Log("PhoneMethodForAddThreeDText...");

            appSDK.ProcessDone(json);//1 代表完成创建3D字体
        }

        public static void PhoneMehtodForOpenUrlFromUnity(string json)
        {
            Debug.Log("PhoneMehtodForOpenUrlFromUnity...");
            appSDK.OpenUrlFromUnity(json);

        }
        public static void PhoneMethodForUnloadAssetsComplete()
        {
            Debug.Log("PhoneMethodForUnloadAssetsComplete");
            appSDK.UnloadAssetsComplete();
        }

        public static void PhoneMethodForUnityInitComplete()
        {
            Debug.Log("PhoneMethodForUnityInitComplete...");
            appSDK.UnityInitComplete();
        }

        public static void PhoneMethodForLoadingAreaAssetsDone()
        {
            Debug.Log("PhoneMethodForLoadingAreaAssetsDone...");
            appSDK.LoadingAreaAssetsDone();
        }

        /// <summary>
        /// 发送当前定位图片
        /// </summary>
        public static void SendCurrentViewToPhone(string json)
        {
            Debug.Log("SendCurrentViewFromUnity...");
            appSDK.GetCurrentView(json);
        }

        #endregion
    }
}