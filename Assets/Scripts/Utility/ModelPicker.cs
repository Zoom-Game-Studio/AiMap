using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;
namespace AppLogic
{
    public class ModelPicker : MonoBehaviour
    {
        void MousePick()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.MODEL_PICKER, hit.transform.gameObject);
                }
            }
        }
        void MobilePick()
        {
            if (Input.touchCount != 1)
                return;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.MODEL_PICKER, hit.transform.gameObject);
                }
            }
        }

        void Update()

        {
#if UNITY_EDITOR
            MousePick();
#else
            MobilePick();
#endif
        }
    }
}
