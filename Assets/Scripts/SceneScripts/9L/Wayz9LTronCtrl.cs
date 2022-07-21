using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Wayz9LTronCtrl : MonoBehaviour
{
    public Animation ani;
    public VideoPlayer[] videos;
    private void OnEnable()
    {
        ani.enabled = false;
        ani.enabled = true;
        ani.Play();
        StartCoroutine(ShowVideos());
    }
    IEnumerator ShowVideos()
    {
        yield return new WaitForSeconds(3.0f);
        foreach (var video in videos)
        {
            PlayAVideo(video);
        }

    }

    void PlayAVideo(VideoPlayer videoplayer)
    {
        videoplayer.gameObject.SetActive(true);
        videoplayer.Play();
        StartCoroutine(InitVideo(videoplayer));
    }

    IEnumerator InitVideo(VideoPlayer videoplayer)
    {
        MeshRenderer render = videoplayer.GetComponent<MeshRenderer>();
        render.enabled = false;
        yield return new WaitForSeconds(1f);
        render.enabled = true;

    }

    private void OnDisable()
    {
        foreach (var video in videos)
        {
            video.gameObject.SetActive(false);

        }
        StopCoroutine(ShowVideos());
    }
}