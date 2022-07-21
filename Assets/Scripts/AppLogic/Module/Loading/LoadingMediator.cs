using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AppLogic
{
    public class LoadingMediator : BaseMediator
    {
        LoadingView loadingView;
        AsyncOperation sceneLoadAsync;
        string currentSceneName = string.Empty;
        Tick sceneUpdateTick;

        //进度模仿数值
        float sceneSimulateProgress = 0;

        LoadItem assetLoader;
        Queue<LoadAssetInfo> assetLoadQueue = new Queue<LoadAssetInfo>();
        LoadAssetInfo currentAssetLoadInfo = null;
        int currentLoadAssetTotalCount = 0;
        int currentLoadAssetIndex = 0;


        public LoadingMediator()
        {
            m_mediatorName = MEDIATOR.LOADING;
        }



        public override void onRegister()
        {
            Debug.Log("loading mediator init...");
#if UNITY_EDITOR
            #region  Resources Load Mode Example
            Transform canvas = GameObject.Find("Canvas").transform;
            GameObject viewPrefab = Resources.Load<GameObject>("LoadingView");
            GameObject viewObj = GameObject.Instantiate<GameObject>(viewPrefab);
            viewObj.transform.SetParent(canvas, false);
            loadingView = new LoadingView();
            loadingView.init(viewObj);
            viewComponent = loadingView;
            viewComponent.addEvent(BaseView.BUTTON_CLICK, onButtonClick);
            #endregion
#endif
            //loadingView = viewComponent as LoadingView;

            sceneUpdateTick = TickManager.GetInstance().createTick(1, null, UpdateLoadingScene);

            addModuleEvent(ModuleEventType.LOADING_SCENE, HandleLoadScene);
            //addModuleEvent(ModuleEventType.LOADING_ASSET, handleLoadingAsset);

            addModuleEvent(ModuleEventType.SET_LOADING_ANI, HandleSetLoadingAni);

            //addModuleEvent(ModuleEventType.LOADING_COMPLETE, HandleLoadingComplete);
            //sendModuleEvent(ModuleEventType.LOADING_SCENE, SCENE_NAME.ARMODEL);
            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.DOWNLOAD);

            //sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.MODE_SWITCH);
            //sendModuleEvent(ModuleEventType.SET_LOADING_ANI, false);
        }

        private void handleLoadingAsset(EventObject ev)
        {
            //@:强制不转义
            //string uri = @"http://peo77op1t.bkt.clouddn.com/asset/config/1.txt?v=112";
            LoadAssetInfo info = ev.param as LoadAssetInfo;
            if (null == info) return;
            if (null == currentAssetLoadInfo)
            {
                currentAssetLoadInfo = info;
                if (0 == currentAssetLoadInfo.assetNameList.Count)
                {
                    currentLoadAssetTotalCount = 0;
                    currentLoadAssetIndex = 0;
                    handleLoadAssetComplete();
                }
                else
                {
                    //loadingView.loadAssetPanel.setActive(true);
                    currentLoadAssetTotalCount = currentAssetLoadInfo.assetNameList.Count;
                    currentLoadAssetIndex = 1;
                    loadAsset(currentLoadAssetIndex);
                }
            }
            else assetLoadQueue.Enqueue(info);
        }

        void loadAsset(int index)
        {
            if (null == currentAssetLoadInfo) return;
            if (index > currentAssetLoadInfo.assetNameList.Count) return;
            //loadingView.loadAssetPanel.setDegree(0, index, currentLoadAssetTotalCount);
            //loadingView.loadAssetPanel.progressBar.setProgress(0);
            //loadingView.loadAssetPanel.progressBar.setInfo(index + "/" + currentLoadAssetTotalCount);
            string assetName = currentAssetLoadInfo.assetNameList[index - 1];

#if UNITY_EDITOR
            if (!AppUtil.UPDATE)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetName).Length > 0)
                {
                    handleLoadAssetComplete();
                    return;
                }
                //return;
            }
#endif
            //判断是否已更新
            //DynamicAsset dynAsset = DBManager.GetInstance().select<DynamicAsset>("id", assetName);
            //if (null == dynAsset || dynAsset.isUpdate)
            //{
            //    handleLoadAssetComplete();
            //    return;
            //}
            //string assetUrl = Path.Combine(AppUtil.DYNAMIC_ASSET_URL, assetName) + "?v=" + dynAsset.crc;
            //int timeout = dynAsset.length / 100;//根据配置表资源大小设置超时时间
            //int timeout = 100;
            //assetLoader = LoadManager.GetInstance().createLoader(assetUrl, timeout);
            //assetLoader.addEvent(LoadItem.LOAD_COMPLETE, handleLoadAsset);
            //assetLoader.addEvent(LoadItem.LOAD_UPDATE, handleLoadAssetUpdate);
            //assetLoader.onStart();
        }
        void handleLoadAssetUpdate(EventObject ev)
        {
            float progress = (float)ev.param;
            //loadingView.loadAssetPanel.setDegree(progress, currentLoadAssetIndex, currentLoadAssetTotalCount);
            //loadingView.loadAssetPanel.progressBar.setProgress(progress);
        }
        void handleLoadAsset(EventObject ev)
        {
            assetLoader.removeEvent(LoadItem.LOAD_COMPLETE, handleLoadAsset);
            assetLoader.removeEvent(LoadItem.LOAD_UPDATE, handleLoadAssetUpdate);
            bool isSucc = (bool)ev.param;
            if (isSucc)
            {
                string assetName = currentAssetLoadInfo.assetNameList[currentLoadAssetIndex - 1];
                string path = Path.Combine(GlobalObject.ASSET_PATH, assetName);
                string dirName = Path.GetDirectoryName(path);
                if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
                File.WriteAllBytes(path, assetLoader.getData());
                assetLoader.onDispose();
                //DBManager.GetInstance().update<DynamicAsset>(new KeyValuePair<string, string>("isUpdate", "True"), new KeyValuePair<string, string>("id", assetName));
                handleLoadAssetComplete();
            }
            else
            {
                assetLoader.onDispose();
                if (null != currentAssetLoadInfo.CallBack) currentAssetLoadInfo.CallBack(false);
                loadNextAsset();
            }
        }
        void handleLoadAssetComplete()
        {
            //判断当前资源加载队列已加载完毕
            if (currentLoadAssetIndex == currentLoadAssetTotalCount)
            {
                currentLoadAssetIndex = 0;
                if (null != currentAssetLoadInfo.CallBack) currentAssetLoadInfo.CallBack(true);
                loadNextAsset();
            }
            else
            {
                currentLoadAssetIndex++;
                loadAsset(currentLoadAssetIndex);
            }
        }
        void loadNextAsset()
        {
            if (0 == assetLoadQueue.Count)
            {
                currentAssetLoadInfo = null;
                //loadingView.loadAssetPanel.setActive(false);
            }
            else
            {
                currentAssetLoadInfo = assetLoadQueue.Dequeue();
                currentLoadAssetTotalCount = currentAssetLoadInfo.assetNameList.Count;
                currentLoadAssetIndex = 1;
                loadAsset(currentLoadAssetIndex);
            }
        }

        /// <summary>
        /// 设置loading小球显示或者消失
        /// </summary>
        /// <param name="ev"></param>
        private void HandleSetLoadingAni(EventObject ev)
        {
            Debug.Log("HandleLoadingAni...");
            bool isShow = (bool)ev.param;
            loadingView.LoadingPanel.SetActive(isShow);
        }


        /// <summary>
        /// 加载场景事件
        /// </summary>
        /// <param name="ev"></param>
        private void HandleLoadScene(EventObject ev)
        {
            Debug.Log("HandleLoadScene...");
            currentSceneName = ev.param.ToString();
            if (null == sceneLoadAsync)
            {
                if (!currentSceneName.Equals("MainScene"))
                {
                    sceneLoadAsync = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
                    if (null == sceneLoadAsync)
                    {
                        Debug.Log("场景不存在");
                        return;
                    }
                    //禁止加载完成后自动切换场景
                    sceneLoadAsync.allowSceneActivation = false;
                    sceneSimulateProgress = 0;
                    sceneUpdateTick.reStart();
                }
                else
                {
                    Scene scene = SceneManager.GetActiveScene();
                    sceneLoadAsync = SceneManager.UnloadSceneAsync(scene);
                    sceneSimulateProgress = 0;
                    sceneUpdateTick.reStart();
                }
            }
            else
            {
                Debug.Log("场景加载中，不能叠加");
            }
        }

        private void UpdateLoadingScene()
        {
            if (sceneSimulateProgress < 0.9)
            {
                if (sceneSimulateProgress >= sceneLoadAsync.progress)
                    sceneSimulateProgress = sceneLoadAsync.progress;
                else
                    sceneSimulateProgress += 0.04f;
            }
            else
            {
                if (sceneSimulateProgress >= 1)
                {
                    if (!sceneLoadAsync.allowSceneActivation)
                        sceneLoadAsync.allowSceneActivation = true;
                    sceneSimulateProgress = 1;
                }
                else
                    sceneSimulateProgress += 0.04f;
            }
            if (sceneLoadAsync.isDone)
            {
                Debug.Log("场景加载完成");
                Scene scene = SceneManager.GetSceneByName(currentSceneName);
                SceneManager.SetActiveScene(scene);
                sceneUpdateTick.stop();
                //loadingView.setActive(false);
                //sendModuleEvent(ModuleEventType.SET_LOADING_ANI, false);
                sceneLoadAsync = null;
                sendModuleEvent(ModuleEventType.SCENE_ENTER, currentSceneName);
            }
            else
            {
                //loadingView.SetProgressText(sceneSimulateProgress.ToString());
                //loadingView.SetImageMaskFillAmount(1-sceneSimulateProgress);
            }
        }
        protected override void onButtonClick(EventObject ev)
        {
            Debug.Log("onButtonClick...");
            base.onButtonClick(ev);
            if (ev.param.Equals(loadingView.TestBtn))
            {
                Debug.Log("TestBtn...");
                Model6DoFInfo cameraInfo = new Model6DoFInfo();
                cameraInfo.pos = Camera.main.transform.position;
                cameraInfo.rot = Camera.main.transform.rotation;
                sendModuleEvent(ModuleEventType.START_POSITION, cameraInfo);
                //cameraImage.GetImageAsync();
            }
        }

        public override void onRemove()
        {
            base.onRemove();
        }
    }
}
