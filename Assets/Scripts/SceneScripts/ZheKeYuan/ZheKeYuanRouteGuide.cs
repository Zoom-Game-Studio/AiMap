using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZheKeYuanRouteGuide : MonoBehaviour
{
    //bool isActive = false;
    public GameObject routeObj;

    private bool status = false;

    public bool isHideAll = false;

    public GameObject[] allObj;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        if (routeObj == null) return;
    }

    private void OnButtonClick()
    {
        //isActive = !isActive;
        status = routeObj.activeInHierarchy;
        if (isHideAll) HideAll();
        if (routeObj != null)
        {
            status = !status;
            routeObj.SetActive(status);

        }

    }

    void HideAll()
    {
        foreach (var item in allObj)
        {
            item.SetActive(false);
        }
    }

}