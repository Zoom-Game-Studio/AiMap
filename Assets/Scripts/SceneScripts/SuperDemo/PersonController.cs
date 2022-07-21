using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class PersonController : MonoBehaviour
    {
        public AudioClip[] audios;
        private float speed = 1;
        private bool isArrived = true;
        private int currentWayPoint = 0;
        private bool isInit = false;
        private List<GameObject> waypointsList = new List<GameObject>();
        private LookAtCamera lookatCamera;

        private void Awake()
        {
            Debug.Log("PersonController Start...");
            //if (isInit) return;
            //AppFacade.GetInstance().addEvent(ModuleEventType.GET_POSITION_IN_UNITY_CALL_BACK, HandleGetPositionInUnityCallBack);
            AppFacade.GetInstance().addEvent(ModuleEventType.GET_WAYPOINTS_LIST, HandleGetWayPointsList);
            //isInit = true;
        }

        private void HandleGetWayPointsList(EventObject ev)
        {
            Debug.Log("HandleGetWayPointsList...");
            if (ev.param == null) Debug.Log("is null@louis");
            waypointsList = ev.param as List<GameObject>;
            Debug.Log("Controllerwaypointslistcount:" + waypointsList.Count);
            StartMove();

        }

        GameObject nextWayPoint;
        void StartMove()
        {
            if (currentWayPoint >= waypointsList.Count)
            {
                Debug.Log("currentwaypoint:" + currentWayPoint);
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_PLAY_AIMAPGROUP);
                return;
            }
            Debug.Log("StartMove...");

            nextWayPoint = waypointsList[currentWayPoint];
            SetAnimatorPara(0);
            isArrived = false;
        }


        void MoveToNextPoint()
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nextWayPoint.transform.position, step);
            //transform.(nextWayPoint);
            transform.LookAt(nextWayPoint.transform.position);
            if (currentWayPoint.Equals(6))
            {
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_SUPERDEMO_CAR);
                //AppFacade.GetInstance().dispatchEvent(ModuleEventType.TALKING_TO_WORKER_IN_SUPERDEMO);
            }


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
            Debug.Log("xxx");
            audioSource.clip = audios[waypoint];
            audioSource.Play();
        }
        void SetAnimatorPara(int para)
        {
            Debug.Log("SETPara:" + para);
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
