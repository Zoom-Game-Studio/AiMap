using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KleinEngine;
namespace AppLogic
{
    public class FYXYUICtrl : MonoBehaviour
    {
        Slider slider;
        AudioSource audio;
        public Button buttonPlay;
        public Button buttonPause;
        bool isInit = false;
        private void Start()
        {
            slider = GetComponent<Slider>();
            audio = GetComponent<AudioSource>();
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            isInit = true;
        }

        private void OnSliderValueChanged(float value)
        {

        }

        private void Update()
        {
            if (!isInit) return;
            slider.value = audio.time / audio.clip.length;

        }


        public void ButtoPlay()
        {
            buttonPause.gameObject.SetActive(true);
            buttonPlay.gameObject.SetActive(false);
            audio.Play();
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.FYXY_STOP_AUDIO);
        }

        public void ButtonPause()
        {
            buttonPlay.gameObject.SetActive(true);
            buttonPause.gameObject.SetActive(false);
            audio.Pause();
        }

        //float mp3StartPlayTime = 0f;//从音乐开始播放时起，开始累加时间
        //float fixPlayTime = 0f;//用于修复mp3播放时间进度误差越来越大的问题
        //// Update is called once per frame
        //void Update()
        //{
        //    if (audio.isPlaying)
        //    {
        //        mp3StartPlayTime += Time.deltaTime;
        //        fixPlayTime = mp3StartPlayTime * 0.01f;//此处累加时间百分之一的偏移量，就是修复的关键
        //        for (int i = 0; i < musiciansArr.Length; i++)
        //        {
        //            musiciansArr[i].FixMp3PlayingTime(audioSource.time + fixPlayTime);//更新实际时间进度
        //        }
        //    }
        //}
    }
}
