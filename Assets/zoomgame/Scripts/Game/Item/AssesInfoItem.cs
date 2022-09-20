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
    [Header("��ɫ����")]
    public Material grayMat;
    int count = 0;

    bool isNeedDown = false;//�Ƿ���Ҫ����
    bool isDownLoading= false;
    //������Դ���
    private HttpDownLoad loader;
    private FloatReactiveProperty progress = new FloatReactiveProperty();
    [SerializeField]private Image progressSlider;
    [SerializeField] private Text progressText;
    public Transform downPart;//����������ʾUI

    
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
                //Debug.LogWarning("�������ؽ���" + progress + "||" + v);
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
                Debug.Log("UIΪ��");
            }

        });
    }
  
    private void Update()
    {
        if (loader != null&&isDownLoading&&isNeedDown)
        {
            //Debug.LogWarning("����" + loader.progress);
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
        //Debug.LogError("���г�ʼ��");
       this.isDownLoading = isDownLoading;
        count = count1;
        //Debug.LogWarning("���"+count);
        
        itemInfo = evtInfo;
        //info.text = $"{evtInfo.place}��{evtInfo.name}��{evtInfo.description}";
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
                //Debug.LogWarning("�������Ҫ����" + evtInfo.name);
                isNeedDown = false;
                SetImageColor(true);
                    
                }
                else
                {
                   // Debug.LogWarning("��Ҫ���±�����Դ��" + evtInfo.name);
                //TODO ͼƬ���ɫ
                isNeedDown = true;
                SetImageColor(false);
                   
                }


            }
            else
            {
                //Debug.LogWarning("���Ϊ��");
               // Debug.LogWarning("��Ҫ���±�����Դ��" + evtInfo.name);
            //TODO ͼƬ���ɫ
            isNeedDown = true;
            SetImageColor(false);
                
            }
        
        

    }
    public void SetLoader(HttpDownLoad loader, FloatReactiveProperty progress)
    {
        if (loader!=null)
        {
           // Debug.LogWarning("loader��Ϊ��");
           this. loader= BasicGridAssestScorlPanel.ins.Data[count].loader;
        }
        else
        {
           // Debug.LogWarning("loaderΪ��");
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
    /// ���ݽ�������ͼƬ����ɫ
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
