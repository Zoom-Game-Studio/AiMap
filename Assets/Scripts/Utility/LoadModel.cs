using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public enum ModelType
{
    KOREA_8i = 1,
    UNITYPACKAGE = 2,
    OBJ = 3,
    SAMPLE_8i = 4,
    ASSETBUNDLE = 5
}

public enum SceneName
{
    Video = 1,
    ARModel = 2,
    ARModel_ImageTarge = 5,
    VRTheater = 6
}

public class LoadModel : SingletonTemplate<LoadModel>
{
    private GameObject model; //当前下载的模型对象
    private Animator ani; //animotor 控制器
    private float goX = 0.2f;
    private float goY = 0.2f;
    private float goZ = 0.2f;

    //public GameObject prefabHvr;

    //public GameObject parent;

    // Use this for initialization
    void Start()
    {
        //    setText("http://dlvod.tv189.com/models/AssetBundles/hvractor_president");        
        //    UnityWebRequest.
        //HvrActor h = FindObjectOfType<HvrActor>();
        //string path = Path.Combine(Application.persistentDataPath + "president.8i");
        //h.SetAssetData(path,HvrActor.eDataMode.path);
        //        CreateModel();
        //CreateHvActor();

        //        hvrActor = FindObjectOfType<HvrActor>();
        //        DontDestroyOnLoad(gameObject);
    }




    //android调用该方法，将模型路径传入
    public void CreateModel(string content)
    {
        //转成JSON串
        JsonData jsonObj = JsonMapper.ToObject(content);
        Debug.Log(jsonObj.ToString());
        if (jsonObj != null)
        {
            string modelType = jsonObj["moduleType"].ToString();
            Debug.Log("moduleType===" + modelType);
            if (modelType.Equals("4"))
            {
            }

            //解析file节点
            JsonData file = jsonObj["files"];
            Debug.Log("files====" + file.ToString());
            if (file != null && file.Count > 0)
            {
                //解析出具体模型路径
                var modelUrl = file[0]["path"].ToString();
                Debug.Log("modelUrl==== " + modelUrl);
                //AB 加载模型到内存,合成类型的项目 加载资源，路径不带file://"
                AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(modelUrl);
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                    return;
                }

                //拆分模型实例名,“/”最后一位就是实例名
                var splitArray = Regex.Split(modelUrl, "/", RegexOptions.IgnoreCase);
                string urlName = splitArray[splitArray.Length - 1];
                Debug.Log("urlName====" + urlName);
                //从内存中加载赋值模型对象
                model = myLoadedAssetBundle.LoadAsset<GameObject>(urlName);
                //赋值到新的模型对象，用以控制尺寸
                GameObject newModel = Instantiate(model);
                newModel.transform.SetParent(transform, false);
                //赋值尺寸
                newModel.transform.localScale = new Vector3(goX, goY, goZ);
                newModel.transform.localEulerAngles = new Vector3(0, 180, 0);
                if (newModel.GetComponent<FingerController>() == null)
                    newModel.AddComponent<FingerController>();
                //赋值动画控制器
                ani = newModel.GetComponent<Animator>();
                if (ani != null)
                    //播放实例名为jump的帧动画
                    ani.Play("jump");
                Debug.Log("position===" + newModel.transform.position.ToString());
                Debug.Log("rotation===" + newModel.transform.eulerAngles.ToString());
                Debug.Log("localscale===" + newModel.transform.localScale.ToString());

                myLoadedAssetBundle.Unload(false);
            }
            else
            {
                Debug.Log("解析错误");
            }
        }
    }
}