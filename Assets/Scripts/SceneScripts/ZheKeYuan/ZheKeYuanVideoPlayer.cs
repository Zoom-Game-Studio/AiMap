using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ZheKeYuanVideoPlayer : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Transform root = transform.parent.root;
        if (root == null) return;
        Debug.Log(root.name);
        VideoPlayer video = root.GetComponentInChildren<VideoPlayer>();
        if (video == null) return;
        video.Play();
        AudioSource audio = root.GetComponentInChildren<AudioSource>();
        if (audio == null) return;
        audio.Play();
    }

}
