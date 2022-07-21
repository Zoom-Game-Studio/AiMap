using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSceneCtrl : MonoBehaviour
{

//    private AppManager appManager; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("StartedGuideMap3:" + PlayerPrefs.HasKey("StartedGuideMap"));

        //        appManager = FindObjectOfType<AppManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
