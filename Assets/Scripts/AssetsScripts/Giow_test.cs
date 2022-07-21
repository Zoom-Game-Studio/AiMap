using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giow_test : MonoBehaviour
{
    float a=1;
    int bb=3;
    bool cc;
    bool dd;
    float e;
    public float ee = 37.667f;
    public float speed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!cc)
        {
            if (bb == 1)
            {
                a += speed * Time.deltaTime;
                if (a > 1)
                {
                    bb = 3;
                    a = 1;
                    cc = true;
                    dd = true;
                    StartGame();
                }
            }
            if (bb == 2)
            {
                a -= speed * Time.deltaTime;
                if (a < 0)
                {
                    bb = 3;
                    a = 0;
                    cc = true;
                    dd = true;
                    
                }
            }
            GetComponent<Renderer>().material.SetFloat("_node_6357", a);
        }
        if(dd)
        {
            e -= Time.deltaTime;
            if (e<=0)
            {
                dd = false;
                cc = false;
                bb = 1;
            }
        }
    }
    public void StartGame()
    {
        a = 1;
        e = ee;
        bb = 2;
        cc = false;
    }
}
