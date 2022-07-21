using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuperDemoDelayAppera : MonoBehaviour
{
    public GameObject[] objs;
    private int index = 0;


    private void OnDisable()
    {
        foreach (var obj in objs)
        {
            obj.SetActive(false);
        }
    }
    private void OnEnable()
    {
        foreach (var obj in objs)
        {
            Appear(obj);
            index++;
        }
    }

    void Appear(GameObject go)
    {
        StartCoroutine(AppearByTime(go));
    }

    IEnumerator AppearByTime(GameObject go)
    {
        yield return new WaitForSeconds(index * 0.2f);
        go.SetActive(true);
    }

}