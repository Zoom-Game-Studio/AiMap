using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideMap : MonoBehaviour {

    public GameObject Hint_;
    public Sprite sprite;
	void Start ()
    {
        //return;
        Debug.Log("StartedGuideMap1:"+PlayerPrefs.HasKey("StartedGuideMap"));
        if (PlayerPrefs.HasKey("StartedGuideMap"))
        {
            Hint_.SetActive(false);
        }
        else
        {
            Hint_.GetComponent<Image>().sprite = sprite;
            Hint_.SetActive(true);
        }
    }
    void Update () {
	}
    void LateUpdate()
    {
        //启动存储一个数
        //PlayerPrefs.GetInt("StartedGuideMap", 666);
    }
    public void CloseHint()
    {
        PlayerPrefs.SetInt("StartedGuideMap", 666);
        //PlayerPrefs.SetInt("StartedGuideMap", 666);
        Debug.Log("StartedGuideMap2:" + PlayerPrefs.HasKey("StartedGuideMap"));
        Hint_.SetActive(false);
    }
}
