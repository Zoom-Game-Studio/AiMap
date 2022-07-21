using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using LitJson;

namespace AppLogic
{
    public class ParallelverseCommand : BaseCommand
    {
        List<ParallelverseInfo> parallelverseInfoList = new List<ParallelverseInfo>();
        List<string> parallelIDs = new List<string>();//该场景包含的所有可替换的id
        List<string> parallelModelIDs = new List<string>();//该场景包含的所有模型的id
        List<string> parallelNames = new List<string>();//该场景包含的所有可替换的名字
        public override void onExecute(object param)
        {
            addModuleEvent(ModuleEventType.SWITCH_PARALLEL_UNIVERSE_CALL_BACK, HandleSwitchParallelUniverseCallBack);
            SendParallelverseInfo();
        }

        void SendParallelverseInfo()
        {
            //Debug.Log("SendParallelverseInfo...");

            parallelIDs.Clear();
            parallelverseInfoList.Clear();
            parallelModelIDs.Clear();
            IDictionary modelCfgs = ConfigManager.GetConfigs<ModelConfigInfo>();
            if (modelCfgs == null) { Debug.Log("modelCfgs is null..."); return; }
            foreach (ModelConfigInfo modelCfg in modelCfgs.Values)
            {
                //Debug.Log("SendParallelverseInfo...1");
                if (modelCfg.parallelverse == null) continue;
                if (modelCfg.parallelverse.Equals(string.Empty)) continue;
                parallelModelIDs.Add(modelCfg.id);
                foreach (string id in modelCfg.parallelverse)
                {
                    if (parallelIDs.Contains(id)) continue;
                    parallelIDs.Add(id);
                }
            }

            foreach (string id in parallelIDs)
            {
                if (id == null) continue;
                int tmpId = int.Parse(id);
                ParallelverseConfigInfo info = ConfigManager.GetConfigInfo<ParallelverseConfigInfo>(tmpId);

                if (info == null) { Debug.Log("info is null..."); continue; }
                ParallelverseInfo parallelverseInfo = new ParallelverseInfo();
                parallelverseInfo.id = info.id;
                parallelverseInfo.name = info.name;
                parallelverseInfo.remark = info.remark;
                parallelverseInfoList.Add(parallelverseInfo);
                if (parallelNames.Contains(info.name)) continue;
                parallelNames.Add(info.name);
            }
            if (parallelverseInfoList.Count < 1) return;
            ParallelverseInfos infos = new ParallelverseInfos();
            infos.parallesverseList = parallelverseInfoList;
            string json = JsonMapper.ToJson(infos);
            if (json == null) return;
            SDKManager.PhoneMethodForSendParallelverseInfo(json);
        }

        private void HandleSwitchParallelUniverseCallBack(EventObject ev)
        {
            ParallelverseModelInfo modelInfo = ev.param as ParallelverseModelInfo;
            if (modelInfo == null) return;
            int id = int.Parse(modelInfo.id);
            ParallelverseConfigInfo info = ConfigManager.GetConfigInfo<ParallelverseConfigInfo>(id);
            if (info == null) return;
            SwitchModel(info.name, modelInfo.modelList);
        }

        private void SwitchModel(string enableName, List<GameObject> modelList)
        {
            foreach (var model in modelList)
            {
                if (model == null) continue;
                if (parallelModelIDs.Contains(model.name))
                {
                    //Debug.Log("switchmodel..." + model.name);
                    SwitchModelByName(enableName, model);
                }
            }
        }

        void SwitchModelByName(string enableName, GameObject model)
        {
            if (model == null) return;
            foreach (var name in parallelNames)
            {
                //Debug.Log("@SwitchModelByName:" + name);
                if (name.Equals(enableName))
                {
                    Transform tran = model.transform.Find(enableName);
                    if (tran == null) continue;
                    tran.gameObject.SetActive(true);
                    continue;
                }
                Transform otherTran = model.transform.Find(name);
                if (otherTran == null) continue;
                otherTran.gameObject.SetActive(false);
                //Debug.Log("DiableModelByName...false" + name);

            }
        }
    }
}
