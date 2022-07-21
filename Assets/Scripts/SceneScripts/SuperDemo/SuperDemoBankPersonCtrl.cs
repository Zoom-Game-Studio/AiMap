using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
namespace AppLogic
{
    public class SuperDemoBankPersonCtrl : MonoBehaviour
    {
        public AudioClip[] audios;
        private float speed = 1;
        private bool isArrived = true;
        private int currentWayPoint = 0;
        private bool isInit = false;
        public List<GameObject> waypointsList;
        private LookAtCamera lookatCamera;
        public GameObject menu1;
        public GameObject menu2;
        GameObject nextWayPoint;
        AudioClip nextClip;
        private void Awake()
        {
            Debug.Log("BankPersonController Start...");
            lookatCamera = GetComponent<LookAtCamera>();
            AppFacade.GetInstance().addEvent(ModuleEventType.SUPERDEMO_SHOW_BANK_MENU, HandleShowBankMenu);
        }

        private void Menu2OnClicked()
        {
            Debug.Log("Menu2OnClicked");
            menu1.gameObject.SetActive(false);
            menu2.gameObject.SetActive(false);
            nextWayPoint = waypointsList[2];
            nextClip = audios[2];
            AudioSource audio = GetComponent<AudioSource>();
            if (nextClip != null)
            {
                audio.clip = nextClip;
                PlayAudio(audio);
            }
            StartMove();
        }

        private void Menu1OnClicked()
        {
            Debug.Log("Menu1OnClicked");
            menu1.gameObject.SetActive(false);
            menu2.gameObject.SetActive(false);
            nextWayPoint = waypointsList[1];
            nextClip = audios[1];
            AudioSource audio = GetComponent<AudioSource>();
            if (nextClip != null)
            {
                audio.clip = nextClip;
                PlayAudio(audio);
            }
            StartMove();
        }

        void HandleShowBankMenu(EventObject ev)
        {
            Debug.Log("HandleShowBankMenu...");
            menu1.gameObject.SetActive(true);
            menu2.gameObject.SetActive(true);
            Button menu1Button = menu1.GetComponentInChildren<Button>();
            if (menu1Button != null) { menu1Button.onClick.AddListener(Menu1OnClicked); }
            Button menu2Button = menu2.GetComponentInChildren<Button>();
            if (menu2Button != null) { menu2Button.onClick.AddListener(Menu2OnClicked); }
            AudioSource audio = transform.GetComponent<AudioSource>();
            //audio.clip = audios[1];
            //PlayAudio(audio, CallBack1);
        }

        private void CallBack1()
        {
            Debug.Log("CallBack1...");
        }

        void StartMove()
        {
            Debug.Log("StartMove...");
            SetAnimatorPara(0);
            isArrived = false;
            if (lookatCamera == null) return;
            lookatCamera.enabled = false;
        }
        public void PlayAudio(AudioSource audio, UnityAction callback = null)
        {
            //获取自身音频文件进行播放并且不重复播放
            audio.Play();
            //执行协成获取音频文件的时间
            StartCoroutine(AudioFinished(audio.clip.length, callback));
        }
        private IEnumerator AudioFinished(float time, UnityAction callback)
        {
            yield return new WaitForSeconds(time);
            //声音播放完毕后之下往下的代码
            #region  声音播放完成后执行的代码
            print("声音播放完毕，继续向下执行");
            if (callback != null)
            {
                callback();
            }

            #endregion
        }

        void MoveToNextPoint()
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWayPoint.transform.position, step);
            transform.LookAt(nextWayPoint.transform.position);

            if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
            {
                Debug.Log("isArrived...");
                isArrived = true;
                {
                    //AudioSource audio = GetComponent<AudioSource>();
                    //if(nextClip!=null)
                    //{
                    //    audio.clip = nextClip;
                    //    PlayAudio(audio);
                    //}
                    lookatCamera.enabled = true;
                    StartCoroutine(BackToBar());
                }
                SetAnimatorPara(1);
            }
        }

        IEnumerator BackToBar()
        {
            yield return new WaitForSeconds(10f);
            gameObject.transform.position = waypointsList[0].transform.position;
            lookatCamera.enabled = true;
            SetAnimatorPara(1);
            menu1.SetActive(true);
            menu2.SetActive(true);
        }

        void SetAnimatorPara(int para)
        {
            Debug.Log("setpara:" + para);
            Animator ani = GetComponent<Animator>();
            if (ani == null) return;
            ani.SetInteger("Talk", para);
        }
        private void Update()
        {
            if (isArrived) return;
            MoveToNextPoint();
        }
    }

}