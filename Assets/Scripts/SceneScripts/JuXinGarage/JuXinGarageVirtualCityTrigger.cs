using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JuXinGarageTrigger
{
    /// <summary>
    /// 触发点
    /// </summary>
    public List<Transform> triggerPoints = new List<Transform>();
    /// <summary>
    /// 显示资源
    /// </summary>
    public GameObject[] triggerAreaObjs;
}

public class JuXinGarageVirtualCityTrigger : MonoBehaviour
{
    bool isInit = false;
    Camera mainCamera = null;
    //public Transform triggerPoint;
    //public GameObject triggerAreaObj;
    public float distance = 5;

    public List<JuXinGarageTrigger> juxinGargeTriggers = new List<JuXinGarageTrigger>();
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var juxinTrigger in juxinGargeTriggers)
        {
            foreach (var triggerPoint in juxinTrigger.triggerPoints)
            {
                foreach (var triggerAreaObj in juxinTrigger.triggerAreaObjs)
                {
                    triggerAreaObj.SetActive(false);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isInit) return;
        foreach (var juxinTrigger in juxinGargeTriggers)
        {
            foreach (var triggerPoint in juxinTrigger.triggerPoints)
            {
                if (Vector3.Distance(triggerPoint.position, mainCamera.transform.position) < distance)
                {
                    foreach (var triggerAreaObj in juxinTrigger.triggerAreaObjs)
                    {
                        triggerAreaObj.SetActive(true);
                    }
                    isInit = true;
                }
            }

        }

    }
}

