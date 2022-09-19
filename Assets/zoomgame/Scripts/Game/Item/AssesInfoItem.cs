using System;

using Architecture;

using Sirenix.OdinInspector;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

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
    Texture texture = null;
    int count = 0;

    bool isNeedDown = false;//�Ƿ���Ҫ����
    bool isClickDown = false;
    //������Դ���
    private HttpDownLoad loader;
    private FloatReactiveProperty progress = new FloatReactiveProperty();
    [SerializeField]private Image progressSlider;
    [SerializeField] private Text progressText;
    public Transform downPart;//����������ʾUI

    private void Start()
    {
        btn = transform.GetComponentInChildren<Button>();
        downPart.gameObject.SetActive(false);
        btn.onClick.AddListener(OnClickBtn);
        MessageBroker.Default.Receive<DownloadEvent>().Subscribe(OnDownload).AddTo(this);
        progress.Subscribe(v =>
        {
            if (progressSlider != null)
            {
                Debug.LogWarning("�������ؽ���" + progress + "||" + v);
                progressSlider.fillAmount = progress;
                progressText.text = (progressSlider.fillAmount * 100).ToString("f0")+"%";
                if (progress >= 1)
                {
                    downPart.gameObject.SetActive(false);
                    SetImageColor(true,null);
                    
                    isNeedDown = false;
                    //this.gameObject.SetActive(false);
                    //this.gameObject.SetActive(true);
                    BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = false;
                    BasicGridAssestScorlPanel.ins.Data[count].isComplete = true;
                   // BasicGridAssestScorlPanel.ins.UpdateUI();
                   
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
            if (loader != null)
            {
                progress.Value = loader.progress;
            }
    }
    [Button]
    void OnClickBtn()
    {
        if (BasicGridAssestScorlPanel.ins.Data[count].isComplete)
        {
            downPart.gameObject.SetActive(false);
            SetImageColor(true, null);
           
        }
        else
        {
            downPart.gameObject.SetActive(true);
            SetImageColor(false, null);
            
        }
       
        BasicGridAssestScorlPanel.ins.Data[count].isDownLoadIng = true;
        
        isClickDown = true;
        AssetDownloader.Instance.AddToDownloadList(itemInfo);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
       

    }
   
    public void Init(AssetInfoItem evtInfo,bool isDownLoading,int count1)
    {
        count = count1;
        downPart.gameObject.SetActive(isDownLoading);
       
        itemInfo = evtInfo;
        //info.text = $"{evtInfo.place}��{evtInfo.name}��{evtInfo.description}";
        info.text = evtInfo.name;
        if (progressSlider.fillAmount>=1)
        {
            downPart.gameObject.SetActive(false);
            SetImageColor(true, null);
        }

        PoolController.Instance.FindImg(evtInfo.avatar,(spr)=>{
             //sprite = spr;
             btn.transform.GetComponent<Image>().sprite = spr;
         },(text)=> {
             Material material = grayMat.Copy();
             //material.mainTexture = text;

             //
             var local = AssetDownloader.Instance.ClientList.Find(e => e.id.Equals(evtInfo.id));
             if (local != null)
             {
                 var needUpdate = evtInfo.updateTime != local.updateTime;
                 if (!needUpdate)
                 {
                     Debug.LogWarning("�������Ҫ����" + evtInfo.id);
                     SetImageColor(true, material);
                 }
                 else
                 {
                     Debug.LogWarning("��Ҫ���±�����Դ��" + evtInfo.id);
                     //TODO ͼƬ���ɫ



                     SetImageColor(false, material);
                     isNeedDown = true;
                 }


             }
             else
             {
                 Debug.LogWarning("���Ϊ��");
                 Debug.LogWarning("��Ҫ���±�����Դ��" + evtInfo.id);
                 //TODO ͼƬ���ɫ
                 SetImageColor(false,material);
                 isNeedDown = true;
             }
         });

       
       
    }
    public void SetLoader(HttpDownLoad loader, FloatReactiveProperty progress)
    {
        if (loader!=null)
        {
           this. loader= BasicGridAssestScorlPanel.ins.Data[count].loader;
        }
        else
        {
            
            BasicGridAssestScorlPanel.ins.Data[count].progress = progress;
        }
        if (progress != null)
        {
            this.progress = BasicGridAssestScorlPanel.ins.Data[count].progress;
        }
        else
        {
           
            BasicGridAssestScorlPanel.ins.Data[count].progress = progress;
        }
    }
    /// <summary>
    /// ���ݽ�������ͼƬ����ɫ
    /// </summary>
    void SetImageColor(bool isComplete,Material material)
    {
        //btn.transform.GetComponent<Image>().color = Color.white;
        //return;
        if (isComplete)
        {
            //btn.transform.GetComponent<Image>().material = null;
            btn.transform.GetComponent<Image>().color = Color.white;
        }
        else
        {

            //btn.transform.GetComponent<Image>().material = grayMat;
            btn.transform.GetComponent<Image>().color = new Color(0.299f, 0.587f, 0.114f,0.5f);

        }
        
    }
    void OnDownload(DownloadEvent evt)
    {
        this.loader = evt.resLoader;
    }
    
}
