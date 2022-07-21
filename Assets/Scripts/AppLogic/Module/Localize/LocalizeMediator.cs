using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using LitJson;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using msQuaternion = System.Numerics.Quaternion;
using msVector3 = System.Numerics.Vector3;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System;

namespace AppLogic
{
    public class LocalizeMediator : BaseMediator
    {
        public LocalizeView localizeView;
        public Model6DoFInfo origin6DoFInfo = new Model6DoFInfo();
        public string positionInfo = "{\"causeValue\":0,\"description\":\"succ\",\"deviation\":null,\"ori_qvec\":[-0.679598,-0.623293,0.250852,0.294493],\"ori_tvec\":[366592.663071,3450584.111551,40.865078],\"rotation\":\"-0.428 -0.514 -0.575 -0.471\",\"timestamp\":\"151574384600\",\"translation\":\"0.383 -0.1393 41.065\"}";
        //public string positionInfo = "{\"causeValue\":0,\"description\":\"succ\",\"deviation\":null,\"ori_qvec\":[-0.679598,-0.623293,0.250852,0.294493],\"ori_tvec\":[366592.663071,3450584.111551,40.865078],\"rotation\":\"0.720 0.034 0.045 0.691\",\"timestamp\":\"151574384600\",\"translation\":\"2.098 3.583 41.152\"}";
        public Model6DoFInfo startPosition = new Model6DoFInfo();

        ARCameraManager arCameraManager;
        CameraImageExample cameraImage;
        CameraImageInfo cameraInfo = new CameraImageInfo();
        List<ChangeModelInfo> modelInfoList = new List<ChangeModelInfo>();
        List<ChangeModelInfo> prefabInfoList = new List<ChangeModelInfo>();
        private bool isARInit = false;
        private bool isEnableCheck = false;
        //float scale;
        public LocalizeMediator()
        {
            m_mediatorName = MEDIATOR.LOCALIZE;
        }

        protected override void onInit()
        {
            Debug.Log("LocalizeMediator init...");
            //localizeView = viewComponent as LocalizeView;
            addModuleEvent(ModuleEventType.SET_LOCALIZE_STATUS_TEXT, HandleSetLocalizeStatusText);
            addModuleEvent(ModuleEventType.UI_SET_LOCALIZE_BACKBTN, HandleSetLocalizeBackBth);
            addModuleEvent(ModuleEventType.UI_SET_LOCALIZE_BTN_STATUS, HandleSetLocalizeBtnStatus);
            addModuleEvent(ModuleEventType.SEND_MODEL_PREFAB_PARA, HandleGetModelPrefabsPara);//第一次获取
            addModuleEvent(ModuleEventType.START_POSITION, HandleStartPosition);
            addModuleEvent(ModuleEventType.SEND_CAMERA_INFO, HandleSendCameraInfo);
            addModuleEvent(ModuleEventType.FOUND_MAPS, HandleLoacalizeFoundMaps);
            addModuleEvent(ModuleEventType.SEND_POI_INFO_FROM_AIMAP_POI, HandleSendPOIInfoFromAIMapPOI);
            addModuleEvent(ModuleEventType.UNLOAD_UNITY_ASSETS, HandleUnloadUnityAssets);
            addModuleEvent(ModuleEventType.ENABLE_CONSISTENCY_CHECK, HandleEnableConsistencyCheck);

            //addModuleEvent(ModuleEventType.GET_POSITION_IN_UNITY, HandleGetPositionInUnity);


            sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.AI_MAP_SCENE);//主要业务逻辑入口，进入AIMapScene模块

            arCameraManager = GameObject.FindObjectOfType<ARCameraManager>();
            arCameraManager.frameReceived += ArCamera_frameReceived;
            cameraImage = GameObject.FindObjectOfType<CameraImageExample>();
        }

        private void HandleEnableConsistencyCheck(EventObject ev)
        {
            isEnableCheck = (bool)ev.param;
            Debug.Log("HandleEnableConsistencyCheck..." + isEnableCheck);
        }

        private void HandleUnloadUnityAssets(EventObject obj)
        {
            Debug.Log("localize...HandleUnloadUnityAssets...");
            modelInfoList.Clear();
            prefabInfoList.Clear();
        }

        //private void HandleGetPositionInUnity(EventObject ev)
        //{
        //    Debug.Log("HandleGetPostionInUnity...");
        //    Model6DoFInfo modelInfo = ev.param as Model6DoFInfo;
        //    if (modelInfo == null) return;
        //    Vector3 originPos = modelInfo.pos;
        //    Quaternion originRot = modelInfo.rot;
        //    Vector3 finalPos;
        //    Quaternion finalRot;
        //    Model6DoFInfo newModel = new Model6DoFInfo();
        //    ReturnPositionInUnity(camera6DoFInfo, originPos, originRot, out finalPos, out finalRot);
        //    newModel.pos = finalPos;
        //    newModel.rot = finalRot;
        //    Debug.Log("newPos:" + newModel.pos);
        //    sendModuleEvent(ModuleEventType.GET_POSITION_IN_UNITY_CALL_BACK, newModel);
        //}

        private void HandleSendPOIInfoFromAIMapPOI(EventObject ev)
        {
            Debug.Log("HandleSendPOIInfoFromAIMapPOI...");
            List<POIInfoInUnity> buildingInfos = ev.param as List<POIInfoInUnity>;
            if (buildingInfos == null) return;
            foreach (var buildingInfo in buildingInfos)
            {
                if (buildingInfo == null) continue;
                Vector3 originPos = buildingInfo.buildingLocation;
                Quaternion originRot = Quaternion.identity;
                Vector3 finalPos;
                Quaternion finalRot;
                ReturnPositionInUnity(camera6DoFInfo, originPos, originRot, out finalPos, out finalRot);
                buildingInfo.buildingLocation = finalPos;
            }
            sendModuleEvent(ModuleEventType.GET_POI_POS_IN_UNITY, buildingInfos);
        }

        /// <summary>
        /// 只在初始化模型加载时调用一次
        /// </summary>
        /// <param name="ev"></param>
        private void HandleGetModelPrefabsPara(EventObject ev)
        {
            Debug.Log("HandleGetModelPrefabsPara...");
            prefabInfoList.Clear();
            List<ChangeModelInfo> tempPrefabsInfoList = ev.param as List<ChangeModelInfo>;
            Debug.Log(tempPrefabsInfoList.Count);
            foreach (ChangeModelInfo prefabInfo in tempPrefabsInfoList)
            {
                prefabInfoList.Add(prefabInfo);
            }
#if UNITY_EDITOR
            sendModuleEvent(ModuleEventType.FOUND_MAPS, positionInfo);
#endif
        }

        #region 定位

        /// <summary>
        /// 获取ARcore返回相机参数
        /// </summary>
        /// <param name="ev"></param>
        private void HandleSendCameraInfo(EventObject ev)
        {
            Debug.Log("HandleSendCameraInfo...");
            Texture2D texture = ev.param as Texture2D;
            if (texture == null) return;
            //string path = "";
            //path = UnityUtilties.SaveCaptureScreen(texture, Application.persistentDataPath, "ScreenShot.png");

            //#if UNITY_IOS
            //            scale = 640f / texture.width;
            //            float height = scale * texture.height;

            //            texture = UnityUtilties.ReSetTextureSize(texture, 640, (int)height);
            //#endif
            string base64 = UnityUtilties.Texture2DToBase64(texture);
            if (base64 == null) return;
            cameraInfo.path = base64;
            JsonData json = JsonMapper.ToJson(cameraInfo);
            //Debug.Log("cameraInfo:" + json.ToString());
            SDKManager.SendCurrentViewToPhone(json.ToString());
        }

        private void ArCamera_frameReceived(ARCameraFrameEventArgs ev)
        {
            if (!isARInit)
            {
                //In my case 0=640*480, 1= 1280*720, 2=1920*1080
                arCameraManager.subsystem.currentConfiguration = arCameraManager.GetConfigurations(Allocator.Temp)[1];
                isARInit = true;
            }

            //arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage cpuImage);
            //Debug.Log("cpuImage:" + cpuImage.width + "/" + cpuImage.height);
            //NativeArray<XRCameraConfiguration> cameraConfig = arCameraManager.GetConfigurations(Allocator.Temp);
            bool isSucc = arCameraManager.TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics);
            //Debug.Log("focalLength:" + cameraIntrinsics.focalLength + "principalPoint:" + cameraIntrinsics.principalPoint);
            //arCameraManager.frameReceived -= ArCamera_frameReceived;

            cameraInfo.focalLength = cameraIntrinsics.focalLength.x + "," + cameraIntrinsics.focalLength.y;
            //cameraInfo.resolution = cpuImage.width + "," + cpuImage.height;
            cameraInfo.principalPoint = cameraIntrinsics.principalPoint.x + "," + cameraIntrinsics.principalPoint.y;
        }

        /// <summary>
        /// 开始定位
        /// </summary>
        /// <param name="obj"></param
        private void HandleStartPosition(EventObject ev)
        {
            //sendModuleEvent(ModuleEventType.CLEAR_ALL_MODEL);
            startPosition.pos = Camera.main.transform.position;
            startPosition.rot = Camera.main.transform.rotation;
            cameraImage.GetImageAsync();

            Debug.Log("StartPosition:" + startPosition.pos.ToString());
        }

        /// <summary>
        /// 定位成功
        /// </summary>
        /// <param name="ev"></param>
        private void HandleLoacalizeFoundMaps(EventObject ev)
        {
            if (ev.param == null) return;
            string json = ev.param.ToString();
            Debug.Log("FoundMap:" + json);
            CreateMapByData(json);

            //sendModuleEvent(ModuleEventType.SET_LOADING_ANI, false);
        }
        Model6DoFInfo camera6DoFInfo = new Model6DoFInfo();

        /// <summary>
        /// 根据返回json相对数据创建地图相对关系
        /// </summary>
        /// <param name="json"></param>
        private void CreateMapByData(string json)
        {
            if (json == null) return;
            Debug.Log("CreateMapData..." + json);
            MapPositionInfo positionInfo = JsonMapper.ToObject<MapPositionInfo>(json);
            if (positionInfo == null) return;

            string[] translation = positionInfo.translation.Split(' ');
            string[] rotation = positionInfo.rotation.Split(' ');

            camera6DoFInfo.pos = new Vector3(float.Parse(translation[0]), float.Parse(translation[1]), float.Parse(translation[2]));
            camera6DoFInfo.rot = new Quaternion(float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2]), float.Parse(rotation[3]));

            origin6DoFInfo.pos = new Vector3((float)positionInfo.ori_tvec[0], (float)positionInfo.ori_tvec[1], (float)positionInfo.ori_tvec[2]);
            origin6DoFInfo.rot = new Quaternion((float)positionInfo.ori_qvec[0], (float)positionInfo.ori_qvec[1], (float)positionInfo.ori_qvec[2], (float)positionInfo.ori_qvec[2]);

            if (isEnableCheck)
                if (CheckConsistency(camera6DoFInfo, startPosition) == false)
                    return;
            GetModelPosition(camera6DoFInfo);
        }

        #region 算法
        private Vector3 ConvertRightHandedToLeftHandedVector(Vector3 rightHandedVector)
        {
            return new Vector3(rightHandedVector.x, rightHandedVector.z, rightHandedVector.y);
        }

        private Quaternion ConvertRightHandedToLeftHandedQuaternion(Quaternion rightHandedQuaternion)
        {
            return new Quaternion(-rightHandedQuaternion.x,
                                   -rightHandedQuaternion.z,
                                   -rightHandedQuaternion.y,
                                     rightHandedQuaternion.w);
        }

        private Vector3 ConvertRightHandedInverseY(Vector3 rightHandedVector)
        {
            return new Vector3(rightHandedVector.x, -rightHandedVector.y, rightHandedVector.z);
        }

        private Quaternion ConvertRightHandedInverseY(Quaternion rightHandedQuaternion)
        {
            return new Quaternion(rightHandedQuaternion.x,
                                   -rightHandedQuaternion.y,
                                   rightHandedQuaternion.z,
                                     -rightHandedQuaternion.w);
        }

        private msVector3 MsQuaternionRotatePoint(msQuaternion quaternion, msVector3 point)
        {
            msQuaternion quat_norm = msQuaternion.Normalize(quaternion);
            msQuaternion q1 = quat_norm;
            q1 = msQuaternion.Conjugate(q1);

            msQuaternion qNode = new msQuaternion(point.X, point.Y, point.Z, 0);
            qNode = quat_norm * qNode * q1;

            return new msVector3(qNode.X, qNode.Y, qNode.Z);

        }

        private Vector3 QuaternionRotatePoint(Quaternion quaternion, Vector3 point)
        {
            Quaternion quat_norm = Quaternion.Normalize(quaternion);
            Quaternion q1 = quat_norm;
            q1 = Quaternion.Inverse(q1);
            Quaternion qNode = new Quaternion(point.x, point.y, point.z, 0);
            qNode = quat_norm * qNode * q1;
            return new Vector3(qNode.x, qNode.y, qNode.z);
        }



        /**@brief Calculate transformation from odometry to Map
        /* @param Model6DoFInfo  transformation from camera to map
        /* @param Model6DoFInfo  transformation from camera to odom
        /* @return  Model6DoFInfo transformation from odom to map
        */
        Model6DoFInfo TransformOdom2Map(Model6DoFInfo camera6DoFInfo, Model6DoFInfo startPosition)
        {
            //  calulate transformation from odometry(ARKit) to Map(vps)
            // meanwhile, right-handed to left-handed
            Quaternion cam2map_q = new Quaternion(camera6DoFInfo.rot.x, -camera6DoFInfo.rot.y, camera6DoFInfo.rot.z, -camera6DoFInfo.rot.w);
            Vector3 cam2map_t = new Vector3(camera6DoFInfo.pos.x, -camera6DoFInfo.pos.y, camera6DoFInfo.pos.z);

            Quaternion cam2odom_q = new Quaternion(startPosition.rot.x, startPosition.rot.y, startPosition.rot.z, startPosition.rot.w);
            Vector3 cam2odom_t = new Vector3(startPosition.pos.x, startPosition.pos.y, startPosition.pos.z);
            // inverse
            Quaternion odom2cam_q = Quaternion.Inverse(cam2odom_q);
            Vector3 odom2cam_t = (QuaternionRotatePoint(odom2cam_q, -cam2odom_t));

            //
            Model6DoFInfo odom2map = new Model6DoFInfo();
            odom2map.pos = (QuaternionRotatePoint(cam2map_q, odom2cam_t)) + cam2map_t;
            odom2map.rot = cam2map_q * odom2cam_q;
            return odom2map;
        }


        static Model6DoFInfo odom2map_init;
        static bool first_entry = true;
        /**@brief Check consistency when get a result of vps
        /* @param Model6DoFInfo  transformation from camera to map
        /* @param Model6DoFInfo  transformation from camera to odom
        /* @param float  distance threshold
        /* @param float  angle threshold
        /* @return  bool  consistent or not
        */
        bool CheckConsistency(Model6DoFInfo camera2Map, Model6DoFInfo camera2Odom, float distance_threshold = 0.5f, float angle_threshold = 20.0f)
        {
            Debug.Log("camera2Map:" + camera2Map.ToString());
            Debug.Log("camera2Odom:" + camera2Odom.ToString());
            if (first_entry)
            {
                odom2map_init = TransformOdom2Map(camera2Map, camera2Odom);
                first_entry = false;
                return true;
            }
            else
            {
                Model6DoFInfo odom2map = TransformOdom2Map(camera2Map, camera2Odom);
                Vector3 diff_pos = odom2map.pos - odom2map_init.pos;
                float diff_distance = diff_pos.magnitude;
                Quaternion diff_rotation = odom2map.rot * Quaternion.Inverse(odom2map_init.rot);

                Debug.Log("odom2map:" + odom2map.ToString());
                Debug.Log("diff_distance:" + diff_distance.ToString());

                // TODO(frye angre  )
                if (diff_distance > distance_threshold)
                {
                    return false;
                }

                return true;
            }
        }


        void ReturnPositionInUnity(Model6DoFInfo camera6DoFInfo, Vector3 modelPosition, Quaternion modelRotation, out Vector3 pos, out Quaternion rot)
        {
            //if (CheckConsistency(camera6DoFInfo, startPosition) == false)
            //{
            //    return;
            //}

            ///+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++///
            /// 将模型点转到视觉定位的相机坐标下
            // TODO(frye) UnityQuaternion replace msQuaternion 
            msQuaternion cameraQuatInMapRight = new msQuaternion(camera6DoFInfo.rot.x, camera6DoFInfo.rot.y, camera6DoFInfo.rot.z, camera6DoFInfo.rot.w);
            msVector3 cameraTransInMapRight = new msVector3(camera6DoFInfo.pos.x, camera6DoFInfo.pos.y, camera6DoFInfo.pos.z);
            msQuaternion cameraQuatInMapRightInverse = msQuaternion.Inverse(cameraQuatInMapRight);
            msVector3 msModelPosition = new msVector3(modelPosition.x, modelPosition.y, modelPosition.z);
            msVector3 cameraTransInMapRightInverse = (MsQuaternionRotatePoint(cameraQuatInMapRightInverse, (msModelPosition - cameraTransInMapRight)));
            Vector3 modelInCamPosition = new Vector3(cameraTransInMapRightInverse.X, cameraTransInMapRightInverse.Y, cameraTransInMapRightInverse.Z);

            Quaternion modelInCamRotation = Quaternion.Inverse(camera6DoFInfo.rot) * modelRotation;
            ///=============================================================///

            ///+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++///
            /// 将视觉定位相机坐标（右手系）下的模型点转到Unity相机坐标系(左手系)下
            modelInCamPosition.y = -modelInCamPosition.y;
            modelInCamRotation.y = -modelInCamRotation.y;
            modelInCamRotation.w = -modelInCamRotation.w;
            //localizeView.SetStatus1Text("modelInCamPositionLeft:" + modelInCamPosition.ToString());
            ///=============================================================///

            ///+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++///
            /// ARCore跟踪得到的unity手机本体位姿
            Vector3 mobilePositionInUnity = startPosition.pos;
            Quaternion mobileQuatInUnity = startPosition.rot;
            //localizeView.SetStatusText("unityCamPose:" + mobilePositionInUnity.ToString() + "startPostionRot:" + mobileQuatInUnity.ToString());

            /// unity手机本体坐标系到unity相机坐标系需要绕Z轴转90°
            Quaternion z90 = Quaternion.Euler(0, 0, -90);
            if (GlobalObject.SCREEN_ORIENTATION.Equals("1"))
            {
                switch (Input.deviceOrientation)
                {
                    case DeviceOrientation.LandscapeLeft:
                        z90 = Quaternion.Euler(0, 0, 0);
                        break;
                    case DeviceOrientation.LandscapeRight:
                        z90 = Quaternion.Euler(0, 0, 0);
                        break;
                }
            }
            Quaternion cameraRotInUnity90 = mobileQuatInUnity * z90;
            ///=============================================================///

            /// 最终得到模型点在unity本体坐标系下的坐标
            Vector3 modelPosUnityCam = QuaternionRotatePoint(cameraRotInUnity90, modelInCamPosition) + mobilePositionInUnity;
            Quaternion modeRotUnityCam = cameraRotInUnity90 * modelInCamRotation;
            Debug.Log("modelPosInUnity:" + modelPosUnityCam.ToString());


            pos = modelPosUnityCam;
            rot = modeRotUnityCam;
            //Model6DoFInfo model6DofInfoUnityCam = new Model6DoFInfo();
            //model6DofInfoUnityCam.pos = modelPosUnityCam;
            //model6DofInfoUnityCam.rot = modeRotUnityCam;
            //return model6DofInfoUnityCam;
        }

        #endregion

        /// <summary>
        /// 创建模型点位置
        /// </summary>
        void GetModelPosition(Model6DoFInfo camera6DoFInfo)
        {
            modelInfoList.Clear();
            Debug.Log("GetModelPostion...");

            foreach (ChangeModelInfo item in prefabInfoList)
            {
                LoadPrefab(item);
            }
            sendModuleEvent(ModuleEventType.LOADING_MODELS, modelInfoList);
        }

        private void LoadPrefab(ChangeModelInfo prefabInfo)
        {
            Vector3 pos;
            Quaternion rot;
            ReturnPositionInUnity(camera6DoFInfo, prefabInfo.pos, Quaternion.Euler(prefabInfo.eulerAngle), out pos, out rot);
            ChangeModelInfo modelInfo = new ChangeModelInfo();
            modelInfo.modelId = prefabInfo.modelId;
            modelInfo.eulerAngle = rot.eulerAngles;
            modelInfo.pos = pos;
            modelInfo.scale = prefabInfo.scale;

            modelInfoList.Add(modelInfo);
        }
        #endregion

        void WriteModelInfoToLocal(List<ChangeModelInfo> savingList)
        {
            List<ChangeModelInfoSavingFormat> savingFormatList = new List<ChangeModelInfoSavingFormat>();
            foreach (ChangeModelInfo item in savingList)
            {
                ChangeModelInfoSavingFormat savingFormat = new ChangeModelInfoSavingFormat();
                savingFormat.pos = UnityUtilties.Vector3ToString(item.pos);
                savingFormat.eulerAngle = UnityUtilties.Vector3ToString(item.eulerAngle);
                savingFormat.scale = UnityUtilties.Vector3ToString(item.scale);
                savingFormat.modelId = item.modelId;
                savingFormat.abName = item.abName;
                savingFormat.remarks = item.remarks;
                savingFormatList.Add(savingFormat);
            }

            string jsonStr = JsonMapper.ToJson(savingFormatList);
            bool isSucc = UnityUtilties.AddTxtTextByFileStream(jsonStr);
        }

        #region test
        private void HandleSetLocalizeBtnStatus(EventObject ev)
        {
            if (ev.param == null) return;
            localizeView.LocalizeSwitchBtnText.text = ev.param.ToString();
        }

        private void HandleSetLocalizeBackBth(EventObject ev)
        {
            if (ev.param == null) return;
            bool isShow = (bool)ev.param;
        }

        private void HandleSetLocalizeStatusText(EventObject ev)
        {
            if (ev.param == null) return;
            string text = ev.param.ToString();
        }

        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(localizeView.TestBtn))
            {
                LocalAreaAssetsFile assetsFile = new LocalAreaAssetsFile();
                assetsFile.id = "9L";
                assetsFile.contentPath = Application.streamingAssetsPath;
                string json = JsonMapper.ToJson(assetsFile);
                sendModuleEvent(ModuleEventType.GET_AREA_ASSETS_FILES, json);
            }
            else if (ev.param.Equals(localizeView.LocalizeSwitchBtn))
            {
                //sendModuleEvent(ModuleEventType.SET_LOADING_ANI, false);
                //todo
                Model6DoFInfo cameraInfo = new Model6DoFInfo();
                cameraInfo.pos = Camera.main.transform.position;
                cameraInfo.rot = Camera.main.transform.rotation;
                sendModuleEvent(ModuleEventType.START_POSITION, cameraInfo);
                cameraImage.GetImageAsync();
            }
            else if (ev.param.Equals(localizeView.EnableBtn))
            {
                sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.LOCALIZATION_TOOL);
            }
        }
        #endregion
    }
}
