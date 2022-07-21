using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SkyboxManager : MonoBehaviour
{

  
    public static string PathURL;
    public  GameObject skyChangeBu;
    UnityWebRequest request;
    AssetBundle ab;
    public Text noteText;
    //void Awake()
    //{
    //    //if (Application.platform == RuntimePlatform.Android)
    //    //    PathURL = "jar:file://" + Application.dataPath + "!/assets/";
    //    //else if (Application.platform == RuntimePlatform.OSXPlayer)
    //    //    PathURL = Application.dataPath + "/Raw/";
    //    //else
    //    //       PathURL = "file://" + Application.dataPath + "/StreamingAssets/skytheme.sky";
    //    //// PathURL = @"C:\Users\Administrator\Desktop\skyBox\skytheme.sky";

    //    //PathURL = "file://" + Application.dataPath + "/StreamingAssets/skytheme.sky";
    //    //string path = PlayerPrefs.GetString("skyBoxPath");
    //    //print("path skybox is:" + path);
    //    //StartCoroutine(ReadFromWeb(path));
    //}

    

    private void Start()
    {
        //ab = SkyJsonManager.instance.ab;
        //changeTexture(SkyJsonManager .instance .ab , "04");
        string path = PlayerPrefs.GetString("skyBoxPath");
        print("path skybox is:" + path);
        StartCoroutine(ReadFromWeb(path));
        skyChangeBu.SetActive(false );
    }
    /// <summary>
    /// 网络上下载
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator ReadFromWeb(string url)
    {
        // print("1111");
        request = UnityWebRequestAssetBundle.GetAssetBundle(url);

        float LoadProgress = request.downloadProgress;
        Debug.Log("LoadProgress :" + LoadProgress.ToString());
        noteText.gameObject.SetActive(true);
        noteText.text = "开始初始化背景.....";
        yield return request.SendWebRequest();
      
            ab = DownloadHandlerAssetBundle.GetContent(request);
            changeTexture(ab, "04");
            Debug.Log("sky download is over");
        noteText.text = "加载完成";
        Invoke ("HideText",1);
        skyChangeBu.SetActive(true);

    }
    void HideText()
    {
        noteText.gameObject.SetActive(false );
        noteText.text = "暂时没有数据";
    }
    
    //void LoadAssetBundle(UnityWebRequest request)
    //{
    //    ab = DownloadHandlerAssetBundle.GetContent(request);
    //    changeTexture(ab, "04");

    //}
    void changeTexture(AssetBundle ab, string  num)
    {
       
        Texture front = ab.LoadAsset<Texture>("front_"+ num);
        Texture back = ab.LoadAsset<Texture>("back_" + num);
        Texture left = ab.LoadAsset<Texture>("left_" + num);
        Texture right = ab.LoadAsset<Texture>("right_" + num);
        Texture top = ab.LoadAsset<Texture>("top_" + num);

        RenderSettings.skybox.SetTexture("_FrontTex", front);
        RenderSettings.skybox.SetTexture("_BackTex", back);
        RenderSettings.skybox.SetTexture("_LeftTex", left);
        RenderSettings.skybox.SetTexture("_RightTex", right);
        RenderSettings.skybox.SetTexture("_UpTex", top);
    }
    private Texture TransformToTexture(Sprite sprite)
    {
        var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);
        targetTex.SetPixels(pixels);
        targetTex.Apply();
        return targetTex;

    }

    IEnumerator LoadFromMemoryAsyn(string url)
    {


        print("1111");
        AssetBundleCreateRequest request1 =
            AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(url)); //读取文件1请求
       
        yield return request1;
        
        AssetBundle ab1 = request1.assetBundle;
        Material sky = ab1.LoadAsset<Material>("Skybox9");
      //加载 资源ab1中的名叫“Sphere-Head”的圆球
        RenderSettings.skybox = sky;

    }
    int num = 1;
    string index;
    public   void skyboxChange()
    {
        
      
        
        if (num <5)
        {
            if (num < 10)
            {
                index = "0" + num.ToString();
            }
            else
            {
                index = num.ToString();
            }
            changeTexture(ab ,index );
            num++;
        }
        else
        {
            num = 1;
            index = "0" + num.ToString();
            changeTexture(ab, index);
            num++;
        }
        
        //num++;
    }

}
