/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples {

    #if UNITY_EDITOR
	using UnityEditor;
	#endif
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Clocks;
    using Inputs;
  
    using System;
    using System.IO;

    public class ReplayCam : MonoBehaviour {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        [Header("Recording")]
        public int videoWidth = 720;
        public int videoHeight = 1280;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;

        public void StartRecording () {
            // Start recording
           
                //foreach (AudioSource item in AppManager.Instance.model.GetComponentsInChildren<AudioSource>())
                //{
                //    //  Debug.Log("AppManager.Instance.model name:" + AppManager.Instance.model.name);
                //    Debug.Log("microphoneSource 00000" );
                //    microphoneSource = item;
                //    Debug.Log("microphoneSource name:" + microphoneSource.transform.name);
                
                   
                //}
                microphoneSource.Play();
           
            Debug.Log("microphoneSource 1111");
            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                recordMicrophone ? AudioSettings.outputSampleRate : 0,
                recordMicrophone ? (int)AudioSettings.speakerMode : 0,
                OnReplay
            );
            Debug.Log("microphoneSource 2222");
            // Create recording inputs
            cameraInput = CameraInput.Create(videoRecorder, Camera.main, recordingClock);
            {
              
                StartMicrophone();
                audioInput = AudioInput.Create(videoRecorder, microphoneSource, recordingClock, true);
                Debug.Log("audioInput ");
            }
        }

        private void StartMicrophone () {
#if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
           
            {
                Debug.Log("AppManager.Instance.model....");
                //foreach (AudioSource item in AppManager.Instance.model.GetComponentsInChildren<AudioSource>())
                //{
                //    Debug.Log("child name:"+item .name );
                //   // Debug.Log("model name:" + AppManager.Instance.model.name);

                //    microphoneSource = item;
                //    Debug.Log("microphoneSource name:" + microphoneSource.transform.name);
                //}
                // microphoneSource = AppManager.Instance.model.GetComponent<AudioSource>();
            }
            if (microphoneSource==null)
            {
                Debug.Log("microphoneSource==null)");
            }
            else
            {
                if (microphoneSource.clip==null)
                {
                    Debug.Log("microphoneSource.clip ==null)");
                }
                else
                {
                  
                    Debug.Log("microphoneSource.clip is true........" + microphoneSource.gameObject.name); ;
                }
            }
            Debug.Log("clip .name:" + microphoneSource.clip .name);
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
          //  microphoneSource.clip = Microphone.Start(null, true, 1, 44100);
           // 
            while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            Debug.Log("microphoneSource.volume1:"+ microphoneSource.volume);
            microphoneSource.volume = 0.5f;
            Debug.Log("microphoneSource.volume2:" + microphoneSource.volume);
            microphoneSource.Play();
            if (microphoneSource.isPlaying )
            {
                Debug.Log("microphoneSource.isPlaying:");
                Debug.Log("microphoneSource.volume...:" + microphoneSource.volume);
            }
            else
            {
                Debug.Log(" microphoneSource.Play false");
            }
            
#endif
        }

        public void StopRecording () {
            // Stop the recording inputs


          //  AppManager.Instance.videoSceneCtrl.SetPreviewMode(true);
            if (recordMicrophone) {
                StopMicrophone();
                audioInput.Dispose();
            }
            cameraInput.Dispose();
            // Stop recording
            videoRecorder.Dispose();
        }

        private void StopMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR
            Microphone.End(null);
            microphoneSource.Stop();
            #endif


        }

        private void OnReplay (string path)
        {
            Debug.Log("Saved recording to: "+path);
            // Playback the video
            string[] spr = path.Split('/');
            Debug.Log(spr[spr.Length - 1]);
#if UNITY_EDITOR
            EditorUtility.OpenWithDefaultApp(path);
#elif UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
#elif UNITY_ANDROID
            Handheld.PlayFullScreenMovie(path);


             string tempPath =
            Application.persistentDataPath.Split(new string[] { "/Android" }, StringSplitOptions.None)[0];
      
           
             string videoPath = tempPath+ "/DCIM/Camera/"+spr[spr.Length - 1];
          
            File.Move(path, videoPath);
            string folderPath = tempPath + "/DCIM/Camera";

            string fileName=spr[spr.Length - 1];
            VideoDoWith();

#endif
            //  
            //  string pa = "C:/Users/Administrator/Desktop/aa/"+ spr[spr.Length - 1];


        }

        //如何处理视频
        void VideoDoWith()
        {

        }
    }
}