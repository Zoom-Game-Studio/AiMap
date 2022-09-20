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

public class AssesInfoItem : MonoBehaviour
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
    //下载资源相关
    private HttpDownLoad loader;
    private FloatReactiveProperty progress = new FloatReactiveProperty();
    [SerializeField]private Image progressSlider;
    [SerializeField] private Text progressText;
    public Transform downPart;//完整下载显示UI

    
    private void Start()
    {
        isNeedDown = false;
        btn = transform.GetComponentInChildren<Button>();
        downPart.gameObject.SetActive(false);
        btn.onClick.AddListener(OnClickBtn);
        MessageBroker.Default.Receive<DownloadEvent>().Subscribe(OnDownload).AddTo(this);
        progress.Subscribe(v =>
        {
            if (progressSlider != null&&isNeedDown)
            {
                //Debug.LogWarning("场景下载进度" + progress + "||" + v);
                progressSlider.fillAmount = progress;
                progressText.text = (progressSlider.fillAmount * 100).ToString("f0")+"%";
                if (progress >= 1)
                {
                    downPart.gameObject.SetActive(false);
                    SetImageColor(true, null);
                    isNeedDown = false;
                    BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = false;
                    BasicGridAssestScorlPanel.ins.Data[count].isComplete = true;
                    //BasicGridAssestScorlPanel.ins.UpdateUI();

                }
                else
                {
                    downPart.gameObject.SetActive(true);
                    SetImageColor(false, null);
                }

            }
            else
            {
                Debug.Log("UI为空");
            }

        });
    }
  
    private void Update()
    {
        if (loader != null&&isDownLoading&&isNeedDown)
        {
            //Debug.LogWarning("测试" + loader.progress);
            progress.Value = loader.progress;
        }
    }
    [Button]
    void OnClickBtn()
    {
        isDownLoading = true;
        BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = true;
        
        AssetDownloader.Instance.AddToDownloadList(itemInfo);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
       

    }
   
    public void Init(AssetInfoItem evtInfo,bool isDownLoading,int count1)
    {
        //Debug.LogError("进行初始化");
       this.isDownLoading = isDownLoading;
        count = count1;
        //Debug.LogWarning("序号"+count);
        
        itemInfo = evtInfo;
        //info.text = $"{evtInfo.place}：{evtInfo.name}，{evtInfo.description}";
        info.text = evtInfo.name;
       
        PoolController.Instance.FindImg(evtInfo.avatar /*"https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/9lshowcase.jpg?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1718186231&Signature=KRokrFFnw8K5B6sI%2BT6Lrk0xfNQ%3D"*/, (spr)=>{
                                                        
             btn.transform.GetComponent<Image>().sprite = spr;
         });
       
            var local = AssetDownloader.Instance.ClientList.Find(e => e.id.Equals(evtInfo.id));
            if (local != null)
            {
                var needUpdate = evtInfo.updateTime != local.updateTime;
                if (!needUpdate)
                {
                //Debug.LogWarning("这个不需要下载" + evtInfo.name);
                isNeedDown = false;
                SetImageColor(true);
                    
                }
                else
                {
                   // Debug.LogWarning("需要更新本地资源：" + evtInfo.name);
                //TODO 图片变灰色
                isNeedDown = true;
                SetImageColor(false);
                   
                }


            }
            else
            {
                //Debug.LogWarning("这个为空");
               // Debug.LogWarning("需要更新本地资源：" + evtInfo.name);
            //TODO 图片变灰色
            isNeedDown = true;
            SetImageColor(false);
                
            }
        
        

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
