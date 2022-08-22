using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaicLogic : MonoBehaviour
{
    private Animation RoBotAnimation;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        RoBotAnimation = transform.GetChild(0).GetChild(0).GetComponentInChildren<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RoBotAnimation&&!RoBotAnimation.isPlaying&&!transform.GetChild(0).GetChild(1).gameObject.activeSelf) {

            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

   

}
