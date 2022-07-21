using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using LitJson;

namespace AppLogic
{
    public class LocalizationToolMediator : BaseMediator
    {
        LocalizationToolView localizationToolView;
        List<ChangeModelInfo> deltaModelInfoList = new List<ChangeModelInfo>();
        public string currentModelID = null;
        public GameObject currentModelObj = null;
        float deltaT = 0.1f;
        float deltaR = 10f;
        float deltaScale = 0.1f;
        public LocalizationToolMediator()
        {
            m_mediatorName = MEDIATOR.LOCALIZATION_TOOL;
        }

        protected override void onInit()
        {
            localizationToolView = viewComponent as LocalizationToolView;
            addModuleEvent(ModuleEventType.SAVE_MODEL_INFO, HandleSaveModelInfo);
            addModuleEvent(ModuleEventType.MODEL_PICKER, HandleModelPicker);
            GetDeltaModelInfoList();
        }

        void GetDeltaModelInfoList()
        {
            string path = "";
            string json = UnityUtilties.ReadTxtByFileName(path);
            if (json == null) return;

            List<ChangeModelInfoSavingFormat> savingFromatList = JsonMapper.ToObject<List<ChangeModelInfoSavingFormat>>(json);
            foreach (ChangeModelInfoSavingFormat item in savingFromatList)
            {
                ChangeModelInfo deltaModelInfo = new ChangeModelInfo();
                deltaModelInfo.modelId = item.modelId;
                deltaModelInfo.eulerAngle = UnityUtilties.StringToVector3(item.eulerAngle);
                deltaModelInfo.pos = UnityUtilties.StringToVector3(item.pos);
                deltaModelInfo.scale = UnityUtilties.StringToVector3(item.scale);
                deltaModelInfoList.Add(deltaModelInfo);
            }
            //deltaModelInfoList = JsonMapper.ToObject<List<ChangeModelInfo>>(json);
            Debug.Log(deltaModelInfoList.Count);
        }

        private void HandleModelPicker(EventObject ev)
        {
            GameObject pickerModel = ev.param as GameObject;
            if (pickerModel == null) return;
            currentModelObj = pickerModel;
            Debug.Log(pickerModel.name);
        }

        private void HandleSaveModelInfo(EventObject ev)
        {
            Debug.Log(deltaModelInfoList.Count);
            List<ChangeModelInfoSavingFormat> savingFormatList = new List<ChangeModelInfoSavingFormat>();
            foreach (ChangeModelInfo item in deltaModelInfoList)
            {
                ChangeModelInfoSavingFormat savingFormat = new ChangeModelInfoSavingFormat();
                savingFormat.pos = UnityUtilties.Vector3ToString(item.pos);
                savingFormat.eulerAngle = UnityUtilties.Vector3ToString(item.eulerAngle);
                savingFormat.scale = UnityUtilties.Vector3ToString(item.scale);
                savingFormat.modelId = item.modelId;
                savingFormatList.Add(savingFormat);
            }

            string jsonStr = JsonMapper.ToJson(savingFormatList);
            Debug.Log(jsonStr + "@louis");
            bool isSucc = UnityUtilties.AddTxtTextByFileStream(jsonStr);
            localizationToolView.SetSaveStutusText(isSucc.ToString());
        }
        void ChangeModelDeltaInfo(ChangeModelInfo deltaInfo)
        {
            foreach (ChangeModelInfo item in deltaModelInfoList)
            {
                if (item.modelId.Equals(deltaInfo.modelId))
                {
                    //Debug.Log(item.scale.ToString()+"/"+deltaInfo.scale.ToString());
                    Vector3 pos = item.pos + deltaInfo.pos;
                    Vector3 eulerAngles = item.eulerAngle + deltaInfo.eulerAngle;
                    Vector3 scale = item.scale + deltaInfo.scale;

                    item.pos = pos;
                    item.eulerAngle = eulerAngles;
                    item.scale = scale;
                    sendModuleEvent(ModuleEventType.CHANGE_MODEL_INFO, item);
                    return;
                }
            }
            ChangeModelInfo newModelInfo = new ChangeModelInfo();
            newModelInfo.pos = deltaInfo.pos;
            newModelInfo.eulerAngle = deltaInfo.eulerAngle;
            newModelInfo.scale = deltaInfo.scale;
            newModelInfo.modelId = deltaInfo.modelId;
            deltaModelInfoList.Add(newModelInfo);
            sendModuleEvent(ModuleEventType.CHANGE_MODEL_INFO, newModelInfo);

        }
        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(localizationToolView.AddRXBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(deltaR, 0, 0);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;
                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddRYBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, deltaR, 0);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddRZBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, 0, deltaR);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddTXBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(deltaT, 0, 0);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddTYBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, deltaT, 0);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddTZBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, 0, deltaT);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceRXBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(-deltaR, 0, 0);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceRYBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, -deltaR, 0);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceRZBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, 0, -deltaR);
                changeInfo.eulerAngle = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }

            else if (ev.param.Equals(localizationToolView.ReduceTXBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(-deltaT, 0, 0);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceTYBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, -deltaT, 0);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceTZBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(0, 0, -deltaT);
                changeInfo.pos = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.ReduceScaleBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(-deltaScale, -deltaScale, -deltaScale);
                changeInfo.scale = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.AddScaleBtn))
            {
                ChangeModelInfo changeInfo = new ChangeModelInfo();
                Vector3 delta = new Vector3(deltaScale, deltaScale, deltaScale);
                changeInfo.scale = delta;
                changeInfo.model = currentModelObj;
                changeInfo.modelId = currentModelObj.name;

                ChangeModelDeltaInfo(changeInfo);
            }
            else if (ev.param.Equals(localizationToolView.BackBtn))
            {
                sendModuleEvent(ModuleEventType.MODULE_EXIT, MEDIATOR.LOCALIZATION_TOOL);
            }
            else if (ev.param.Equals(localizationToolView.SaveBtn))
            {
                sendModuleEvent(ModuleEventType.SAVE_MODEL_INFO, deltaModelInfoList);
            }

        }

        public override void onRemove()
        {
            base.onRemove();
        }
    }
}
