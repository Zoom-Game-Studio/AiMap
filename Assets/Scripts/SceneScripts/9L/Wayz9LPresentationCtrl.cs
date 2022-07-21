using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KleinEngine;
using UnityEngine.Video;
using System;
namespace AppLogic
{
    [System.Serializable]
    public class PresentationInfo
    {
        public int speakerID = 0;
        public Texture mainTex = null;
        public AudioClip clip;
        public VideoClip videoClip;
    }

    public class Wayz9LPresentationCtrl : MonoBehaviour
    {
        public Animator speaker1Ani;
        public Animator speaker2Ani;

        public MeshRenderer msr;
        public VideoPlayer videoPlayer;
        private AudioSource audio;
        private int index = -1;
        private string aniPara = "isSpeaking";
        public PresentationInfo[] presentationList;
        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        void StartPresentation()
        {
            Debug.Log(index);
            index++;
            PresentationInfo presentationInfo = presentationList[index];
            if (presentationInfo == null) return;
            if (msr == null) return;
            if (presentationInfo.mainTex)
                msr.material.mainTexture = presentationInfo.mainTex;
            SwitchSpeaker(presentationInfo.speakerID);
            if (presentationInfo.clip)
            {
                PlaySpeakerAudio(presentationInfo.clip);
            }
            if (presentationInfo.videoClip)
            {
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.clip = presentationInfo.videoClip;
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Stop();
                videoPlayer.gameObject.SetActive(false);
            }
        }



        void PlaySpeakerAudio(AudioClip audioClip)
        {
            if (audio == null) return;
            audio.Stop();
            audio.clip = audioClip;
            audio.Play();
            StartCoroutine(AudioSpeakerPlayFinished(audio.clip.length, callback));
        }

        private IEnumerator AudioSpeakerPlayFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 
            if (index < presentationList.Length - 1)
            {
                StartPresentation();
            }

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");


            #endregion

        }

        private void callback()
        {
        }
        void SwitchSpeaker(int speakerID)
        {
            if (speakerID.Equals(1))
            {
                //speaker1Ani.enabled = false;
                speaker1Ani.SetInteger(aniPara, 0);
                //speaker2Ani.enabled = true;
                speaker2Ani.SetInteger(aniPara, 1);
            }
            else
            {
                //speaker2Ani.enabled = false;
                speaker1Ani.SetInteger(aniPara, 1);
                speaker2Ani.SetInteger(aniPara, 0);

                //speaker1Ani.enabled = true;
            }
        }

       
        void Start()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_PRESENTATION_START, PresentaionStart);
        }
        bool isStart = false;

        private void PresentaionStart(EventObject obj)
        {
            if (isStart) return;
            StartPresentation();
            isStart = true;
        }
    }

}