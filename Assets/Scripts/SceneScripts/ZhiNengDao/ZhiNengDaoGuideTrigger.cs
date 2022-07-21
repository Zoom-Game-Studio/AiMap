using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using KleinEngine;

namespace AppLogic

{
    public class ZhiNengDaoGuideTrigger : MonoBehaviour
    {
        private bool isInit = false;
        private bool isPlayed = false;
        private Camera mainCamera = null;
        public float triggerDis = 8;
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            isInit = true;
        }

        void GuideTrigger()
        {
            if (mainCamera == null) return;
            if (Vector3.Distance(mainCamera.transform.position, transform.position) < triggerDis)
            {
                isPlayed = true;
                PlayWaypointVoice(transform);
            }
        }

        private void Update()
        {
            if (!isInit) return;
            if (isPlayed) return;
            GuideTrigger();
        }
        void PlayWaypointVoice(Transform trans)
        {
            Debug.Log("PlayWaypointVoice..." + trans.name);
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.Stop();
            audio.Play();
        }
        public void NextButtonClicked()
        {
            Debug.Log("NextButtonClickeed...");
        }

    }

}