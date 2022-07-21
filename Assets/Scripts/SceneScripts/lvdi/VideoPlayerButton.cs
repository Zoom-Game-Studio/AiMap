using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VideoPlayerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowVideoPlayer()
    {
        Transform parent = transform.parent.parent.parent;
        if (parent == null) return;
        Transform videoObj = parent.Find("VideoPlayer");
        if (videoObj == null) return;
        videoObj.gameObject.SetActive(true);
    }

    public void HideVideoPlayer()
    {
        Transform videoObj = transform.parent.parent;
        if (videoObj == null) return;
        videoObj.gameObject.SetActive(false);
    }

}
