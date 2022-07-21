using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;
public class SuperDemoShopPanelCtrl : MonoBehaviour
{
    //public GameObject startApperaObj;
    public Renderer render;
    public GameObject finalApperaObj;
    private Animation ani;
    private void Awake()
    {
        AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleOnTriggerSwitch);
    }
    private void Start()
    {
        ani = transform.GetComponentInChildren<Animation>();

    }
    private void HandleOnTriggerSwitch(EventObject ev)
    {
        Debug.Log("HandleSwitchObj...");
        Transform trans = ev.param as Transform;
        if (transform.Equals(trans))
        {
            AddFavouritePlay();
        }
    }

    public void AddFavouritePlay()
    {
        //startApperaObj.SetActive(true);
        if (render != null)
            render.enabled = true;
        if (ani == null) return;
        Debug.Log(ani.transform.name);
        ani.enabled = false;
        ani.enabled = true;
        ani.Play();
    }


    public void AddFavouritePlayComplete()
    {
        if (finalApperaObj != null)
            finalApperaObj.SetActive(true);
        if (ani != null)
            ani.enabled = false;
        if (render != null)
            render.enabled = false;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Transform child = transform.GetChild(i);
        //    if (child == null) continue;
        //    if (child.name.Equals("Blow")) continue;
        //    child.gameObject.SetActive(true);
        //}
        //if (ani == null) return;
        //ani.enabled = false;

        //GameObject blowObj = UnityUtilties.FindChild(transform, "Blow");
        //if (blowObj == null) return;
        //GameObject firework = Instantiate<GameObject>(blowObj);

        //firework.transform.SetParent(transform, true);
        //firework.transform.localPosition = blowObj.transform.localPosition;
        //firework.transform.localRotation = blowObj.transform.localRotation;
        //firework.transform.localScale = blowObj.transform.localScale;
        //firework.SetActive(true);
        //firework.AddComponent<SuperDemoDelayDestroy>();
    }

}