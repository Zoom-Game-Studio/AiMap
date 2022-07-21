using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(MovingUpAndDown))]
    public class SuperDemoOnTriggerSwitch : MonoBehaviour
    {
        bool isMoving = false;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleOnTriggerSwitch);
        }
        private void Start()
        {
            GetComponent<MovingUpAndDown>().enabled = isMoving;
        }
        private void HandleOnTriggerSwitch(EventObject ev)
        {
            Debug.Log("HandleSwitchObj...");
            Transform trans = ev.param as Transform;
            if (transform.Equals(trans))
            {
                isMoving = !isMoving;
                GetComponent<MovingUpAndDown>().enabled = isMoving;
            }
        }
    }
}
