using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;

public class DownLoadModel : MonoBehaviour
{
    HttpDownLoad http;
    bool isDone;
    bool isBannerDone = false;
    bool isStart;
    bool jut;
    string SavePath;
    string SaveName;
    public Text text; //进度条
    int n;
    int u = 0;
    string modelCode;//版本号
    string modelCode_ = "";

   public string bannerURL = null;
    string savePath = null;

    void Start()
    {
         savePath = Application.persistentDataPath + "/ModelData/";
        //http://dlvod.tv189.com/models/AssetBundles/model_monster_dagonnew
        //http://dlvod.tv189.com/models/AssetBundles/model_role_soldier
        //string DownLoadUrl = "http://q2pf87nrj.bkt.clouddn.com/president.8i";
        //StartDownLoadModel(DownLoadUrl);
        //string a = "{ \"fileName\":\"model_activity_teacher\",\"cover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/06/7008423651.png\",\"title\": \"逢考必过\",\"modelCode\":\"1.0.0\",\"moduleType\":\"6\",\"files\":[{\"path\":\"http://dlvod.tv189.com/models/AssetBundles//model_activity_mouse06.bd\"}],\"smallCover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/06/test.png\"}";
        // //string a = { "title":"好运萌鼠","cover":"http:\/\/tp.nty.tv189.com\/image\/tmpl\/2020\/01\/09\/7008428150.png","smallCover":"http:\/\/tp.nty.tv189.com\/image\/tmpl\/2020\/01\/08\/7008427337.png","moduleType":"6","modelCode":"1.0.0","fileName":"model_activity_mouse03","files":[{"path":"http:\/\/dlvod.tv189.com\/models\/AssetBundles\/model_activity_mouse03.bd"}]}
        //SrartGetModelJson(a);

        string fileUrl = PlayerPrefs.GetString("BannerPath");
        if (fileUrl != null)
            if (File.Exists(fileUrl))
                File.Delete(fileUrl);
        


    }

    public void DownLoadModelBtnClicked()
    {
        //string a = "{ \"fileName\":\"model_activity_teacher\",\"cover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/06/7008423651.png\",\"title\": \"逢考必过\",\"modelCode\":\"1.0.0\",\"moduleType\":\"6\",\"files\":[{\"path\":\"http://dlvod.tv189.com/models/AssetBundles//model_activity_teacher.bd\"}],\"smallCover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/06/test.png\"}";
        string a = "{ \"title\":\"土豪萌鼠\",\"cover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/08/7008427329.png\",\"smallCover\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/08/7008427335.png.png\",\"moduleType\":\"6\",\"modelCode\":\"1.1.0\",\"fileName\":\"model_activity_mouse01\", \"banner\":\"http://tp.nty.tv189.com/image/tmpl/2020/01/22/7008441579.png\",\"files\":[{\"path\":\"http://tp.nty.tv189.com/h5/res/2020/unity/01/model_activity_mouse01.bd\"}]}";
        SrartGetModelJson(a);
    }
    void Update()
    {
        return;
        if (!isBannerDone && !isDone && isStart) //下载中
        {
            text.text = "正在玩命下载" + ((int)(http.progress * 100)).ToString() + "%" + "小主请稍后~"; //progress值默认0-1
        }

        if (isDone && isBannerDone && n == u) //下载完成
        {
            isStart = false;
            text.text = "正在玩命加载,小主请稍后~";
            isDone = false;
            SceneManager.LoadScene("NewYear"); //加载场景
        }
    }

    public void SrartGetModelJson(string url_) //传入模型下载地址json字符串
    {
        //Debug.Log("json====" + url_);
        //ABModelRoot ABModelRoot = JsonConvert.DeserializeObject<ABModelRoot>(url_); //调用json实体类
        //modelCode = ABModelRoot.modelCode; //版本号
        //bannerURL = ABModelRoot.banner;
        //Debug.Log("banner url====" + bannerURL);
        //if (PlayerPrefs.HasKey("setmodelCode"))
        //{
        //    modelCode_ = PlayerPrefs.GetString("setmodelCode");
        //}
        //PlayerPrefs.SetString("setmodelCode", modelCode); //存储
        //List<ABModelFilesItem> ABModelFilesItem = new List<ABModelFilesItem>();
        //ABModelFilesItem = ABModelRoot.files;
        //n = ABModelFilesItem.Count; //AB资源个数
        //for (int i = 0; i < ABModelFilesItem.Count; i++)
        //{
        //    string path = ABModelFilesItem[i].path; //下载地址
        //    StartDownLoadModel(path);
        //    print("+++++++" + path);
        //}
    }
    public void DownLoadBannerAndSave(string url)
    {
        if (url == null) { Debug.Log("banner url is null"); return; }
       
        string fileName = "banner.png";
        //string filePath = savePath + "banner.png";
        Debug.Log("banner save path====" + savePath);
       
        PlayerPrefs.SetString("BannerPath", savePath + fileName);
        http = new HttpDownLoad();
        http.DownLoad(url, savePath, fileName, DownloadBannerDone);
    }
    public void StartDownLoadModel(string url) //下载模型，传入下载地址
    {
        Debug.Log("Download Path===" + url);
        string[] a = url.Split('/');
        int b = a.Length - 1;
        SaveName = a[b];
        Debug.Log("SaveName===" + SaveName);
        SavePath = Application.persistentDataPath + "/ModelDate/";
        print("保存地址： " + SavePath);
        PlayerPrefs.SetString("ModelUrl", SavePath + SaveName); //储存模型本地地址
        Debug.Log("ModelUrl====" + SavePath + SaveName);

        http = new HttpDownLoad();
        if (FileHelper.IsFile(SavePath, SaveName)) //路径，物体名+类型
        {
            u += 1;
            http.JudgeExistence(SavePath + SaveName, url, judeg); //判断本地下载是否完整
            if (jut)//完整
            {
                if (modelCode != modelCode_)//版本号不相同
                {
                    FileHelper.DeleteFile(Application.persistentDataPath + "/ModelDate", SaveName); //删除下载完成文件
                    isStart = true;
                    text.text = "开始下载数据，小主请稍后~！";
                    http.DownLoad(url, SavePath, SaveName, LoadLevel); //下载地址，路径，保存物体名+类型，回调函数
                }
                else
                {
                    if (n == u)
                    {
                        text.text = "正在玩命加载,小主请稍后~";
                        Debug.Log("版本号相同====LoadScene...");
                        isDone = true;
                        if (!isBannerDone)
                            DownLoadBannerAndSave(bannerURL);
                        else
                            SceneManager.LoadScene("NewYear"); //加载场景
                    }
                    else
                    {
                        text.text = "正在玩命加载,小主请稍后~~";
                    }
                }
            }
            else
            {
                FileHelper.DeleteFile(Application.persistentDataPath + "/ModelDate", SaveName); //删除未下载完成文件
                isStart = true;
                text.text = "开始下载数据，小主请稍后~！";
                http.DownLoad(url, SavePath, SaveName, LoadLevel); //下载地址，路径，保存物体名+类型，回调函数
            }
        }
        else//不存在
        {
            u += 1;
            //if (modelCode != modelCode_)
            //{
            //    FileHelper.DeleteFile(Application.persistentDataPath + "/ModelDate", SaveName);
            //}
            isStart = true;
            text.text = "开始下载数据，小主请稍后~！";
            http.DownLoad(url, SavePath, SaveName, LoadLevel); //下载地址，路径，保存物体名+类型，回调函数
        }
    }
    void DownloadBannerDone()
    {
        Debug.Log("DownloadBannerDone");
        isBannerDone = true;
    }
    void LoadLevel()
    {
        isDone = true;
        DownLoadBannerAndSave(bannerURL);
    }

    void judeg(bool t)
    {
        jut = t;
    }
}