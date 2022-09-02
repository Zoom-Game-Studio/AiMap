using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTopic : MonoBehaviour
{
    private bool isSanXingDui;
    public GameObject SanxingDUi, ShengWuZhiYao;
    // Start is called before the first frame update
    void Start()
    {
        isSanXingDui = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangScene() {
        isSanXingDui = !isSanXingDui;
        SanxingDUi.SetActive(isSanXingDui);
        ShengWuZhiYao.SetActive(!isSanXingDui);


    }

}
