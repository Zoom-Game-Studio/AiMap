using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SuperDemoDelayDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DelayDestroy());
    }
    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }

}