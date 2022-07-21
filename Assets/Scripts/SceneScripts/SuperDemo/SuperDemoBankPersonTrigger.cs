using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.Events;
using System;

namespace AppLogic
{
    public class SuperDemoBankPersonTrigger : MonoBehaviour
    {
        private bool isInit = false;
        private bool isPlayed = false;
        private Camera mainCamera = null;
        public GameObject nextPoint;
        private LookAtCamera lookatCamera;
        private void Awake()
        {
            mainCamera = Camera.main;
        }
        private void Start()
        {
            isInit = true;
            lookatCamera = transform.GetComponent<LookAtCamera>();
        }

        void GuideTrigger()
        {
            if (mainCamera == null) return;
            if (Vector3.Distance(mainCamera.transform.position, transform.position) < 2)
            {
                isPlayed = true;
                PlayWaypointVoice(transform);
                if (lookatCamera != null) lookatCamera.enabled = true;
            }
        }
        void SetAnimatorPara(int para)
        {
            Debug.Log("setpara:" + para);
            Animator ani = GetComponent<Animator>();
            if (ani == null) return;
            ani.SetInteger("Talk", para);
        }
        private void Update()
        {
            //if (!isArrived)
            //MoveToNextPoint();
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
            SetAnimatorPara(1);
            StartCoroutine(FinalAudioGuidePlayFinished(audio.clip.length, callback));
        }
        private void callback()
        {
        }

        private IEnumerator FinalAudioGuidePlayFinished(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            //声音播放完毕后之下往下的代码 
            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");
            isArrived = false;
            SetAnimatorPara(0);
            if (lookatCamera != null) lookatCamera.enabled = false;
            //float distance = Vector3.Distance(gameObject.transform.position, nextPoint.transform.position);
            //finalSpeed = distance / 3.0f;
            StartCoroutine(Arrived());
            #endregion

        }
        float finalSpeed = 0;
        bool isArrived = true;

        void MoveToNextPoint()
        {
            //float step = speed * Time.deltaTime;
            //float distance = Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position);
            //float finalSpeed = distance / 3.0f;
            float step = finalSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextPoint.transform.position, step);
            transform.LookAt(nextPoint.transform.position);

            if (Vector3.Distance(gameObject.transform.position, nextPoint.transform.position) < 0.5)
            {
                StartCoroutine(Arrived());
            }
        }

        IEnumerator Arrived()
        {
            Debug.Log("isArrived...");
            isArrived = true;
            //gameObject.transform.rotation = nextPoint.transform.rotation;
            if (lookatCamera != null) lookatCamera.enabled = true;
            SetAnimatorPara(1);
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SUPERDEMO_SHOW_BANK_MENU);
            yield return null;
            Destroy(this);
        }
    }

}