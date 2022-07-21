using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using AppLogic;

public class Wayz9LGuideTrigger : MonoBehaviour
{
    public AudioClip[] audios;

    private bool isInit = false;
    private bool isPlayed = false;
    public List<GameObject> waypointsList = new List<GameObject>();

    private Camera mainCamera = null;

    public float triggerDistance = 5;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        isInit = true;
    }

    void GuideTrigger()
    {
        if (mainCamera == null) return;
        if (Vector3.Distance(mainCamera.transform.position, transform.position) <= triggerDistance)
        {
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SUPERDEMO_GUIDE_TRIGGER, this.transform);
        }
    }

    private void Update()
    {
        if (!isInit) return;
        if (isPlayed) return;
        GuideTrigger();
    }
}
