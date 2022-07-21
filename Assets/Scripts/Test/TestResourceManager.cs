using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppLogic;
using KleinEngine;
using System;
using UnityEngine.UI;
using AppLogic;
using LitJson;

public class TestResourceManager : MonoBehaviour
{
    //private string abname = "9l2_color";
    private string abname = "juxincheku";

    //private string assetName1 = "9L2_color_01";
    private string assetName1 = "juxincheku_001";

    private string assetName2 = "9L2_color_03";

    private string data = "data";
    private string model = "model";

    private GameObject modelObj;

    private string parallel = "parallelverse";

    Tick tick;
    void Start()
    {
        Button1();

        tick = TickManager.GetInstance().createTick(0, null, ModelLerp);
        tick.start();
    }

    void ModelLerp()
    {
        return;
        if (modelObj == null) return;
        modelObj.transform.position = Vector3.Lerp(modelObj.transform.position, new Vector3(10, 10, 10), Time.deltaTime);
        modelObj.transform.eulerAngles = Vector3.Lerp(modelObj.transform.position, new Vector3(10, 10, 10), Time.deltaTime);
    }

    private void Update()
    {
        ResourceManager.GetInstance().update();
        if (modelObj == null) return;
        modelObj.transform.position = Vector3.Lerp(modelObj.transform.position, new Vector3(10, 10, 10), Time.deltaTime);
        modelObj.transform.rotation = Quaternion.Lerp(modelObj.transform.rotation, Quaternion.Euler(new Vector3(100, 200, 300)), Time.deltaTime);
    }

    bool loadCompleted = false;
    List<GameObject> modelList = new List<GameObject>();
    List<ChangeModelInfo> prefabsInfoList = new List<ChangeModelInfo>();
    int loadedPrefabsNum = 0;
    int prefabsNum = 0;

    private void CallBack(UnityEngine.Object obj)
    {
        if (loadCompleted) return;
        GameObject prefab = (GameObject)obj;
        GameObject go = GameObject.Instantiate<GameObject>(prefab);
        go.name = go.name.Replace("(Clone)", "");
        modelList.Add(go);
        modelObj = go;
        for (int i = 0; i < prefabsInfoList.Count; i++)
        {
            if (!prefabsInfoList[i].modelId.Equals(prefab.name)) continue;
        }

        loadedPrefabsNum++;
        if (prefabsNum.Equals(loadedPrefabsNum))
        {
            loadCompleted = true;
            Debug.Log("Done...");
        }
    }

    public void Button1()
    {
        //GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/9L_shengdan";
        GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/juxincheku";

        TextAsset asset = ResourceManager.GetInstance().loadAsset<TextAsset>("data", model);
        IDictionary modelInfos = ConfigManager.GetConfigs<ModelConfigInfo>();
        Debug.Log(modelInfos.Count);
        foreach (ModelConfigInfo modelInfo in modelInfos.Values)
        {
            Debug.Log(modelInfo.id);
            ChangeModelInfo changeModelInfo = new ChangeModelInfo();

            changeModelInfo.abName = modelInfo.abName;
            changeModelInfo.modelId = modelInfo.id;
            prefabsInfoList.Add(changeModelInfo);
        }
        GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/juxincheku/model";
        prefabsNum = prefabsInfoList.Count;
        foreach (ChangeModelInfo modelInfo in prefabsInfoList)
        {
            LoadModel(modelInfo);
        }

        //SendParallelverseInfo();

        //TextAsset assetP = ResourceManager.GetInstance().loadAsset<TextAsset>("data", parallel);
        //ParallelverseConfigInfo dd = ConfigManager.GetConfigInfo<ParallelverseConfigInfo>("1")
        //Debug.Log(dd.id);
        //Debug.Log("@louis>>>>>>>>>>>>>>>"+assetP.ToString());

        //ResourceManager.GetInstance().loadAssetAsync<GameObject>(abname, assetName1, CallBack);
    }

    void LoadModel(ChangeModelInfo modelInfo)
    {
        Debug.Log(modelInfo.abName);
        string assetName = modelInfo.abName.Split('/')[1];
        //string assetName = "model/" + modelInfo.abName.Split('/')[1];

        Debug.Log("abname:" + abname + "////" + "assetName:" + assetName);
        ResourceManager.GetInstance().loadAssetAsync<GameObject>(abname, assetName, CallBack);
    }

    List<ParallelverseInfo> parallelverseInfoList = new List<ParallelverseInfo>();
    List<string> parallelIDs = new List<string>();//该场景包含的所有可替换的id
    List<string> parallelModelIDs = new List<string>();//该场景包含的所有模型的id
    List<string> parallelNames = new List<string>();//该场景包含的所有可替换的名字


    void SendParallelverseInfo()
    {
        //Debug.Log("SendParallelverseInfo...");

        parallelIDs.Clear();
        parallelverseInfoList.Clear();
        parallelModelIDs.Clear();

        GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/SuperDemo";

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
        ParallelverseInfos infos = new ParallelverseInfos();
        infos.parallesverseList = parallelverseInfoList;
        string json = JsonMapper.ToJson(infos);
        if (json == null) return;
        Debug.Log("SendParallelverseInfo..." + json);
        //SDKManager.PhoneMethodForSendParallelverseInfo(json);
    }

    bool is1 = false;
    public void Button1OnClick()
    {
        is1 = !is1;
        ParallelverseModelInfo info = new ParallelverseModelInfo();
        info.modelList = modelList;
        if (is1)
            info.id = "1";
        else
            info.id = "2";

        HandleSwitchParallelUniverseCallBack(info);
    }

    private void HandleSwitchParallelUniverseCallBack(ParallelverseModelInfo modelInfo)
    {
        Debug.Log("HandleSwitchParallelUniverse...");
        //ParallelverseModelInfo modelInfo = ev.param as ParallelverseModelInfo;
        if (modelInfo == null) return;
        int id = int.Parse(modelInfo.id);
        ParallelverseConfigInfo info = ConfigManager.GetConfigInfo<ParallelverseConfigInfo>(id);
        if (info == null) return;
        SwitchModel(info.name, modelInfo.modelList);

    }

    private void SwitchModel(string enableName, List<GameObject> modelList)
    {
        Debug.Log("SwitchModel..." + enableName);
        Debug.Log(modelList.Count);
        foreach (var model in modelList)
        {
            if (model == null) continue;
            Debug.Log(model.name);
            if (parallelModelIDs.Contains(model.name))
            {
                //Transform tran = model.transform.Find(enableName);
                //if (tran == null) continue;
                //tran.gameObject.SetActive(true);
                SwitchModelByName(enableName, model);
            }
        }
    }

    void SwitchModelByName(string enableName, GameObject model)
    {
        if (model == null) return;
        Debug.Log("SwitchModelByName...");
        foreach (var name in parallelNames)
        {
            //Debug.Log("@louisparallelName:" + name);
            if (name.Equals(enableName))
            {
                Transform tran = model.transform.Find(enableName);
                if (tran == null) continue;
                tran.gameObject.SetActive(true);
                Debug.Log("DiableModelByName...true");
                continue;
            }
            Transform otherTran = model.transform.Find(name);
            if (otherTran == null) return;
            otherTran.gameObject.SetActive(false);
            Debug.Log("DiableModelByName...false");
        }
    }

    public void Button2()
    {

    }
    public void Button3()
    {
        prefabsInfoList.Clear();
        modelList.Clear();
        loadedPrefabsNum = 0;
        GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/9L_kaixue";
        TextAsset asset = ResourceManager.GetInstance().loadAsset<TextAsset>("data", model);
        IDictionary modelInfos = ConfigManager.GetConfigs<ModelConfigInfo>();
        Debug.Log("modelinfos" + modelInfos.Count);
        foreach (ModelConfigInfo modelInfo in modelInfos.Values)
        {
            ChangeModelInfo changeModelInfo = new ChangeModelInfo();
            changeModelInfo.abName = modelInfo.abName;
            changeModelInfo.modelId = modelInfo.id;
            prefabsInfoList.Add(changeModelInfo);
        }
        GlobalObject.ASSET_PATH = Application.streamingAssetsPath + "/9L_kaixue/model";
        prefabsNum = prefabsInfoList.Count;
        Debug.Log(prefabsNum + "@louis");
        foreach (ChangeModelInfo modelInfo in prefabsInfoList)
        {
            LoadModel(modelInfo);
        }
    }

}
