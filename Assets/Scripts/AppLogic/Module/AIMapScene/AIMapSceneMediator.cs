using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using TMPro;
using System;
using LitJson;
using UnityEngine.XR.ARFoundation;

namespace AppLogic
{
    public class AIMapSceneMediator : BaseMediator
    {
        public GameObject worldRoot;
        List<ChangeModelInfo> prefabsInfoList = new List<ChangeModelInfo>();
        List<ChangeModelInfo> areaAssetsInfoList = new List<ChangeModelInfo>();

        Dictionary<string, ChangeModelInfo> previousInfos = new Dictionary<string, ChangeModelInfo>();
        Dictionary<string, ChangeModelInfo> currentInfos = new Dictionary<string, ChangeModelInfo>();

        List<GameObject> modelList = new List<GameObject>();
        List<GameObject> waypointsList = new List<GameObject>();
        int prefabsNum;
        int loadedPrefabsNum = 0;
        bool loadCompleted = false;
        bool isLoadedWayPoints = false;
        GameObject wayPointsParent = null;
        string oldFilePath = null;
        Tick tick;

        public AIMapSceneMediator()
        {
            m_mediatorName = MEDIATOR.AI_MAP_SCENE;
        }
        protected override void onInit()
        {
            Debug.Log("AIMapMediatorInit....");
            addModuleEvent(ModuleEventType.LOADING_PREFABS, HandleLoadingPrefabs);//读取当前区域内模型的Prefab数据、生成prefab
            addModuleEvent(ModuleEventType.LOADING_MODELS, HandleLoadingModels);//读取当前区域内的模型进Unity
            addModuleEvent(ModuleEventType.LOADING_AREA_ASSETS_DONE, HandleLoadingAreaAssetsDone);//读取当前区域内资源完成
            addModuleEvent(ModuleEventType.GET_AREA_ASSETS_FILES, HandleGetAreaAssetsFiles);//获取当前区域内的模型资源数据
            addModuleEvent(ModuleEventType.UNLOAD_UNITY_ASSETS, HandleUnloadUnityAssets);//删除场景中已加载模型资源
            addModuleEvent(ModuleEventType.VISIBLE_DISTANCE_CHANGED, HandleModelVisibleChangeByDistance);//距离改变
            addModuleEvent(ModuleEventType.SWITCH_PARALLEL_UNIVERSE, HandleSwitchParallelUniverse);//平行宇宙切换
            worldRoot = new GameObject("WorldRoot");

            tick = TickManager.GetInstance().createTick(0, null, ModelLerp);

            // Add an ARAnchor component if it doesn't have one already.
            if (worldRoot.GetComponent<ARAnchor>() == null)
            {
                worldRoot.AddComponent<ARAnchor>();
            }
            //sendModuleEvent(ModuleEventType.GET_AREA_ASSETS_FILES);
            InitCommand();
            sendModuleEvent(ModuleEventType.GET_AREA_ASSETS_FILES);
            //sendModuleEvent(ModuleEventType.SCENE_AIMAP_POI);
        }

        private void HandleSwitchParallelUniverse(EventObject ev)
        {
            string id = ev.param.ToString();
            if (id == null) return;
            ParallelverseModelInfo modelInfo = new ParallelverseModelInfo();
            modelInfo.modelList = modelList;
            modelInfo.id = id;
            sendModuleEvent(ModuleEventType.SWITCH_PARALLEL_UNIVERSE_CALL_BACK, modelInfo);
        }

        /// <summary>
        /// 根据模型距离判断是否显示
        /// </summary>
        /// <param name="ev"></param>
        private void HandleModelVisibleChangeByDistance(EventObject ev)
        {
            string distanceStr = ev.param.ToString();
            if (distanceStr == null) return;
            float distance = float.Parse(distanceStr);
            ModelVisibleInfo modelVisible = new ModelVisibleInfo();
            modelVisible.modelList = modelList;
            modelVisible.distance = distance;
            sendModuleEvent(ModuleEventType.VISIBLE_MODEL_CHANGE_BY_DISTANCE, modelVisible);
        }

        /// <summary>
        /// 卸载、删除场景中的资源
        /// </summary>
        /// <param name="obj"></param>
        private void HandleUnloadUnityAssets(EventObject obj)
        {
            Debug.Log("HandleUnloadUnityAssets...");
            waypointsList.Clear();
            isLoadedWayPoints = false;
            prefabsInfoList.Clear();
            areaAssetsInfoList.Clear();
            for (int i = 0; i < modelList.Count; i++)
            {
                GameObject.Destroy(modelList[i]);
            }
            loadedPrefabsNum = 0;
            loadCompleted = false;
            wayPointsParent = null;
        }

        private void InitCommand()
        {
            //registerCommand(ModuleEventType.SCENE_AIMAP_POI, typeof(AIMapPOICommand));
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_VISIBLE_DISTANCE);
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SCENE_PARALLELVERSE);
            //sendModuleEvent(ModuleEventType.SCENE_AIMAP_POI);
            //SDKManager.PhoneMethodForUnityInitComplete();
        }
        #region LoadingPrefabs

        private void HandleGetAreaAssetsFiles(EventObject ev)
        {
            List<ChangeModelInfo> modelInfoList = new List<ChangeModelInfo>();
            IDictionary modelInfos = ConfigManager.GetConfigs<ModelConfigInfo>();
            Debug.Log("AreaAssetCount:" + modelInfos.Count);
            foreach (ModelConfigInfo item in modelInfos.Values)
            {
                ChangeModelInfo modelInfo = new ChangeModelInfo();
                modelInfo.modelId = item.id;
                modelInfo.abName = item.abName;
                modelInfo.remarks = item.remarks;
                modelInfoList.Add(modelInfo);
                areaAssetsInfoList.Add(modelInfo);
            }
            sendModuleEvent(ModuleEventType.LOADING_PREFABS, modelInfoList);
        }

        /// <summary>
        /// 读取当前区域资源完成
        /// </summary>
        /// <param name="ev"></param>
        private void HandleLoadingAreaAssetsDone(EventObject ev)
        {
            SDKManager.PhoneMethodForLoadingAreaAssetsDone();
        }

        /// <summary>
        /// 读取prefab中的位置信息
        /// </summary>
        /// <param name="ev"></param>
        private void HandleLoadingPrefabs(EventObject ev)
        {
            Debug.Log("HandleLoadingPrefabs...");
            prefabsInfoList.Clear();
            modelList.Clear();
            List<ChangeModelInfo> modelInfoList = ev.param as List<ChangeModelInfo>;
            Debug.Log("HandleLoadingPrefabs:" + modelInfoList.Count);
            loadedPrefabsNum = 0;
            prefabsNum = modelInfoList.Count;
            loadCompleted = false;
            GlobalObject.OLD_ASSET_PATH = null;
            foreach (ChangeModelInfo modelInfo in modelInfoList)
            {
                LoadPrefab(modelInfo);
            }
        }

        /// <summary>
        /// 第一次初始化ab文件中的模型，并读取位置
        /// </summary>
        /// <param name="modelInfo"></param>
        private void LoadPrefab(ChangeModelInfo modelInfo)
        {
            string abPath = PATH_TYPE.MODEL + modelInfo.abName.Split('/')[0];
            string assetName = modelInfo.abName.Split('/')[1];
            prefabsInfoList.Add(modelInfo);
            ResourceManager.GetInstance().loadAssetAsync<GameObject>(abPath, assetName, CallBack);
        }

        private void CallBack(UnityEngine.Object obj)
        {
            if (loadCompleted) return;
            GameObject prefab = (GameObject)obj;
            GameObject go = GameObject.Instantiate<GameObject>(prefab);
            go.name = go.name.Replace("(Clone)", "");
            go.SetActive(false);
            modelList.Add(go);
            for (int i = 0; i < prefabsInfoList.Count; i++)
            {
                if (!prefabsInfoList[i].modelId.Equals(prefab.name)) continue;
                prefabsInfoList[i].pos = prefab.transform.position;
                prefabsInfoList[i].eulerAngle = prefab.transform.eulerAngles;
                prefabsInfoList[i].scale = prefab.transform.localScale;
            }

            loadedPrefabsNum++;
            Debug.Log("loadedPrefabNum:" + loadedPrefabsNum + "," + go.name);
            Debug.Log("prefabsNum:" + prefabsNum);
            if (prefabsNum.Equals(loadedPrefabsNum))
            {
                Debug.Log("LoadPrefabCompleted...");
                loadCompleted = true;
                //sendModuleEvent(ModuleEventType.SEND_MODEL_PREFAB_PARA, prefabsInfoList);
                sendModuleEvent(ModuleEventType.LOADING_AREA_ASSETS_DONE);
                sendModuleEvent(ModuleEventType.SEND_MODEL_PREFAB_PARA, prefabsInfoList);

                //无定位模式下操作 如果配表中remarks==id的时候执行
                if (prefabsInfoList[0] == null) return;
                if (prefabsInfoList[0].remarks.Equals(prefabsInfoList[0].modelId))
                {
                    modelList[0].transform.position = prefabsInfoList[0].pos;
                    modelList[0].transform.eulerAngles = prefabsInfoList[0].eulerAngle;
                    modelList[0].transform.localScale = prefabsInfoList[0].scale;
                    modelList[0].SetActive(true);
                }
            }
        }
        #endregion

        #region loadingModels
        /// <summary>
        /// 经过定位模块返回的数据，重新设置model位置
        /// </summary>
        /// <param name="ev"></param>
        private void HandleLoadingModels(EventObject ev)
        {
            Debug.Log("HandleLoadingModels...");
            if (currentInfos.Count != 0)
            {
                Debug.Log("currentInfos.Count:" + currentInfos.Count);
                previousInfos.Clear();
                foreach (var info in currentInfos.Values)
                {
                    previousInfos.Add(info.modelId, info);
                    Debug.Log("previousinfo:" + info.modelId);
                }
                currentInfos.Clear();
                tick.stop();
            }
            List<ChangeModelInfo> modelInfoList = ev.param as List<ChangeModelInfo>;
            foreach (ChangeModelInfo modelInfo in modelInfoList)
            {
                ChangeModelInfo info = LoadModel(modelInfo);
                if (info == null) continue;
                currentInfos.Add(info.modelId, info);
                Debug.Log(currentInfos.Count + "@louiscount");
            }

            sendModuleEvent(ModuleEventType.VISIBLE_MODEL_CHANGE_BY_DISTANCE, 50);
            tick.start();
            if (isLoadedWayPoints || wayPointsParent == null) return;
            for (int i = 0; i < wayPointsParent.transform.childCount; i++)
            {
                waypointsList.Add(wayPointsParent.transform.GetChild(i).gameObject);
            }
            sendModuleEvent(ModuleEventType.GET_WAYPOINTS_LIST, waypointsList);
            isLoadedWayPoints = true;
            Debug.Log("AIwaypointlistcount:" + waypointsList.Count);
        }

        private ChangeModelInfo LoadModel(ChangeModelInfo modelInfo)
        {
            foreach (var model in modelList)
            {
                if (!model.name.Equals(modelInfo.modelId)) continue;
                model.name = modelInfo.modelId;
                //model.transform.position = modelInfo.pos;
                model.transform.parent = worldRoot.transform;
                //model.transform.eulerAngles = modelInfo.eulerAngle;
                model.transform.localScale = modelInfo.scale;
                model.SetActive(true);
                modelInfo.model = model;
                Debug.Log("ModelPos," + modelInfo.modelId + ":" + modelInfo.pos.ToString());
                if (modelInfo.modelId.Contains("WayPoints")) wayPointsParent = model;
                return modelInfo;
            }
            return null;
        }
        #endregion

        void ModelLerp()
        {
            foreach (var currentInfo in currentInfos.Values)
            {
                ChangeModelInfo info = new ChangeModelInfo();
                bool isGet = previousInfos.TryGetValue(currentInfo.modelId, out info);

                //Debug.Log("isGet:" + isGet);
                //Debug.Log("ModelLerp..."+currentInfo.modelId);
                //if (currentInfo == null) Debug.Log("is null1");
                //if (currentInfo.pos == null) Debug.Log("is null");
                Vector3 previousPos = currentInfo.pos;
                //if (currentInfo.eulerAngle == null) Debug.Log("rot is null");
                Quaternion previousRot = Quaternion.Euler(currentInfo.eulerAngle);

                //if (currentInfo.model == null) Debug.Log("model is null");
                GameObject go = currentInfo.model;
                //if (go == null) Debug.Log("go is null");
                float T = 1;
                if (isGet)
                {
                    //Debug.Log("isGet");
                    previousPos = info.pos;
                    previousRot = Quaternion.Euler(info.eulerAngle);
                    go = info.model;

                    //T = Time.deltaTime * Vector3.Distance(go.transform.position, currentInfo.pos);
                    //T += 1f / 2f * Time.deltaTime;
                    T = Time.deltaTime;
                }

                //go.transform.position = Vector3.Lerp(previousPos, currentInfo.pos, T);
                //go.transform.eulerAngles = Vector3.Lerp(previousRot.eulerAngles, currentInfo.eulerAngle, T);

                go.transform.position = Vector3.Lerp(go.transform.position, currentInfo.pos, T);
                go.transform.rotation = Quaternion.Lerp(go.transform.rotation, Quaternion.Euler(currentInfo.eulerAngle), T);
                //Debug.Log("pos:" + go.transform.position.ToString() + "/" + go.transform.eulerAngles.ToString());
                //Debug.Log(currentInfo.ToString());
            }
        }

        /// <summary>
        /// 隐藏所有模型
        /// </summary>
        /// <param name="ev"></param>
        private void HandleHideAllModels(EventObject ev)
        {
            foreach (var model in modelList)
            {
                model.SetActive(false);
            }
        }

        /// <summary>
        /// 清除所有模型
        /// </summary>
        /// <param name="ev"></param>
        private void HandleClearAllModel(EventObject ev)
        {
            Debug.Log(worldRoot.transform.childCount);
            for (int i = 0; i < worldRoot.transform.childCount; i++)
            {
                Transform childTran = worldRoot.transform.GetChild(i);
                if (childTran == null) continue;
                GameObject.Destroy(childTran.gameObject);
            }
        }
        protected override void onButtonClick(EventObject obj)
        {
            base.onButtonClick(obj);
        }
        public override void onExit()
        {
        }
    }
}