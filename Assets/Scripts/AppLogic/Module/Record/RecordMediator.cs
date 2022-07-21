using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using UnityEngine.SceneManagement;
using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
using NatMic;
using NatMic.DSP;
using System.IO;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AppLogic
{
    public class RecordMediator : BaseMediator, IAudioProcessor
    {
        [Header("Recording")]
        public int videoWidth = 720;
        public int videoHeight = 1280;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;

        private IAudioDevice audioDevice;


        private RecordView recordView;
        private float recordTime = 58f;
        private DateTime oldTime;
        private Tick tick;
        private Sprite logoSprite;

        public RecordMediator()
        {
            m_mediatorName = MEDIATOR.RECORD;
        }

        protected override void onInit()
        {
            recordView = viewComponent as RecordView;
            recordView.SetRecordBtnDefault();

        }

        private void OnRecordingStartAndCountDown()
        {
            if (!recordView.isRecording) return;

            var deltaDateTime = DateTime.Now - oldTime;
            {
                if (recordView.PhotoBtn != null)
                    recordView.PhotoBtn.enabled = false;
                recordView.isRecording = true;
            }
            if (deltaDateTime.TotalSeconds < recordTime)
            {
                float value = 1f / recordTime * (float)deltaDateTime.TotalSeconds;
                recordView.SetImageFillAmount(value);
                return;
            }
            RecordBtnOnClick();
        }

        /// <summary>
        /// 拍照按钮点击
        /// </summary>
        public void PhotoBtnOnClick()
        {

            {
                Texture2D t;
                string bannerPath = PlayerPrefs.GetString("BannerPath");

                if (bannerPath != null && bannerPath.Equals(String.Empty) && logoSprite!=null)
                {
                   int logoSpriteWidth = Screen.width;
                   int logoSpriteHeight = logoSprite.texture.height * Screen.width / logoSprite.texture.width;
                    Sprite tempSprite =UnityUtilties.GetSpriteFormPng(bannerPath, logoSpriteWidth, logoSpriteHeight);
                    if (tempSprite != null)
                    {
                        logoSprite = tempSprite;
                        Debug.Log("tempsprite is not null");
                    }
                    Debug.Log("Add Logo...====");
                }
                //Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                
            }
            {
                Debug.Log("Capture failed");
            }
            //ReportActions("601", "2");
            //引导图
            if (PlayerPrefs.HasKey("StartedGuideMapPhoto"))
            {
                //Hint_.SetActive(false);
            }
            else
            {
                //Hint_.GetComponent<UnityEngine.UI.Image>().sprite = Hint_sprite;
                //Hint_.SetActive(true);
                //PlayerPrefs.SetInt("StartedGuideMapPhoto", 666);
            }
        }


        /// <summary>
        /// 录制视频按钮点击
        /// </summary>
        private void RecordBtnOnClick()
        {
            Debug.Log("RecordBtn cliked");

            if (recordView.isRecording)
                OnVideoRecordingStop();
            else
                OnVideoRecordingStart();
        }

        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(recordView.RecordBtn))
            {
                RecordBtnOnClick();
            }
            else if (ev.param.Equals(recordView.PhotoBtn))
            {
                PhotoBtnOnClick();
            }
            else if (ev.param.Equals(recordView.SwitchGifBtn))
            {

            }
            else if (ev.param.Equals(recordView.SwitchPhotoBtn))
            {

            }
            else if (ev.param.Equals(recordView.SwitchVideoBtn))
            {

            }
        }

        /// <summary>
        /// 开始倒计时
        /// </summary>
        public void StartCountdown()
        {
            oldTime = DateTime.Now;
            recordView.isRecording = true;
            //isStart = true;
        }

        private void OnVideoRecordingStop()
        {
            StopRecording();
            tick.stop();
            tick = null;
        }

        private void OnVideoRecordingStart()
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
        /// 录制完成UI修改
        /// </summary>
        public void RecordCompleted()
        {
            recordView.isRecording = false;
            if (recordView.PhotoBtn != null)
                recordView.PhotoBtn.enabled = true;
            recordView.ResetFillAmountImage();
        }

        /// <summary>
        /// 传入地址打开视频预览界面播放
        /// </summary>
        /// <param name="path">Path.</param>
        public void StartPreviewVideo(string path)
        {
            if (path == null) { Debug.Log("current video path is null"); return; }
            Debug.Log("current video Path====" + path);
            VideoPreview();
            //VideoPlay();
            //arModelSceneCtrl.ResetTopMask(false);
        }

        /// <summary>
        /// Videos the play.
        /// </summary>
        public void VideoPreview()
        {
            Debug.Log("VideoPreview...");

            sendModuleEvent(ModuleEventType.MODEL_ENABLE, false);

            sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.PREVIEW);
        }

        /// <summary>
        /// 录制完成回调
        /// </summary>
        /// <param name="path">Path.</param>
        private void OnReplay(string path)
        {
            //recordView..text = "开始录制";
            recordView.SetTextState("开始录制");
            recordView.SetRecordBtnDefault();
            Debug.Log("Saved recording to: " + path);

            string[] spr = path.Split('/');
            // Playback the video
#if UNITY_EDITOR
            // EditorUtility.OpenWithDefaultApp(path);
#endif
#if UNITY_IOS
//todo
#endif
#if UNITY_ANDROID

#if UNITY_EDITOR
#else

            string tempPath =
               Application.persistentDataPath.Split(new string[] { "/Android" }, StringSplitOptions.None)[0];

            string videoPath = tempPath + "/DCIM/Camera/" + spr[spr.Length - 1];

            File.Move(path, videoPath);
            string folderPath = tempPath + "/DCIM/Camera";
#endif
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
            StartCountdown();
            //开启倒计时
            tick = TickManager.GetInstance().createTick(0, null, OnRecordingStartAndCountDown);
            tick.start();
            recordView.SetTextState("暂停录制");
            recordView.SetRecordBtnRecording();
            //todo
            //AppManager.Instance.arModelSceneCtrl.ResetTopMask(true);
            //AppManager.Instance.SendActionReport("601", "1");
        }
    }
}
