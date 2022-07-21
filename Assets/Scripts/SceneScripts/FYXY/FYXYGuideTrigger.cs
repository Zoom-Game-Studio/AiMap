using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;

namespace AppLogic
{
    public class FYXYGuideTrigger : MonoBehaviour
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
                //isPlayed = true;
                //PlayWaypointVoice(transform);
                if (IsGameObjectInCameraView(gameObject, mainCamera))
                    AppFacade.GetInstance().dispatchEvent(ModuleEventType.FYXY_MOVE_TO_NEXT, transform);
            }
        }

        public static bool IsGameObjectInCameraView(GameObject targetObj, Camera camera = null)
        {
            if (camera == null)
                camera = Camera.main;

            if (camera == null)
                return false;

            Vector3 targetObjViewportCoord = camera.WorldToViewportPoint(targetObj.transform.position);
            if (targetObjViewportCoord.x > 0 && targetObjViewportCoord.x < 1 && targetObjViewportCoord.y > 0f && targetObjViewportCoord.y < 1 && targetObjViewportCoord.z > camera.nearClipPlane && targetObjViewportCoord.z < camera.farClipPlane)
                return true;

            return false;
        }


        private void Update()
        {
            if (!isInit) return;
            //if (isPlayed) return;
            GuideTrigger();
        }
        void PlayWaypointVoice(Transform trans)
        {
            Debug.Log("PlayWaypointVoice..." + trans.name);
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.Stop();
            audio.Play();
            StartCoroutine(WayPointVoicePlayFinished(audio.clip.length));
        }

        IEnumerator WayPointVoicePlayFinished(float time)
        {
            yield return new WaitForSeconds(time);
            isPlayed = false;
        }
    }
}
