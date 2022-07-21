using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhiNengDaoPanleCtrl : MonoBehaviour
{
    Camera camera;
    private void Start()
    {
        camera = Camera.main;
    }
    void Update()
    {
        if (camera == null) return;
        //transform.eulerAngles =-1*Quaternion.LookRotation(camera.transform.position).eulerAngles;
        transform.LookAt(new Vector3(camera.transform.position.x,0, camera.transform.position.z));
    }
}
