using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SuperDemoSwitchTarget : MonoBehaviour
{

    public GameObject[] allObjs;
    private int index=0;

    void Start()
    {
        if (allObjs.Length < 2) return;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        index++;
        if (index >= allObjs.Length)
            index = 0;
        for (int i = 0; i < allObjs.Length; i++)
        {
            if (index.Equals(i))
            {
                allObjs[index].SetActive(true);
                continue;
            }
            allObjs[i].SetActive(false);
        }

    }

   
}
