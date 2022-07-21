using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    [RequireComponent(typeof(BoxCollider))]
    public class SuperDemoOnTriggerSwitch1 : MonoBehaviour
    {
        public GameObject[] allObjs;
        private int index = 0;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleOnTriggerSwitch);
        }
        private void Start()
        {

        }
        private void HandleOnTriggerSwitch(EventObject ev)
        {
            if (allObjs.Length < 2) return;
            Debug.Log("HandleSwitchObj...");
            Transform trans = ev.param as Transform;
            if (transform.Equals(trans))
            {
                OnButtonClick();
            }
        }

        void OnButtonClick()
        {
            index++;
            if (index >= allObjs.Length)
                index = 0;
            for (int i = 0; i < allObjs.Length; i++)
            {
                if (index.Equals(i))
                {
                    allObjs[index].SetActive(true);
                    continue;
                }
                allObjs[i].SetActive(false);
            }
        }
    }
}
