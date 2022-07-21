using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class FYXYGuideController : MonoBehaviour
    {
        [HideInInspector]
        public AudioSource currentAudio = null;
        public AudioClip[] audios;

        private float speed = 1;
        private bool isArrived = true;
        private int currentWayPoint = 0;
        GameObject nextWayPoint;
        private List<GameObject> waypointsList = new List<GameObject>();
        public GameObject startEffect;
        public GameObject guidePeople;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.FYXY_MOVE_TO_NEXT, HandleFYXYMoveToNext);
            AppFacade.GetInstance().addEvent(ModuleEventType.FYXY_STOP_AUDIO, HandleFYXYStopAudio);

        }

        private void HandleFYXYStopAudio(EventObject ev)
        {
            if (currentAudio == null) return;
            currentAudio.Stop();
        }

        private void HandleFYXYMoveToNext(EventObject ev)
        {
            Transform trans = ev.param as Transform;
            if (trans == null) return;
            transform.position = trans.position;
            PlayWaypointVoice(trans);
        }

        IEnumerator ShowEffect()
        {
            //guidePeople.SetActive(false);
            startEffect.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            startEffect.SetActive(false);
            //guidePeople.SetActive(true);
        }

        void MoveToNextPoint()
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nextWayPoint.transform.position, step);
            //transform.(nextWayPoint);
            transform.LookAt(nextWayPoint.transform.position);
            if (currentWayPoint.Equals(6))
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_SUPERDEMO_CAR);
            if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
            {
                Debug.Log("isArrived...");
                isArrived = true;

                if (currentWayPoint < waypointsList.Count)
                {
                    SetAudioClip(currentWayPoint);

                    currentWayPoint++;
                    transform.LookAt(Camera.main.transform.position);
                }
                //if (currentWayPoint >= waypoints.Length) return;

                SetAnimatorPara(1);
                //StartCoroutine(Move());
            }
        }
        void SetAudioClip(int waypoint)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null) return;
            audioSource.clip = audios[waypoint];
            audioSource.Play();
        }
        void SetAnimatorPara(int para)
        {
            Debug.Log("SetPara:" + para);
            Animator ani = GetComponent<Animator>();
            if (ani == null) return;
            ani.SetInteger("Talk", para);
        }

        void PlayWaypointVoice(Transform trans)
        {
            Debug.Log("PlayWaypointVoice..." + trans.name);
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;
            if (audio.Equals(currentAudio)) return;
            StartCoroutine(ShowEffect());
            if (currentAudio != null) currentAudio.Stop();
            audio.Stop();
            audio.Play();
            currentAudio = audio;
            //StartCoroutine(WayPointVoicePlayFinished(audio.clip.length));
        }

        IEnumerator WayPointVoicePlayFinished(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}
