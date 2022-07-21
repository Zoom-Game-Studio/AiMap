using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class ZheKeYuanRouteTrigger : MonoBehaviour
    {

        private Camera mainCamera = null;
        public int groupIndex;
        public GameObject[] triggerGroup1;
        public GameObject[] triggerGroup2;
        private static bool isIndoor = false;
        private void Awake()
        {
            mainCamera = Camera.main;
            AppFacade.GetInstance().addEvent(ModuleEventType.ZHEKEYUAN_ROUTE_TRIGGER, HandleRouteTrigger);
        }

        private void Start()
        {
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHEKEYUAN_ROUTE_TRIGGER);
        }

        private void HandleRouteTrigger(EventObject ev)
        {
            if (isIndoor)
            {
                SetAll(true, triggerGroup1);
                SetAll(false, triggerGroup2);
            }
            else
            {
                SetAll(false, triggerGroup1);
                SetAll(true, triggerGroup2);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.transform.Equals(mainCamera.transform))
            {
                if (groupIndex.Equals(1))
                {
                    isIndoor = true;
                }
                else
                {
                    isIndoor = false;
                }
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHEKEYUAN_ROUTE_TRIGGER);

            }
        }

        void HideAll(GameObject[] go)
        {
            foreach (var item in go)
            {
                item.SetActive(false);
            }
        }

        void SetAll(bool isAvtive, GameObject[] goes)
        {
            if (goes == null) return;
            foreach (var item in goes)
            {
                item.SetActive(isAvtive);
            }
        }
    }
}