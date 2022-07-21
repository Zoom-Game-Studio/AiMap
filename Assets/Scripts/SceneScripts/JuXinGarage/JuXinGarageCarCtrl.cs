using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JuXinGarageCarCtrl : MonoBehaviour
{
    private int currentWayPoint = 0;
    private bool isArrived = true;
    private float speed = 5;
    public List<GameObject> waypointsList = new List<GameObject>();
    public GameObject isCarryObj;
    public GameObject isEmptyObj;
    private void Awake()
    {
    }
    public void StartGarageCar()
    {
        Debug.Log("sb...");
        StartCoroutine(MovingToNext());

    }

    private void HandleStartSuperDemoCar()
    {
        isCarryObj.SetActive(true);
        isEmptyObj.SetActive(false);
    }

    GameObject nextWayPoint;
    void StartMove()
    {
        Debug.Log("StartMove...");
        Debug.Log(currentWayPoint);
        if (currentWayPoint >= waypointsList.Count) return;
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
            Debug.Log("isArrived...");
            isArrived = true;

            if (currentWayPoint < waypointsList.Count)
            {
                currentWayPoint++;
                //if (currentWayPoint >= waypointsList.Count)
                //    currentWayPoint = 0;

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

}