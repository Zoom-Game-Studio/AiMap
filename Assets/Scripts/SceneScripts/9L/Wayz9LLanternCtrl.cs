using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wayz9LLanternCtrl : MonoBehaviour
{
    List<GameObject> lanterns = new List<GameObject>();
    public static int number = 0;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child == null) continue;
            child.name = (i + 1).ToString();
            number++;
            lanterns.Add(child.gameObject);
        }
    }

}
