/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
//using Inputs;
using NatMic;
using NatMic.DSP;
using System;
using System.IO;
using KleinEngine;
using AppLogic;

public class NatRecorderCtrl : MonoBehaviour, IAudioProcessor
{

    /**
    * ReplayCam Example
    * -----------------
    * This example records the screen using a `CameraRecorder`.
    * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
    * -----------------
    * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
    */

    [Header("Recording")]
    public int videoWidth = 1280;
    public int videoHeight = 720;

    //[Header("Microphone")]
    //public bool recordMicrophone;
    //public AudioSource microphoneSource;

    private MP4Recorder videoRecorder;
    private IClock recordingClock;
    private CameraInput cameraInput;

    private IAudioDevice audioDevice;
    //private AudioInput audioInput;

    public Image buttonImage;//按钮的图片
    public Text buttonText;//按钮的文字
    public Sprite imagePlay;
    public Sprite imageStop;


    void Start()
    {
        //AREventUtil.AddListener(GlobalOjbects.START_VIDEO_RECORDING,OnVideoRecordingStart);
        //AREventUtil.AddListener(GlobalOjbects.STOP_VIDEO_RECORDING, OnVideoRecordingStop);
    }


    private void OnVideoRecordingStop(AREventArgs ev)
    {
        StopRecording();
    }

    private void OnVideoRecordingStart(AREventArgs ev)
    {
        StartRecording();
        OnStatRecording();
    }

    public void StartRecording()
    {
        var sampleRate = 44100;
        var channelCount = 1;


        recordingClock = new RealtimeClock();
        videoRecorder = new MP4Recorder(
            videoWidth,
            videoHeight,
            30,
            sampleRate, //recordMicrophone ? AudioSettings.outputSampleRate : 0,
            channelCount, //recordMicrophone ? (int)AudioSettings.speakerMode : 0,
            OnReplay
        );
        // Create recording inputs
        cameraInput = CameraInput.Create(videoRecorder, Camera.main, recordingClock);

        audioDevice = AudioDevice.Devices[0];
        //audioInput = AudioInput.Create(videoRecorder, microphoneSource, recordingClock,true);
        audioDevice.StartRecording(sampleRate, channelCount, this);
    }

    // Invoked by audio device with new audio data
    public void OnSampleBuffer(float[] sampleBuffer, int sampleRate, int channelCount, long timestamp)
    {
        // Send sample buffers directly to NatCorder for recording
        videoRecorder.CommitSamples(sampleBuffer, recordingClock.Timestamp);
    }

    /*
            private void StartMicrophone () {
                #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
                // Create a microphone clip
                microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
                while (Microphone.GetPosition(null) <= 0) ;
                // Play through audio source
                microphoneSource.timeSamples = Microphone.GetPosition(null);
                microphoneSource.loop = true;
                microphoneSource.Play();
                #endif
            }
    */
    public void StopRecording()
    {

        audioDevice.StopRecording();

        cameraInput.Dispose();

        videoRecorder.Dispose();
    }


    /// <summary>
    /// 录制完成回调
    /// </summary>
    /// <param name="path">Path.</param>
    private void OnReplay(string path)
    {
        buttonText.text = "开始录制";
        buttonImage.sprite = imagePlay;
        Debug.Log("Saved recording to: " + path);

        string[] spr = path.Split('/');
        // Playback the video
#if UNITY_EDITOR
        EditorUtility.OpenWithDefaultApp(path);
#endif
#if UNITY_IOS
#endif
#if UNITY_ANDROID
        //Handheld.PlayFullScreenMovie(path);
        string tempPath =
           Application.persistentDataPath.Split(new string[] { "/Android" }, StringSplitOptions.None)[0];


        string videoPath = tempPath + "/DCIM/Camera/" + spr[spr.Length - 1];

        File.Move(path, videoPath);
        string folderPath = tempPath + "/DCIM/Camera";

        //string fileName = spr[spr.Length - 1];
        //AppManager.Instance.RefreshPhotoLibraryCallBack(folderPath, fileName);
#endif

        if (PlayerPrefs.HasKey("StartedGuideMapREC"))
        {
        }
        else
        {
            PlayerPrefs.SetInt("StartedGuideMapREC", 666);
        }
    }

    /// <summary>
    /// 当录制开始时
    /// </summary>
    void OnStatRecording()
    {
        if (buttonText != null)
            buttonText.text = "暂停录制";
        if (buttonImage != null)
            buttonImage.sprite = imageStop;
   }
}

