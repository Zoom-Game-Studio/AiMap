using AppLogic;
using KleinEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AppLogic
{
    public class ZhiNengDaoGuideController : MonoBehaviour
    {
        private float speed = 1;
        private bool isArrived = true;

        private Transform currentSender = null;
        private AudioSource currentAudio = null;
        private Transform finalPoint = null;
        private LookAtCamera lookatCamera;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.ZHINENGDAO_MOVE_TO_NEXT, HanleZhiNengDaoMoveToNext);
            lookatCamera = transform.GetComponent<LookAtCamera>();
        }

        private void HanleZhiNengDaoMoveToNext(EventObject ev)
        {
            Transform trans = ev.param as Transform;
            if (trans == null) return;
            Debug.Log("HanleZhiNengDaoMoveToNext...");
            nextWayPoint = trans.gameObject;
            Debug.Log(nextWayPoint.transform.position);
            if (lookatCamera != null) lookatCamera.enabled = false;
            isArrived = false;
            float distance = Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position);
            finalSpeed = distance / 3.0f;
            currentSender = ev.sender as Transform;
        }

        void PlayGameobjectAudio(Transform trans)
        {
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;
            if (currentAudio != null) currentAudio.Stop();
            audio.Stop();
            audio.Play();
            currentAudio = audio;
            Debug.Log("PlayGameobjectAudio");
            ZhiNengDaoButtonCtrl ctrl = trans.GetComponent<ZhiNengDaoButtonCtrl>();
            if (ctrl == null) return;
            if (ctrl.finalPoint == null) return;
            Debug.Log("final...");
            finalPoint = ctrl.finalPoint;
            StartCoroutine(FinalAudioGuidePlayFinished(audio.clip.length, callback));
        }

        private void callback()
        {
        }

        private IEnumerator FinalAudioGuidePlayFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");

            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHINENGDAO_MOVE_TO_NEXT, finalPoint);
            #endregion

        }


        GameObject nextWayPoint;
        void StartMove()
        {
            Debug.Log("StartMove...");
            SetAnimatorPara(0);
            isArrived = false;
            //if (currentWayPoint.Equals(waypointsList.Count))
            //    AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_PLAY_AIMAPGROUP);
        }
        float finalSpeed = 0;
        void MoveToNextPoint()
        {
            //float step = speed * Time.deltaTime;
            //float distance = Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position);
            //float finalSpeed = distance / 3.0f;

            float step = finalSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWayPoint.transform.position, step);
            transform.LookAt(nextWayPoint.transform.position);

            if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
            {
                Debug.Log("isArrived...");
                isArrived = true;
                transform.rotation = nextWayPoint.transform.rotation;
                if (lookatCamera != null) lookatCamera.enabled = true;
                if (currentSender == null) return;
                PlayGameobjectAudio(currentSender);

                //transform.LookAt(Camera.main.transform.position);
            }
        }

        //void MoveToNextPoint()
        //{
        //    float step = speed * Time.deltaTime;
        //    gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nextWayPoint.transform.position, step);
        //    //transform.(nextWayPoint);
        //    transform.LookAt(nextWayPoint.transform.position);
        //    if (currentWayPoint.Equals(6))
        //        AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_SUPERDEMO_CAR);
        //    if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
        //    {
        //        Debug.Log("isArrived...");
        //        isArrived = true;

        //        {
        //            SetAudioClip(currentWayPoint);

        //            currentWayPoint++;
        //            transform.LookAt(Camera.main.transform.position);
        //        }
        //        //if (currentWayPoint >= waypoints.Length) return;

        //        SetAnimatorPara(1);
        //        //StartCoroutine(Move());
        //    }
        //}
        void SetAudioClip(int waypoint)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null) return;
            //audioSource.clip = audios[waypoint];
            audioSource.Play();
        }
        void SetAnimatorPara(int para)
        {
            Debug.Log("SetPara:" + para);
            Animator ani = GetComponent<Animator>();
            if (ani == null) return;
            ani.SetInteger("Talk", para);
        }
        private void Update()
        {
            if (isArrived) return;
            MoveToNextPoint();
        }

        public void NextButtonClicked()
        {
            Debug.Log("NextButtonClickeed...");
            StartMove();
        }

    }
}
