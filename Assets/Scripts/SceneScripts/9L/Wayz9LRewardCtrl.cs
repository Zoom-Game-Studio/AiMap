using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;

public class Wayz9LRewardCtrl : MonoBehaviour
{
    public Animation rewardAni;
    private Vector3 startPos;
    private Camera camera;

    private bool isArrived = true;
    public float speed = 10;

    Vector3 nextWayPoint;
    bool isComing = false;
    private void Awake()
    {
        AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleOnTriggerSwitch);
    }
    private void Start()
    {
        startPos = transform.position;
        camera = Camera.main;
    }
    private void HandleOnTriggerSwitch(EventObject ev)
    {
        Debug.Log("HandleSwitchObj...");
        Transform trans = ev.param as Transform;
        if (transform.Equals(trans))
        {
            //Vector3 finalPos = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + 2);
            Vector3 finalPos = camera.transform.position + camera.transform.forward * 2;
            nextWayPoint = finalPos;

            StartMove();
        }
    }


    void StartMove()
    {
        Debug.Log("StartMove...");
        isComing = !isComing;
        nextWayPoint = isComing ? nextWayPoint : startPos;
        Debug.Log(nextWayPoint.ToString());
        isArrived = false;
    }

    void MoveToNextPoint()
    {
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWayPoint, step);
        //transform.(nextWayPoint);
        transform.LookAt(nextWayPoint);
        if (Vector3.Distance(gameObject.transform.position, nextWayPoint) < 0.5)
        {
            Debug.Log("isArrived...");
            isArrived = true;

            //if (currentWayPoint < waypointsList.Count)
            //{
            //    currentWayPoint++;
            //    if (currentWayPoint >= waypointsList.Count)
            //        currentWayPoint = 0;

            //    StartMove();
            //}
        }
    }
    private void Update()
    {
        if (isArrived) return;
        MoveToNextPoint();
    }

}