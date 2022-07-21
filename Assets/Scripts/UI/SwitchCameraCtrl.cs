using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCameraCtrl : MonoBehaviour
{
    bool isFront = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        isFront = !isFront;
        print("isFront" + isFront);
        //AREventUtil.DispatchEvent(GlobalOjbects.SWITCH_CAMERA, isFront);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
