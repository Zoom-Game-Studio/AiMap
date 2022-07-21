using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;


namespace AppLogic
{
    public class SuperDemoGuideTrigger : MonoBehaviour
    {
        public AudioClip[] audios;

        private bool isInit = false;
        private bool isPlayed = false;
        public List<GameObject> waypointsList = new List<GameObject>();

        private Camera mainCamera = null;

        private void Awake()
        {

            //Debug.Log("GuideController Start...");

            mainCamera = Camera.main;

        }

        private void Start()
        {
            isInit = true;
        }

        void StartTrigger()
        {

        }

        void GuideTrigger()
        {
            if (mainCamera == null) return;
            if (Vector3.Distance(mainCamera.transform.position, transform.position) <= 8)
            {
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.SUPERDEMO_GUIDE_TRIGGER, this.transform);
            }
        }

        private void Update()
        {
            if (!isInit) return;
            if (isPlayed) return;
            GuideTrigger();
        }

        public void NextButtonClicked()
        {
            Debug.Log("NextButtonClickeed...");
        }
    }
}