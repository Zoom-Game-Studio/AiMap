using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
/// <summary>
/// ���������̿���
/// </summary>
public class MainScenProcessController : MonoBehaviour,ISingleton
{
    public static MainScenProcessController Instance => MonoSingletonProperty<MainScenProcessController>.Instance;

    [Header("��Դ�����б�")]
    public Transform assestScrollView;
    [Header("������")]
    public Transform touchPanel;
    [Header("App��ť")]
    public Button AppBtn;
   
    private void Start()
    {
        AppBtn.onClick.AddListener(onClickShowTouchPanel);
        onClickShowAssestPanel();
    }
    /// <summary>
    /// չʾ������
    /// </summary>
    public void onClickShowTouchPanel()
    {
        if (assestScrollView.gameObject.activeInHierarchy)
        {
            assestScrollView.gameObject.SetActive(false);
        }
        if (!touchPanel.gameObject.activeInHierarchy)
        {
            touchPanel.gameObject.SetActive(true);
        }
        
       
    }
    /// <summary>
    /// չʾ��Դ�����б�
    /// </summary>
    public void onClickShowAssestPanel()
    {
        if (!assestScrollView.gameObject.activeInHierarchy)
        {
            assestScrollView.gameObject.SetActive(true);
        }
        if (touchPanel.gameObject.activeInHierarchy)
        {
            touchPanel.gameObject.SetActive(false);
        }
    }
    public Transform GetTouchPanel()
    {
        return touchPanel;
    }
    public void OnSingletonInit()
    {
       
    }
}
