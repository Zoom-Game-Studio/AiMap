using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(HideInSeconds());
    }

    IEnumerator HideInSeconds()
    {
        yield return new WaitForSeconds(1f);
        transform.gameObject.SetActive(false);
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
