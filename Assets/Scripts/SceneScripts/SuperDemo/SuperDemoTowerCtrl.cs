using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;

public class SuperDemoTowerCtrl : MonoBehaviour
{

    public Transform startPosObj;
    public Transform endPosObj;

    public float startScale;
    public float endScale;

    private Vector3 startPos;
    private Vector3 endPos;
    private void Awake()
    {
        if (startPosObj == null || endPosObj == null) return;
        startPos = startPosObj.position;
        endPos = endPosObj.position;
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
            if (transform.position.Equals(startPos))
            {
                transform.position = endPos;
                transform.localScale = Vector3.one * endScale;
            }
            else
            {
                transform.position = startPos;
                transform.localScale = Vector3.one * startScale;
            }
        }
    }
}
