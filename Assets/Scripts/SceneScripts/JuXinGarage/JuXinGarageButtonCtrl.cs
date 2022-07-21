using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JuXinGarageButtonCtrl : MonoBehaviour
{
    private int clickTimes = 0;
    public void CarOnClick()
    {
        clickTimes++;
        ShowChild();
        if (clickTimes.Equals(2))
        {
            gameObject.BroadcastMessage("StartGarageCar");
        }
    }
    public void ShowChild()
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        print(button.name);
        button.transform.GetChild(0).gameObject.SetActive(true);
    }
}