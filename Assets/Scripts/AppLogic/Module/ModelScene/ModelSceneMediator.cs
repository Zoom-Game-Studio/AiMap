using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using LitJson;
using System.IO;
using System.Text.RegularExpressions;

namespace AppLogic
{
    public class ModelSceneMediator : BaseMediator
    {


        public GameObject model;

        private AssetBundle myLoadedAssetBundle;

        private Transform modelParent;

        private float modelScale = 0.8f;

        public ModelSceneMediator()
        {
            m_mediatorName = MEDIATOR.MODEL_SCENE;
        }


        protected override void onInit()
        {
            InitCommand();
            addModuleEvent(ModuleEventType.MODEL_LOAD, HandleModelLoad);
            addModuleEvent(ModuleEventType.MODEL_ENABLE, HandleModelEnable);
            GameObject go = GameObject.Find("ModelParent");
            if (go != null)
            {
                modelParent = go.transform;
            }


            sendModuleEvent(ModuleEventType.MODEL_LOAD);
            sendModuleEvent(ModuleEventType.MODEL_PLAY);


        }

        private void HandleModelEnable(EventObject ev)
        {
            if (ev.param == null) return;
            bool isAvtive = (bool)ev.param;
            Debug.Log("SetModelsVisible");

            if (model != null)
            {
                model.SetActive(isAvtive);
            }
            if (modelParent == null) return;
            for (int i = 0; i < modelParent.transform.childCount; i++)
            {
                modelParent.transform.GetChild(i).gameObject.SetActive(isAvtive);
            }
            Debug.Log("====SetModelVisible");
        }



        private void HandleModelLoad(EventObject ev)
        {
            LoadSceneModel();
        }

        private void InitCommand()
        {
            Debug.Log("InitCommand");
            //registerCommand(ModuleEventType.MODEL_PLAY, typeof(ModelPlayCommand));
        }

        public override void onExit()
        {
            base.onExit();
        }

        /// <summary>
        /// 创建模型 
        /// </summary>
        /// <param name="content"></param>
        public void LoadSceneModel()
        {
            //if (arModelSceneCtrl != null)
            //    arModelSceneCtrl.SetLightType(LightShadows.Soft);
            //转成JSON串
            LoadModelInfo currentModelInfo = null;
            if (currentModelInfo == null) { Debug.Log("currentModelInfo is null"); return; }
            string modelType = currentModelInfo.moduleType;

            if (currentModelInfo.banner != null && !currentModelInfo.banner.Equals(string.Empty))
            {
            }
            if (currentModelInfo.isArCore != null && !currentModelInfo.isArCore.Equals(string.Empty))
            {
                string isTrue = currentModelInfo.isArCore;
                if (isTrue != null && isTrue != string.Empty)
                {
                }
            }
            if (currentModelInfo.isUpload != null && !currentModelInfo.isUpload.Equals(string.Empty))
            {
                string isTrue = currentModelInfo.isUpload; Debug.Log("isUpload====" + isTrue);
                if (isTrue != null && isTrue != string.Empty)
                {
                }
            }
            string folderPath = currentModelInfo.folderPath;
            Debug.Log("folderPath====" + folderPath);

            Debug.Log("load files...");

            if (currentModelInfo.files != null)
            {
                if (currentModelInfo.files.Count > 0)
                {
                    //解析出具体模型路径
                    var modelUrl = currentModelInfo.files[0].path.ToString();
                    Debug.Log("modelUrl==== " + modelUrl);
                    switch (modelType)
                    {
                        case "1"://类型1为8i文件加载
                            Debug.Log("case 1:");
                            Create8iModel(modelUrl, folderPath);
                            break;
                        case "4"://类型4为8i示例文件加载
                            Debug.Log("case 4:");
                            Create8iSample(folderPath);
                            break;
                        case "5"://类型5文件加载
                            Debug.Log("case 5:");
                            CreateNormalModel(modelUrl);
                            break;
                        case "6"://类型6为特效文件加载
                            Debug.Log("case 6:");
                            CreateModelEffect(modelUrl);
                            break;
                        case "7"://类型7为叠镜模型文件加载
                            Debug.Log("case 7:");
                            break;
                    }
                }
                else { Debug.Log("解析错误"); }
            }
        }

        /// <summary>
        /// 创建类型为5的普通模型
        /// </summary>
        /// <param name="modelUrl"></param>
        public void CreateNormalModel(string modelUrl)
        {
            if (myLoadedAssetBundle != null)
                myLoadedAssetBundle.Unload(false);
            //   print("CreateNormalModel");
            //AB 加载模型到内存,合成类型的项目 加载资源，路径不带file://"
            myLoadedAssetBundle = AssetBundle.LoadFromFile(modelUrl);
            //  print("CreateNormalModel.....");
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }
            TextAsset textAsset = myLoadedAssetBundle.LoadAsset<TextAsset>("modelconfig.txt");
            if (textAsset == null) { Debug.Log("modelconfig read filed"); return; }
            JsonData jsonText = JsonMapper.ToObject(textAsset.text);
            if (jsonText == null) { Debug.Log("jsonText is null"); return; }
            Debug.Log("jsonText===" + jsonText.ToString());
            string modelConfigName = jsonText["name"].ToString();
            if (modelConfigName == null) { Debug.Log("modelConfigName is null"); return; }
            Debug.Log("modelConfigName===" + modelConfigName);
            //拆分模型实例名,“/”最后一位就是实例名
            var splitArray = Regex.Split(modelUrl, "/", RegexOptions.IgnoreCase);
            string urlName = splitArray[splitArray.Length - 1];
            Debug.Log("urlName====" + urlName);
            string abName = urlName.Split('.')[0];
            Debug.Log("abName====");
            //从内存中加载赋值模型对象
            GameObject newModel;
            if (modelConfigName == null) { Debug.Log("modelconfigName is null:"); return; }
            newModel = myLoadedAssetBundle.LoadAsset<GameObject>(modelConfigName);
            //赋值到新的模型对象，用以控制尺寸
            model = UnityEngine.GameObject.Instantiate(newModel);
            //model.SetActive(false);//关闭模型的显示++++
            if (modelParent != null)
                model.transform.SetParent(modelParent.transform, false);
            //赋值尺寸
            model.transform.localPosition = model.transform.localPosition + new Vector3(0, -0.5f, 1.5f);
            model.transform.localScale = new Vector3(modelScale, modelScale, modelScale);
            model.transform.localEulerAngles = new Vector3(0, 180, 0);
            if (model.GetComponent<FingerController>() == null)
                model.AddComponent<FingerController>();
            //赋值动画控制器
            Resources.UnloadUnusedAssets();
            //todo
            //StartCoroutine(DelayShowModel());
            //todo
        }
        /// <summary>
        /// 创建8i模型
        /// </summary>
        public void Create8iModel(string modelUrl, string folderPath)
        {
            //if (arModelSceneCtrl != null)
            //    arModelSceneCtrl.SetLightType(LightShadows.None);
            if (folderPath == null) { Debug.Log("folderPath is null"); return; }
            string fullPath = folderPath + "/";
            string filePath = null;
            string audioPath = null;
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".8i"))
                    {
                        filePath = folderPath + "/" + files[i].Name;
                        Debug.Log("filePath====" + filePath);
                    }
                    if (files[i].Name.EndsWith(".wav"))
                    {
                        audioPath = folderPath + "/" + files[i].Name;
                        Debug.Log("audioPath====" + audioPath);
                    }
                    Debug.Log("Name:" + files[i].Name);
                }
            }
            if (filePath == null) { Debug.Log("filePath is null"); return; }
            if (audioPath == null) { Debug.Log("audioPath is null"); }
            else
            {
                if (File.Exists(audioPath))
                {
                    string newFilePath = Application.persistentDataPath + "/AudioSource.wav";
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(audioPath, newFilePath);
                    if (File.Exists(newFilePath))
                    {
                        AudioSource modelAudio = model.GetComponent<AudioSource>();
                        if (modelAudio != null) return;
                        //todo
                        // StartCoroutine(UnityUtilties.GetAudioClip(modelAudio, newFilePath, AudioType.WAV));
                        //todo
                    }
                }
                Debug.Log("hvrSetData===...");
                //StartCoroutine(Set8iModelVisible());
                //model.SetActive(false);//关闭模型的显示++++
                if (modelParent != null)
                {
                    model.transform.SetParent(modelParent.transform, true);
                }
                //todo
                // StartCoroutine(DelayShowModel());
                //todo
            }
            //AREventUtil.DispatchEvent(GlobalOjbects.SET_LOOPBTN);

        }
        /// <summary>
        /// 创建8i示例模型
        /// </summary>
        public void Create8iSample(string folderPath)
        {
            //todo
            //if (arModelSceneCtrl != null)
            //    arModelSceneCtrl.SetLightType(LightShadows.None);
            //todo
            if (folderPath == null) { Debug.Log("folderPath is null"); return; }
            string fullPath = folderPath + "/";
            string filePath = null;
            string audioPath = null;
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".hvrs"))
                    {
                        filePath = folderPath + "/" + files[i].Name;
                        Debug.Log("filePath====" + filePath);
                    }
                    if (files[i].Name.EndsWith(".mp3"))
                    {
                        audioPath = folderPath + "/" + files[i].Name;
                        Debug.Log("audioPath====" + audioPath);
                    }
                    Debug.Log("Name:" + files[i].Name);
                }
            }
            if (filePath == null) { Debug.Log("filePath is null"); return; }
            if (audioPath == null) { Debug.Log("audioPath is null"); }
            else
            {
                if (File.Exists(audioPath))
                {
                    string newFilePath = Application.persistentDataPath + "/AudioSource.mp3";
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    File.Copy(audioPath, newFilePath);
                    if (File.Exists(newFilePath))
                    //StartCoroutine(GetAudioClip(newFilePath, AudioType.MPEG));
                    {
                        AudioSource modelAudio = model.GetComponent<AudioSource>();
                        if (modelAudio != null) return;
                        //todo
                        // StartCoroutine(UnityUtilties.GetAudioClip(modelAudio, newFilePath, AudioType.MPEG));
                        //todo
                    }
                }
            }
            Debug.Log("hvrSetData===...");
            if (modelParent != null)
            {
                model.transform.SetParent(modelParent.transform, true);
            }
            //todo
            //StartCoroutine(DelayShowModel());
            //todo
            //}
        }

        /// <summary>
        /// 创建特效模型 
        /// </summary>
        /// <param name="modelUrl"></param>
        public void CreateModelEffect(string modelUrl)
        {
            if (myLoadedAssetBundle != null)
                myLoadedAssetBundle.Unload(false);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(modelUrl);
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("myLoadedAssetBundle is null");
                return;
            }
            TextAsset textAsset = myLoadedAssetBundle.LoadAsset<TextAsset>("modelconfig.txt");
            if (textAsset == null) { Debug.Log("modelconfig read filed"); return; }
            JsonData jsonText = JsonMapper.ToObject(textAsset.text);
            if (jsonText == null) { Debug.Log("jsonText is null"); return; }
            Debug.Log("jsonText===" + jsonText.ToString());
            string modelConfigName = jsonText["name"].ToString();
            if (modelConfigName == null) { Debug.Log("modelConfigName is null"); return; }
            Debug.Log("modelConfigName===" + modelConfigName);

            var splitArray = Regex.Split(modelUrl, "/", RegexOptions.IgnoreCase);
            string urlName = splitArray[splitArray.Length - 1];
            Debug.Log("urlName====" + urlName);
            string abName = urlName.Split('.')[0];

            Debug.Log("abName===" + abName);
            GameObject newModel;
            if (modelConfigName == null)
            {
                Debug.Log("modelConfigName is null");
                return;
            }
            newModel = myLoadedAssetBundle.LoadAsset<GameObject>(modelConfigName);
            //todo
            // model = Instantiate(newModel);
            //todo
            //model.SetActive(false);//关闭模型的显示++++
            //赋值到新的模型对象，用以控制尺寸
            if (modelParent == null)
            {
                Debug.Log("modelEffectParent is null====");
                return;
            }

            if (model == null)
            {
                Debug.Log("Instantiate failed====");
                return;
            }
            model.transform.SetParent(modelParent.transform, false);
            //赋值尺寸
            model.transform.localScale = new Vector3(1, 1, 1);
            model.transform.localEulerAngles = new Vector3(0, 0, 0);
            Collider[] colliders = model.GetComponentsInChildren<Collider>();
            Debug.Log("Collider count is ===" + colliders.Length);
            if (colliders != null)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.GetComponent<FingerController>() == null)
                        collider.gameObject.AddComponent<FingerController>();
                }
            }

            Resources.UnloadUnusedAssets();
        }


        /// <summary>
        /// 延时模型显示
        /// </summary>
        /// <returns>The model visible.</returns>
        public IEnumerator DelayShowModel()
        {
            {
                Debug.Log("当前场景为识别场景===");
                {
                    Transform[] father_ = model.GetComponentsInChildren<Transform>();
                    foreach (var child_ in father_)
                    {
                        child_.gameObject.layer = 0;//layer=8模型不显示
                    }
                    yield return new WaitForSecondsRealtime(2);
                    model.SetActive(true);
                    //todo
                    //ProgressBar.AnimatorController();//开始进度条
                    //todo
                }
                {
                    Transform[] father = model.GetComponentsInChildren<Transform>();
                    foreach (var child in father)
                    {
                        child.gameObject.layer = 8;//layer=8模型不显示
                    }
                    model.SetActive(false);
                }
                {
                    model.layer = 8;
                    Debug.Log("model name" + model.name);
                    //todo
                    //ProgressBar = GameObject.Find("TwinSlider").GetComponent<ProgressBar>();
                    //ProgressBar.AnimatorController();//开始进度条
                    //todo
                    yield return new WaitForSecondsRealtime(2);

                    model.gameObject.layer = 0;
                    model.SetActive(true);
                }
            }
        }
    }
}