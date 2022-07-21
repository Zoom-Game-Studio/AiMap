using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhiNengDaoRouteTrigger : MonoBehaviour
{
    private bool isInit = false;
    private bool isPlayed = false;
    private Camera mainCamera = null;
    public GameObject[] triggerGroup;
    private void Awake()
    {
        mainCamera = Camera.main;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (transform.name.Equals("Trigger1"))
        {
            OnTrigger1();
        }
        else if (transform.name.Equals("Trigger2"))
        {
            OnTrigger2();
        }
        else if (transform.name.Equals("Trigger3"))
        {
            OnTrigger3();
        }
    }

    void HideAll()
    {
        foreach (var item in triggerGroup)
        {
            item.SetActive(false);
        }
    }

    void OnTrigger1()
    {
        Debug.Log("OnTrigger1...");
        HideAll();
        triggerGroup[1].SetActive(true);
    }


    private void OnTrigger2()
    {
        Debug.Log("OnTrigger2...");
        HideAll();
        triggerGroup[2].SetActive(true);
    }

    private void OnTrigger3()
    {
        Debug.Log("OnTrigger3...");
        HideAll();
        triggerGroup[0].SetActive(true);
    }




    //private void Start()
    //{
    //    isInit = true;
    //}

    //void GuideTrigger()
    //{
    //    if (mainCamera == null) return;
    //    if (Vector3.Distance(mainCamera.transform.position, transform.position) < 5)
    //    {
    //        isPlayed = true;
    //        PlayWaypointVoice(transform);
    //    }
    //}

    //private void Update()
    //{
    //    if (!isInit) return;
    //    if (isPlayed) return;
    //    GuideTrigger();
    //}
    //void PlayWaypointVoice(Transform trans)
    //{
    //    Debug.Log("PlayWaypointVoice..." + trans.name);
    //    AudioSource audio = trans.GetComponent<AudioSource>();
    //    if (audio == null) return;
    //    audio.Stop();
    //    audio.Play();
    //}

}
