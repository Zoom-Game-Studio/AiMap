using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KleinEngine;
using LitJson;
using System;
using Sirenix.OdinInspector;
using Random = System.Random;

namespace AppLogic
{
    public class Wayz9LButtonCtrl : MonoBehaviour
    {
        public Button buttonLantern;
        public Button buttonGuess;
        public VerticalText questionTxt;
        private static Transform currentTrans = null;
        private Animator currentAnimator;
        private string aniPara = "LanternPara";
        void Start()
        {
            if (questionTxt == null)
                questionTxt = GetComponentInChildren<VerticalText>();
            if (buttonLantern)
                buttonLantern.onClick.AddListener(ButtonClickLantern);
            if (buttonGuess)
            {
                buttonGuess.onClick.AddListener(ButtonClickGuess);
                buttonGuess.gameObject.SetActive(false);
            }
            AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_QUESTION, HandleGetQuestion);
            AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_SEND_ANSWER, HandleSendAnswer);
            AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_RESULT, HandleGetResult);
        }

        private void HandleGetResult(EventObject ev)
        {
            if (currentTrans == null || currentTrans != transform) return;
            UrlFromUnity urlFromUnity = ev.param as UrlFromUnity;
            if (urlFromUnity == null) return;
            Debug.Log("GetResult:" + urlFromUnity.url);
            if (questionTxt == null) return;
            questionTxt.text = urlFromUnity.url;
            currentAnimator = currentTrans.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 1);
            buttonGuess.gameObject.SetActive(false);
            Debug.Log("buttonguess false");
            buttonLantern.gameObject.SetActive(false);
            Debug.Log("buttonlantern false");
        }

        private void HandleSendAnswer(EventObject ev)
        {
            if (currentTrans == null) return;
            currentAnimator = currentTrans.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 0);
            Wayz9LLanternCtrl.number++;

            currentTrans.name = Wayz9LLanternCtrl.number.ToString();
            currentTrans = null;
            currentAnimator = null;
            buttonGuess.gameObject.SetActive(false);
        }

        private void HandleGetQuestion(EventObject ev)
        {
            if (currentTrans == null || currentTrans != transform) return;
            UrlFromUnity urlFromUnity = ev.param as UrlFromUnity;
            if (urlFromUnity == null) return;
            Debug.Log("GetQuestion:" + urlFromUnity.url);
            if (questionTxt == null) return;
            questionTxt.text = urlFromUnity.url;
            currentAnimator = currentTrans.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 1);
            buttonGuess.gameObject.SetActive(true);
        }

        /// <summary>
        /// 获取谜语
        /// </summary>
        [Button]
        public void ButtonClickLantern()
        {
            Debug.Log("ButtonClickLantern...");
            currentTrans = transform;
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            urlFromUnity.type = "5";
            urlFromUnity.url = transform.parent.name;
            string json = JsonMapper.ToJson(urlFromUnity);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }


        /// <summary>
        /// 猜谜
        /// </summary>
        [Button]
        public void ButtonClickGuess()
        {
            Debug.Log("ButtonClickGuess...");
            currentTrans = transform;
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            urlFromUnity.type = "6";
            urlFromUnity.url = transform.parent.name;
            string json = JsonMapper.ToJson(urlFromUnity);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }

        void RandomNumber()
        {
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            int RmNum = 10;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                int nValue = rm.Next(100);
                if (!hashtable.ContainsValue(nValue) && nValue != 0)
                {
                    hashtable.Add(nValue, nValue);
                    Console.WriteLine(nValue.ToString());
                }
            }
        }
    }

}