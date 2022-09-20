using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zoomgame;
/// <summary>
/// 触摸板界面
/// </summary>
public class UIBaseControllerPanel : MonoBehaviour
{

    public Text nameTxt;
    public Image image;

    public Button AppBtn;
   
    private void Start()
    {
        AppBtn.onClick.AddListener(()=> {
            MainScenProcessController.Instance.onClickShowAssestPanel();
            BasicGridAssestScorlPanel.ins.UpdateView();
        });
    }
    public void SetAssest(string name,Sprite sprite)
    {
        Debug.LogWarning("设置资源"+name);
        image.sprite = sprite;
        nameTxt.text = name;
    }
}
