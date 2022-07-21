using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wayz9LFireworksCtrl : MonoBehaviour
{
    public GameObject fireworkPrefab;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(FireworksButtonClicked);
    }

    private void FireworksButtonClicked()
    {
        StartCoroutine(CreateFireworks());

    }

    IEnumerator CreateFireworks()
    {
        GameObject firework = Instantiate<GameObject>(fireworkPrefab);
        if (firework != null)
        {
            firework.transform.SetParent(transform, true);
            firework.transform.localPosition = fireworkPrefab.transform.localPosition;
            firework.transform.localRotation = fireworkPrefab.transform.localRotation;
            firework.transform.localScale = fireworkPrefab.transform.localScale;
            firework.SetActive(true);
        }
        yield return new WaitForSeconds(10f);
        Destroy(firework);
    }

}
