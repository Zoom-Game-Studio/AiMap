using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SkyJsonManager : MonoBehaviour
{
    public static SkyJsonManager instance;
    public static  string PathURL;

  //  UnityWebRequest request;
//   public    AssetBundle ab;

    HttpDownLoad http;
    bool isDone;
    bool jut;
    bool isStart;
   // string VRSavePath;
  //  string VRSaveName;

    //string name;
    //int isArCore;
    //string filesPath;
    //string cover;
    //string title;
    //string modelNo;
    //int modeType;
    private void Awake()
    {
        instance = this;
     //  DontDestroyOnLoad(this );
    }
    void Start()
    {
      
      
        isStart = true;
    }
    public void ReadSkyJson(string jsonStr)
    {
      
        Debug.Log("jsonStr:" + jsonStr);
      
        SkyRoot root = JsonMapper.ToObject<SkyRoot>(jsonStr);
        List<SkyDataItem> skyDataItems = new List<SkyDataItem>();
       skyDataItems = root.data;

        PathURL = skyDataItems[0].path;
        string modelID= skyDataItems[0].modelNo;
       
        PlayerPrefs.SetString("VRSkyPath", skyDataItems[0].path); //储存模型名称
        PlayerPrefs.SetInt ("VRModelType", skyDataItems[0].modeType ); //储存模型名称
        PlayerPrefs.SetString("VRModelCover", skyDataItems[0].cover); //储存模型名称
                                                                      // string modelPath = root.filesPath + skyDataItems[0].modelNo;
       // string modelPath = root.filesPath + "202006040053"+"/";
       string modelPath = root.filesPath + skyDataItems[0] .modelNo+ "/";
        PlayerPrefs.SetString("VRModelPath", modelPath);
        PlayerPrefs.SetString("VRModelName", skyDataItems[0].modelNo);
        //PlayerPrefs.SetString("VRModelNameID", "202006040053");
        PlayerPrefs.SetString("VRFilesPath", root.filesPath); //储存本地地址
        PlayerPrefs.SetInt("VRisArCore", root.isArCore); //储存
     //   Debug.Log("modelPath is :" + PlayerPrefs.GetString("VRModelPath")) ;
    //    Debug.Log("VRSkyPath is :" + PlayerPrefs.GetString("VRSkyPath"));

        PlayerPrefs.SetString("skyBoxPath", skyDataItems[0].path); //天空盒子
    //    AppManager.Instance.modelType = skyDataItems[0].modeType.ToString();
       // VRDownLoad(modelPath);
       // StartCoroutine(PlayerPrefs.GetString("VRSkyPath"));
    }
 

       
   


    
    void LoadLevel()
    {
        //m += 1;
        isDone = true;
    }
    void judeg(bool t)
    {
        jut = t;
    }
}
