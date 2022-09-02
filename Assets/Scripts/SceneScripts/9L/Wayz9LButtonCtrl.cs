using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KleinEngine;
using LitJson;
using System;
using System.Net.Http;
using Architecture;
using BestHTTP;
using NRKernal;
using QFramework;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.EventSystems;
using WeiXiang;
using Console = System.Console;
using Random = System.Random;

namespace AppLogic
{
    public class Wayz9LButtonCtrl : MonoBehaviour, IController,IPointerClickHandler
    {
        public Button buttonLantern;
        public Button buttonGuess;
        public VerticalText questionTxt;
        private static Transform currentTrans = null;
        private Animator currentAnimator;
        private string aniPara = "LanternPara";

        private void Awake()
        {
            //设定题目id
            SetMiyuId();
            //改为ar交互
            this.theCollider = this.gameObject.AddComponent<SphereCollider>();
            theCollider.center = new Vector3(0, 0.1f, 0.16f);
            theCollider.radius = 0.35f;
            _theMiyu = null;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_theMiyu == null)
            {
                OnClickOpenLantern();
            }
            else
            {
                OnClickGuessMiyu();
            }
        }

        void Start()
        {
            SetMiyuId();
            if (questionTxt == null)
                questionTxt = GetComponentInChildren<VerticalText>();
            if (buttonLantern)
                buttonLantern.onClick.AddListener(OnClickOpenLantern);
            if (buttonGuess)
            {
                buttonGuess.onClick.AddListener(OnClickGuessMiyu);
                // buttonGuess.gameObject.SetActive(false);
            }
            // AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_QUESTION, HandleGetQuestion);
            // AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_SEND_ANSWER, HandleSendAnswer);
            // AppFacade.GetInstance().addEvent(ModuleEventType.WAYZ9L_NEW_YEAR_GET_RESULT, HandleGetResult);
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

        void ShouQiDenglong()
        {
            currentAnimator = transform.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 1);
            buttonGuess.gameObject.SetActive(true);
        }

        /// <summary>
        /// 获取谜语
        /// </summary>
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
        public void ButtonClickGuess()
        {
            Debug.Log("ButtonClickGuess...");
            // currentTrans = transform;
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

        #region 新的谜语处理逻辑

        //流程是，点一下打开灯笼，关闭打开灯笼的按钮，点一下答题，答题成功关闭答题成功的按钮
        private MiYu _theMiyu;
        public static int id = 1;
        private int _id = 0;
        public string url = "https://aimap.newayz.com/aimap/lantern/v1/ar_riddles/:id?lantern_id=";
        private SphereCollider theCollider;

        /// <summary>
        /// 刷新本灯笼的谜语
        /// </summary>
        void SetMiyuId()
        {
            _id = id++;
            // _id = 1;
            MessageBroker.Default.Receive<GuessMiyuComplete>().Subscribe(OnGuessComplete).AddTo(this);
        }

        /// <summary>
        /// 点击打开灯笼
        /// </summary>
        [Button("点击打开灯笼")]
        void OnClickOpenLantern()
        {
            var r = new HTTPRequest(new Uri(url + _id), HTTPMethods.Get, OnGetMiyu);
            r.Send();
        }

        /// <summary>
        /// 获取谜语的回调，收到后打开灯笼
        /// </summary>
        /// <param name="r"></param>
        /// <param name="p"></param>
        void OnGetMiyu(HTTPRequest r, HTTPResponse p)
        {
            if (_theMiyu?.answer!= null)
            {
                throw new HttpRequestException("已经有谜语了:" + _theMiyu?.description);
            }
            if (r != null)
            {
                if (r.State == HTTPRequestStates.Finished && p.DataAsText != null)
                {
                    _theMiyu = Newtonsoft.Json.JsonConvert.DeserializeObject<MiYu>(p.DataAsText);
                    questionTxt.text = _theMiyu?.description;
                    OpenLantern();
                }
            }

            r?.Dispose();
            p.Dispose();
        }

        /// <summary>
        /// 打开灯笼
        /// </summary>
        void OpenLantern()
        {
            currentAnimator = transform.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 1);
            buttonLantern.gameObject.SetActive(false);
            buttonGuess.gameObject.SetActive(true);
        }

        [Button("开始答题")]
        void OnClickGuessMiyu()
        {
            //发送事件打开答题ui
            MessageBroker.Default.Publish(new GuessEventOpenUi
            {
                miyu = _theMiyu
            });
        }

        void OnGuessComplete(GuessMiyuComplete evt)
        {
            if (_theMiyu != null)
            {
                if (evt?.miyu.id == _theMiyu.id)
                {
                    CloseLantern();
                }
            }
        }

        [Button("答题成功，关闭交互")]
        void CloseLantern()
        {
            currentAnimator = transform.GetComponentInChildren<Animator>();
            currentAnimator.SetInteger(aniPara, 1);
            buttonGuess.gameObject.SetActive(false);
            theCollider.enabled = false;

        }

        #endregion


        public IArchitecture GetArchitecture()
        {
            return WeiXiangArchitecture.Interface;
        }


    }

    [System.Serializable]
    public class MiYu
    {
        public string id;
        public string description;
        public string puzzle;
        public string answer;
        public string imageUrl;
        public string createTime;
        public string updateTime;
    }

    //开始猜谜的事件
    public class GuessEventOpenUi
    {
        public MiYu miyu;
    }

    //猜谜成功的事件
    public class GuessMiyuComplete
    {
        public MiYu miyu;
    }
}