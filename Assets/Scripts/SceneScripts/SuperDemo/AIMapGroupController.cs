using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class AIMapGroupController : MonoBehaviour
    {
        public Animator[] anis;
        public AudioSource audio;
        void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.START_PLAY_AIMAPGROUP, HandleStartPlayAIMapGroup);

        }

        private void HandleStartPlayAIMapGroup(EventObject obj)
        {
            Debug.Log("HandleStartPlayAIMapGroup...");
            audio.enabled = true;
            foreach (var ani in anis)
            {
                ani.enabled = true;
            }
        }
    }
}