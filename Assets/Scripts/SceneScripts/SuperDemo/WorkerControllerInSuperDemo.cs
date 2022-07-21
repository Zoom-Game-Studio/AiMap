using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using UnityEngine.Events;

namespace AppLogic
{
    public class WorkerControllerInSuperDemo : MonoBehaviour
    {
        public AudioClip audioGuide;
        public AudioClip audioWorker;
        void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.TALKING_TO_WORKER_IN_SUPERDEMO, HandleTalkingToWorkerInSuperDemo);
        }

        private void HandleTalkingToWorkerInSuperDemo(EventObject obj)
        {
            GuideTalk();
        }

        private void CallBack()
        {

        }
        void GuideTalk()
        {
            AudioSource audio = transform.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.clip = audioGuide;
            audio.loop = false;
            audio.Stop();
            audio.Play();
            StartCoroutine(AudioPlayFinished(audio.clip.length, CallBack));
        }
        void WorkerTalk()
        {
            AudioSource audio = transform.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.clip = audioWorker;
            audio.loop = false;
            audio.Stop();
            audio.Play();
            StartCoroutine(AudioPlayFinished(audio.clip.length, CallBack));
        }
        //ִ��Э�ɺ��� ���ҷ���ʱ��
        private IEnumerator AudioPlayFinished(float time, UnityAction callback)
        {


            yield return new WaitForSeconds(time);
            //����������Ϻ�֮�����µĴ���  

            #region   ����������ɺ�ִ�еĴ���

            print("����������ϣ���������ִ��");
            WorkerTalk();

            #endregion
        }


       
    }
}
