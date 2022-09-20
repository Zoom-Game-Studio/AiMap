using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zoomgame;
/// <summary>
/// ���������
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
        Debug.LogWarning("������Դ"+name);
        image.sprite = sprite;
        nameTxt.text = name;
    }
}
