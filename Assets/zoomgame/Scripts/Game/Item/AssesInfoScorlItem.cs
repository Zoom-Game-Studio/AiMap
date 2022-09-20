using System;

using Architecture;

using Sirenix.OdinInspector;

using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Waku.Module;

using zoomgame;
using zoomgame.Scripts.Architecture.TypeEvent;

public class AssesInfoScorlItem : MonoBehaviour
{
    public Text info;
    public Button btn;
    private AssetInfoItem itemInfo;
    public AudioClip clip;
    [Header("灰色材质")]
    public Material grayMat;
    int count = 0;

    bool isNeedDown = false;//是否需要下载
    bool isDownLoading= false;
    bool isInit = false;//是否初始化
    //下载资源相关
    private HttpDownLoad loader =new HttpDownLoad();
    private FloatReactiveProperty progress = new FloatReactiveProperty();
    [SerializeField]private Image progressSlider;
    [SerializeField] private Text progressText;
    public Transform downPart;//完整下载显示UI

    public Sprite spriteTo;
    public string nameTo;
    private void Start()
    {
        isNeedDown = false;
        isDownLoading = false;
        isInit = false;
        //btn = transform.GetComponentInChildren<Button>();
        downPart.gameObject.SetActive(false);
        btn.onClick.AddListener(OnClickBtn);
        MessageBroker.Default.Receive<DownloadEvent>().Subscribe(OnDownload).AddTo(this);
        //progress.Subscribe(v =>
        //{
        //    if (progressSlider != null&&isNeedDown)
        //    {
        //        Debug.LogWarning("场景下载进度" + progress + "||" + v);
        //        progressSlider.fillAmount = progress;
        //        progressText.text = (progressSlider.fillAmount * 100).ToString("f0")+"%";
        //        if (progress >= 1)
        //        {
        //            downPart.gameObject.SetActive(false);
        //            SetImageColor(true, null);
        //            isNeedDown = false;
        //            BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = false;
        //            BasicGridAssestScorlPanel.ins.Data[count].isComplete = true;
        //            //BasicGridAssestScorlPanel.ins.UpdateUI();

        //        }
        //        else
        //        {
        //            downPart.gameObject.SetActive(true);
        //            SetImageColor(false, null);
        //        }

        //    }
        //    else
        //    {
        //        Debug.Log("UI为空");
        //    }

        //});
    }
  
    private void Update()
    {
        //if (!isInit)
        //{
        //    return;
        //}
        //if (BasicGridAssestScorlPanel.ins.Data[count].loader != null && BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng && BasicGridAssestScorlPanel.ins.Data[count].isNeedDown)
        //{
        //    Debug.LogWarning("测试1" + loader.progress);
        //    progress.Value = loader.progress;
          
        //}
        //else
        //{
        //    Debug.Log("测试2");
        //    if (loader == null)
        //    {
        //        Debug.Log("测试3");
        //    }
        //    if (isDownLoading)
        //    {
        //        Debug.Log("测试4");
        //    }
        //    else
        //    {
        //        Debug.Log("测试44");
        //    }
        //    if (isNeedDown)
        //    {
        //        Debug.Log("测试5");
        //    }
        //    else
        //    {
        //        Debug.Log("测试55");
        //    }
        //    Debug.Log("测试2"+ isDownLoading.ToString() + "||" + isNeedDown.ToString() + "|"+ loader == null);
        //}
    }
    [Button]
    void OnClickBtn()
    {
        Debug.LogWarning("点击下载"+ BasicGridAssestScorlPanel.ins.Data[count].isComplete);
        //Debug.LogWarning("点击了"+isDownLoading+"||"+isNeedDown+"|"+loader==null);
        isDownLoading = true;
        BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = true;
        
        AssetDownloader.Instance.AddToDownloadList(itemInfo);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        if (BasicGridAssestScorlPanel.ins.Data[count].isComplete)
        {
           
            MainScenProcessController.Instance.GetTouchPanel().transform.GetComponent<UIBaseControllerPanel>().SetAssest(nameTo, btn.transform.GetComponent<Image>().sprite);
            MainScenProcessController.Instance.onClickShowTouchPanel();
            return;
        }
        StartCoroutine(test());

    }
   IEnumerator test()
    {
        while (!BasicGridAssestScorlPanel.ins.Data[count].isComplete)
        {
            Debug.LogError("dokjakfjofjoafhuoahf");
            if (progressSlider != null && BasicGridAssestScorlPanel.ins.Data[count].isNeedDown)
            {
                Debug.LogWarning("测试" + loader.progress);
                progress.Value = loader.progress;
                progressSlider.fillAmount = progress.Value;
                progressText.text = (progressSlider.fillAmount * 100).ToString("f0") + "%";
                if (progress.Value >= 1)
                {
                    downPart.gameObject.SetActive(false);
                    SetImageColor(true, null);
                    isNeedDown = false;
                    BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = false;
                    BasicGridAssestScorlPanel.ins.Data[count].isComplete = true;

                }
                else
                {
                    downPart.gameObject.SetActive(true);
                    SetImageColor(false, null);
                }

            }
            if (progress.Value>=1)
            {
                
                MainScenProcessController.Instance.GetTouchPanel().transform.GetComponent<UIBaseControllerPanel>().SetAssest(nameTo, spriteTo);
                MainScenProcessController.Instance.onClickShowTouchPanel();
                
            }
            yield return null;
        }
       
        
    }
    public void Init(AssetInfoItem evtInfo,bool isDownLoading,int count1)
    {
       
        //Debug.LogError("进行初始化");
       this.isDownLoading = isDownLoading;
        count = count1;
        Debug.LogWarning("序号"+count);
        
        itemInfo = evtInfo;
        //info.text = $"{evtInfo.place}：{evtInfo.name}，{evtInfo.description}";
        info.text = evtInfo.name;
        nameTo = evtInfo.name;

        PoolController.Instance.FindImg(evtInfo.avatar , (spr)=>{
                                                        
             btn.transform.GetComponent<Image>().sprite = spr;
            spriteTo = spr;
         });
       
            var local = AssetDownloader.Instance.ClientList.Find(e => e.id.Equals(evtInfo.id));
            if (local != null)
            {
                var needUpdate = evtInfo.updateTime != local.updateTime;
                if (!needUpdate)
                {
                Debug.LogWarning("这个不需要下载" + evtInfo.name);
                isNeedDown = false;
                SetImageColor(true);
                    
                }
                else
                {
                    Debug.LogWarning("需要更新本地资源：" + evtInfo.name);
                //TODO 图片变灰色
                isNeedDown = true;
                Debug.LogWarning("需要更新本地资源：" +this. isNeedDown);

               SetImageColor(false);
                   
                }


            }
            else
            {
                //Debug.LogWarning("这个为空");
                Debug.LogWarning("需要更新本地资源：" + evtInfo.name);
            //TODO 图片变灰色
            isNeedDown = true;
            SetImageColor(false);
                
            }
        BasicGridAssestScorlPanel.ins.Data[count].isNeedDown = isNeedDown;
        BasicGridAssestScorlPanel.ins.Data[count].isComplete = !isNeedDown;
        BasicGridAssestScorlPanel.ins.Data[count].nameTitle = evtInfo.name;
        isInit = true;

    }
    public void SetLoader(HttpDownLoad loader, FloatReactiveProperty progress)
    {
        if (loader!=null)
        {
            // Debug.LogWarning("loader不为空");
            
           this. loader= BasicGridAssestScorlPanel.ins.Data[count].loader;
        }
        else
        {
           // Debug.LogWarning("loader为空");
            BasicGridAssestScorlPanel.ins.Data[count].progress = this.progress;
        }
        if (progress != null)
        {
            this.progress = BasicGridAssestScorlPanel.ins.Data[count].progress;
        }
        else
        {
           
            BasicGridAssestScorlPanel.ins.Data[count].progress = this.progress;
        }
       // StartCoroutine(IESetProcess());
        
    }
    /// <summary>
    /// 根据进度设置图片的颜色
    /// </summary>
    void SetImageColor(bool isComplete,Material material=null)
    {
      
        if (isNeedDown)
        {
            if (isComplete)
            {
                //btn.transform.GetComponent<Image>().material = null;
                btn.transform.GetComponent<Image>().color = Color.white;
            }
            else
            {

                //btn.transform.GetComponent<Image>().material = grayMat;
                btn.transform.GetComponent<Image>().color = new Color(0.299f, 0.587f, 0.114f, 0.5f);

            }
        }
        else
        {
            btn.transform.GetComponent<Image>().color = Color.white;
        }
       
        
    }
   
    void OnDownload(DownloadEvent evt)
    {
        this.loader = evt.resLoader;
    }
    
}
