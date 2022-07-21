using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class AutoDriveCarController : MonoBehaviour
    {
        private int currentWayPoint = 0;
        private bool isArrived = true;
        public float speed = 10;
        public List<GameObject> waypointsList = new List<GameObject>();
        public bool startWhenInit = false;
        public GameObject isCarryObj;
        public GameObject isEmptyObj;
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.START_SUPERDEMO_CAR, HandleStartSuperDemoCar);
        }

        private void Start()
        {
            if (startWhenInit)
                AppFacade.GetInstance().dispatchEvent(ModuleEventType.START_SUPERDEMO_CAR);
        }

        private void HandleStartSuperDemoCar(EventObject ev)
        {
            if (!gameObject.activeInHierarchy) return;
            if (isCarryObj != null)
                isCarryObj.SetActive(true);
            if (isEmptyObj != null)
                isEmptyObj.SetActive(false);
            StartCoroutine(MovingToNext());
        }

        GameObject nextWayPoint;
        void StartMove()
        {
            //Debug.Log("StartMove...");
            //Debug.Log(currentWayPoint);
            nextWayPoint = waypointsList[currentWayPoint];
            isArrived = false;
        }

        void MoveToNextPoint()
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWayPoint.transform.position, step);
            //transform.(nextWayPoint);
            transform.LookAt(nextWayPoint.transform.position);
            if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
            {
                //Debug.Log("isArrived...");
                isArrived = true;

                if (currentWayPoint < waypointsList.Count)
                {
                    currentWayPoint++;
                    if (currentWayPoint >= waypointsList.Count)
                        currentWayPoint = 0;

                    StartMove();
                }
            }
        }
        private void Update()
        {
            if (isArrived) return;
            MoveToNextPoint();
        }

        IEnumerator MovingToNext()
        {
            yield return new WaitForSeconds(1);
            StartMove();
        }
        public void NextButtonClicked()
        {
            Debug.Log("NextButtonClickeed...");
            StartMove();
        }

        private void OnDisable()
        {
            //StopCoroutine(MovingToNext());
        }
    }
}

