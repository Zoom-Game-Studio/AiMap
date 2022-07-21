using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;


public class Wayz9LVTDanceCtrl : MonoBehaviour
{
    public Animator vtAniCtrl;
    private void Awake()
    {
        AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleOnTriggerSwitch);
    }
    private void Start()
    {
    }
    private void HandleOnTriggerSwitch(EventObject ev)
    {
        Debug.Log("HandleSwitchObj...");
        Transform trans = ev.param as Transform;
        if (transform.Equals(trans))
        {
            if (vtAniCtrl != null)
                vtAniCtrl.SetInteger("isStart", 1);
        }
    }

}