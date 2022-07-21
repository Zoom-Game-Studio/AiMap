using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.UI;
using LitJson;

namespace AppLogic
{
    public class SuperDemoButtonCtrl : MonoBehaviour
    {
        private bool isShow = false;

        public void PlayTransFormAudio()
        {
            AudioSource audio = transform.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.Stop();
            audio.Play();
        }

        public void SwitchChildActive()
        {
            isShow = !isShow;
            transform.GetChild(0).gameObject.SetActive(isShow);
        }

        //todo 改名
        //点击后显示子物体
        public void Button009OnClicked()
        {
            Debug.Log("clicked...");
            transform.GetChild(0).gameObject.SetActive(true);
        }

        //点击后隐藏子物体
        public void ButtonHideChild()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void SetGameObjectScale()
        {
            transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
        }

        public void ButtonShowChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                if (child == null) continue;

                child.gameObject.SetActive(true);
            }

        }

        //todo 改名
        /// <summary>
        /// 点击后播放视频
        /// </summary>
        public void Button004OnClicked()
        {
            Transform parent = transform.parent;
            if (parent == null) return;
            Transform videoObj = parent.Find("VideoPlayer");
            if (videoObj == null) return;
            videoObj.gameObject.SetActive(true);
        }

        public void TaskReportButton1Clicked()
        {

            Transform parent = transform.parent;
            if (parent == null) return;
            Transform button1Obj = parent.Find("Button1");
            if (button1Obj == null) return;
            Image image1 = button1Obj.GetComponent<Image>();
            if (image1 == null) return;
            image1.color = new Color(1, 1, 1, 1);
            Transform button0Obj = parent.Find("Button0");
            if (button0Obj == null) return;
            Image image0 = button0Obj.GetComponent<Image>();
            if (image0 == null) return;
            image0.color = new Color(1, 1, 1, 0);

        }

        public void SuperDemoCoffeButtonClick()
        {
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.SUPERDEMO_COFFE_BUTTON_CTRL);
        }
        Animation ani;
        public void AddFavouritePlay()
        {
            ani = transform.GetComponentInChildren<Animation>();
            if (ani == null) return;
            Debug.Log(ani.transform.name);
            ani.enabled = false;
            ani.enabled = true;
            ani.Play();
        }

        public void AddFavouritePlayComplete()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child == null) continue;
                if (child.name.Equals("Blow")) continue;
                child.gameObject.SetActive(true);
            }
            if (ani == null) return;
            ani.enabled = false;

            GameObject blowObj = UnityUtilties.FindChild(transform, "Blow");
            if (blowObj == null) return;
            GameObject firework = Instantiate<GameObject>(blowObj);

            firework.transform.SetParent(transform, true);
            firework.transform.localPosition = blowObj.transform.localPosition;
            firework.transform.localRotation = blowObj.transform.localRotation;
            firework.transform.localScale = blowObj.transform.localScale;
            firework.SetActive(true);
            firework.AddComponent<SuperDemoDelayDestroy>();
        }



        public void TaskReportButton0Clicked()
        {
            Transform parent = transform.parent;
            if (parent == null) return;
            Transform button1Obj = parent.Find("Button1");
            if (button1Obj == null) return;
            Image image1 = button1Obj.GetComponent<Image>();
            if (image1 == null) return;
            image1.color = new Color(1, 1, 1, 0);
            Transform button0Obj = parent.Find("Button0");
            if (button0Obj == null) return;
            Image image0 = button0Obj.GetComponent<Image>();
            if (image0 == null) return;
            image0.color = new Color(1, 1, 1, 1);
        }

        public void OpenUrlFromUnityClicked()
        {
            Debug.Log("OpenUrlFromUnityClicked...");
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            urlFromUnity.type = "1";
            urlFromUnity.url = "http://www.wayz.ai/";
            string json = JsonMapper.ToJson(urlFromUnity);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }

        public void OpenHaiQuYuanUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E6%B5%B7%E8%B6%A3%E5%9B%AD%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231606&Signature=w5%2B6vyCPYLAOadKajd3Xufsua2Y%3D");
        }

        public void OpenJuXinAUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%82%AC%E5%88%9B%E8%8A%AFA%E5%BA%A7%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231664&Signature=BeEUGqgUsNO9djj2PU%2BjbjDD5%2Fs%3D");
        }

        public void OpenJuXinBUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%82%AC%E5%88%9B%E8%8A%AFB%E5%BA%A7%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231683&Signature=%2BCM4ouzpKXPWrb6kTSB66spIsdY%3D");
        }

        public void OpenBaiDuUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%99%BE%E5%BA%A6%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231750&Signature=h49CcvuYji2hIQkQMONwMs5DBHo%3D");
        }

        public void OpenKeHaiDaLouUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%A7%91%E6%B5%B7%E5%A4%A7%E6%A5%BC%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231785&Signature=8do6J8VWE5tZj3gzzRc%2BYbrbpNA%3D");
        }

        public void OpenKeXianYuanUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%A7%91%E8%B4%A4%E5%9B%AD%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231806&Signature=sQJIihhdWaw01osyw7TGPRcx7Cs%3D");
        }

        public void OpenLvDiUrlClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E7%BB%BF%E5%9C%B0%E7%BC%A4%E7%BA%B7%E5%B9%BF%E5%9C%BA%E8%AF%A6%E6%83%85%E5%8D%A1%E7%89%87.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1705231832&Signature=AfwaNCS7D6z9Vvo6jkd4Umwn%2B4A%3D");
        }

        public void OpenSomeDetailsClicked()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/superdemo/%E6%A5%BC%E5%AE%87%E4%BF%A1%E6%81%AF.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1708343226&Signature=8Y%2FY0l1FHflxZxLZOM20mmsf5pk%3D");
        }

        public void OpenUrlFromUnityClicked(string type,string url)
        {
            Debug.Log("OpenUrlFromUnityClicked...");
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            urlFromUnity.type = type;
            urlFromUnity.url = url;
            string json = JsonMapper.ToJson(urlFromUnity);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }
    }

}