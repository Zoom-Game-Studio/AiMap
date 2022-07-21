using System;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using System.Collections;

namespace AppLogic
{
    public class ControlMediator : BaseMediator
    {        
        RectTransform canvas;
        Dictionary<string, RectTransform> layerDic;
        List<string> loadingView = new List<string>();
        List<string> backView = new List<string>();

        public ControlMediator()
        {
            Debug.Log("ControlMediator...");
            m_mediatorName = MEDIATOR.CONTROL;
        }

        public override void onRegister()
        {
            //InitLayer();
            addModuleEvent(ModuleEventType.MODULE_ENTER, handleModuleEnter);
            addModuleEvent(ModuleEventType.MODULE_EXIT, handleModuleExit);
            addModuleEvent(ModuleEventType.MODULE_REMOVE, handleModuleRemove);
            addModuleEvent(ModuleEventType.UI_HIDE, handleUiHide);
            addModuleEvent(ModuleEventType.SCENE_BACK, handleSceneBack);
        }

        /// <summary>
        /// TODO
        /// </summary>
        void InitLayer()
        {
            layerDic = new Dictionary<string, RectTransform>();
            canvas = GameObject.Find("Canvas").transform as RectTransform;
            IDictionary layers = ConfigManager.GetConfigs<LayerConfigInfo>();
            if (null != layers)
            {
                foreach (LayerConfigInfo layerCfgInfo in layers.Values)
                {
                    RectTransform rect = createLayer(layerCfgInfo.name);
                    layerDic[layerCfgInfo.id] = rect;
                }
            }
        }

        RectTransform createLayer(string layerName)
        {
            GameObject layerObj = new GameObject(layerName);
            layerObj.transform.SetParent(canvas, false);
            layerObj.layer = LayerMask.NameToLayer("UI");
            RectTransform rect = layerObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            return rect;
        }

        void handleUiHide(EventObject ev)
        {
            UIHideInfo info = ev.param as UIHideInfo;
            switch(info.type)
            {
                case 0:
                    canvas.gameObject.SetActive(!info.hide);
                    break;
                case 1:
                    foreach(var trans in layerDic.Values)
                    {
                        trans.gameObject.SetActive(!info.hide);
                    }
                    break;
            }
        }

        void handleSceneBack(EventObject ev)
        {
            if (backView.Count < 2) return;
            string oldName= backView[backView.Count - 2];
            string newName = backView[backView.Count - 1];
            backView.Remove(oldName);
            backView.Remove(newName);
            //getProxy<CarProxy>().setSceneBackOld(string.Empty);

            //if (oldName.Equals("Main"))
            //{
            //    getProxy<SchemeProxy>().setCurrentSchemeListInfo(null);
            //    getProxy<CarProxy>().setBilling(false);
            //    getProxy<CarProxy>().setSchemeEdit(false);
            //    sendModuleEvent(ModuleEventType.SKY_STOP, false);
            //    sendModuleEvent(ModuleEventType.LOADING_SCENE, SCENE_NAME.MAINSCENE);
            //}
            //else
            //{
            //    sendModuleEvent(ModuleEventType.MODULE_EXIT, newName);
            //    sendModuleEvent(ModuleEventType.MODULE_ENTER, oldName);
            //}
        }

        void onBackView(ModuleConfigInfo moduleCfg)
        {
            //if (moduleCfg.limitBack) return;
            //int max = backView.Count - 1;
            //if (max > 0 && backView[max] == moduleCfg.id) return;
            //if(max >= 0) getProxy<CarProxy>().setSceneBackOld(backView[max]);
            //backView.Add(moduleCfg.id);
            //getProxy<CarProxy>().setModuleName(moduleCfg.id);
        }

        void handleModuleEnter(EventObject ev)
        {
            string moduleId = ev.param.ToString();
            ModuleConfigInfo cfgInfo = ConfigManager.GetConfigInfo<ModuleConfigInfo>(moduleId);
            if (null == cfgInfo)
            {
                Debug.Log("模块配置不存在：" + moduleId);
                return;
            }
            onBackView(cfgInfo);
            IMediator mediator = AppFacade.GetInstance().retrieveMediator(cfgInfo.id);
            if (null == mediator)
            {
                if (string.IsNullOrEmpty(cfgInfo.abName))
                {
                    onCreateModule(moduleId, null);
                    return;
                }
                if (loadingView.Contains(moduleId)) return;
                loadingView.Add(moduleId);
                string abPath = PATH_TYPE.UI + cfgInfo.abName;
                string assetName = cfgInfo.id + "View";
                onLoadModuleComplete(ResourceManager.GetInstance().loadAsset<GameObject>(abPath, assetName));
                //ResourceManager.GetInstance().loadAssetAsync<GameObject>(abPath, assetName, onLoadModuleComplete);
            }
            else
            {
                if (mediator.state.Equals(BaseMediator.STATE_ENTER)) return;
                mediator.onEnter();
            }
        }

        void handleModuleExit(EventObject ev)
        {
            string moduleName = ev.param.ToString();
            IMediator mediator = AppFacade.GetInstance().retrieveMediator(moduleName);
            if (null == mediator) return;
            if (mediator.state.Equals(BaseMediator.STATE_EXIT)) return;
            mediator.onExit();
        }

        void handleModuleRemove(EventObject ev)
        {
            string moduleName = ev.param.ToString();
            ResourceManager.GetInstance().unloadBundle(PATH_TYPE.UI + moduleName, true);
            AppFacade.GetInstance().removeMediator(moduleName);
        }

        void onLoadModuleComplete(object obj)
        {
            GameObject prefab = obj as GameObject;
            string moduleId = prefab.name.Substring(0, prefab.name.Length - 4);
            if (loadingView.Contains(moduleId))
            {
                loadingView.Remove(moduleId);
                onCreateModule(moduleId, prefab);                
            }            
        }

        void onCreateModule(string moduleId, GameObject prefab)
        {
            BaseView view = null;
            ModuleConfigInfo cfgInfo = ConfigManager.GetConfigInfo<ModuleConfigInfo>(moduleId);
            if (null == cfgInfo) return;
            if (null != prefab)
            {
                GameObject go = GameObject.Instantiate(prefab) as GameObject;                
                if (layerDic.ContainsKey(cfgInfo.layer))
                    go.transform.SetParent(layerDic[cfgInfo.layer], false);
                go.transform.SetAsLastSibling();
                string viewName = cfgInfo.id + "View";
                go.name = viewName;
                Type viewtype = Type.GetType("AppLogic." + viewName);//需要加上命名空间
                if (null != viewtype)
                {
                    view = Activator.CreateInstance(viewtype) as BaseView;
                    view.init(go);
                }
                else
                {
                    Debug.LogWarning("类型不存在:" + viewName);
                }
            }

            string clsName = "AppLogic." + cfgInfo.id + "Mediator";
            Type t = Type.GetType(clsName);
            if(null != t)
            {
                IMediator mediator = Activator.CreateInstance(t) as IMediator;
                mediator.viewComponent = view;
                AppFacade.GetInstance().registerMediator(mediator);
            }
            else
            {
                Debug.LogWarning("类型不存在:" + clsName);
            }
        }
    }
}
