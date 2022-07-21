using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KleinEngine;

namespace AppLogic
{
    public class Wayz9LPresentationStart : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(PresentationStart);
        }

        private void PresentationStart()
        {
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.WAYZ9L_PRESENTATION_START);
        }
    }
}
