using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
/// <summary>
/// 主场景流程控制
/// </summary>
public class MainScenProcessController : MonoBehaviour,ISingleton
{
    public static MainScenProcessController Instance => MonoSingletonProperty<MainScenProcessController>.Instance;

    [Header("资源滚动列表")]
    public Transform assestScrollView;
    [Header("触摸板")]
    public Transform touchPanel;
    [Header("App按钮")]
    public Button AppBtn;
   
    private void Start()
    {
        AppBtn.onClick.AddListener(onClickShowTouchPanel);
        onClickShowAssestPanel();
    }
    /// <summary>
    /// 展示触摸板
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
    /// 展示资源滚动列表
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
