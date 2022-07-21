using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;
using UnityEngine.Events;
using LitJson;
using UnityEngine.UI;
using System.IO;

namespace AppLogic
{
    public class ZhiNengDaoButtonCtrl : MonoBehaviour
    {
        public Transform nextPoint;
        public Transform finalPoint;

        public Image imageDetail;


        public void PlayTheFinalAudioButtonClicked()
        {
            Debug.Log("PlayTheFinalAudioButtonClicked...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHINENGDAO_MOVE_TO_NEXT, nextPoint, transform);

            //PlayFinalGameobjectAudio(transform);
        }

        void PlayFinalGameobjectAudio(Transform trans)
        {
            AudioSource audio = trans.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.Stop();
            audio.Play();
            StartCoroutine(AudioFinalGuidePlayFinished(audio.clip.length, callback));

        }

        private IEnumerator AudioFinalGuidePlayFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHINENGDAO_MOVE_TO_NEXT, finalPoint);


            #endregion

        }

        public void PlayAudioByButtonClicked()
        {
            Debug.Log("PlayAudioByButtonClicked...");
            AppFacade.GetInstance().dispatchEvent(ModuleEventType.ZHINENGDAO_MOVE_TO_NEXT, nextPoint, transform);

            //PlayGameobjectAudio(transform);
        }
        void PlayGameobjectAudio(Transform trnas)
        {
            AudioSource audio = trnas.GetComponent<AudioSource>();
            if (audio == null) return;
            audio.Stop();
            audio.Play();
            StartCoroutine(AudioGuidePlayFinished(audio.clip.length, callback));
        }

        private void callback()
        {
        }

        private IEnumerator AudioGuidePlayFinished(float time, UnityAction callback)
        {

            yield return new WaitForSeconds(time);

            //声音播放完毕后之下往下的代码 

            #region  声音播放完成后执行的代码

            print("声音播放完毕，继续向下执行");


            #endregion

        }

        public void SendImageDetail()
        {
            Sprite sprite = imageDetail.sprite;
            Texture2D texture = SpriteToTexture2D(sprite);
            if (texture == null) return;
            string base64 = UnityUtilties.Texture2DToBase64(texture);
            if (base64 == null) return;
            Debug.Log("base64" + base64);
            OpenUrlFromUnityClicked("4", base64);
        }

        public Texture2D SpriteToTexture2D(Sprite sprite)
        {
            //sprite为图集中的某个子Sprite对象
            var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height);
            targetTex.SetPixels(pixels);
            targetTex.Apply();
            return targetTex;
        }
        public void OpenUrlADA()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/ada.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207846&Signature=4uCxjArBXW9qj5nYEb9kDKYzRjc%3D");
            OpenUrlFromUnityClicked("2", "DAD.png");

        }

        public void OpenUrlYuanJing()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E8%BF%9C%E6%99%AF%E6%99%BA%E8%83%BD.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670481009&Signature=uOtQ0qC%2FcZAVw3dwsfAR8L4Ax%2FY%3D");
            OpenUrlFromUnityClicked("2", "YuanJing.png");

        }

        public void OpenUrlMicrosoft()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%BE%AE%E8%BD%AF.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670482420&Signature=ZaVPNr3VOPTRryeUc7OaD530mHI%3D");
            OpenUrlFromUnityClicked("2", "Microsoft.png");

        }

        public void OpenUrlXiaoYi()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%B0%8F%E8%9A%81%E7%A7%91%E6%8A%80.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207927&Signature=dFCgJU1JjMGDnPanhBs479pkgP8%3D");
            OpenUrlFromUnityClicked("2", "XiaoYi.png");

        }

        public void OpenUrlBaiDu()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E7%99%BE%E5%BA%A6.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207948&Signature=aCncFRTGzKWWSaAnPAORgj8xQQQ%3D");
            OpenUrlFromUnityClicked("2", "BaiDu.png");

        }

        public void OpenUrlHongShan()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E7%BA%A2%E6%9D%89.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207966&Signature=QbAnGfG3JjIqeTh8kvAQbzb1FNQ%3D");
            OpenUrlFromUnityClicked("2", "HongShan.png");

        }

        public void OpenUrlTongJiB4()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%90%8C%E6%B5%8E%E5%A4%A7%E5%AD%A6B4.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207993&Signature=VgMqZolr%2F35UWvbftFWlj92OmhA%3D");
            OpenUrlFromUnityClicked("2", "TongJiB4.png");

        }

        public void OpenUrlTongJiB5()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%90%8C%E6%B5%8E%E5%A4%A7%E5%AD%A6B5.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670208015&Signature=ZG2y4Mxyeu6UsJkdPlYZ5Dhv%2Buc%3D");
            OpenUrlFromUnityClicked("2", "TongJiB5.png");

        }

        public void OpenUrlYunCong()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E4%BA%91%E4%BB%8E.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670207509&Signature=DL8faMbrZ%2B3akifuPA%2F2D33D1r8%3D");
            OpenUrlFromUnityClicked("2", "YunCong.png");

        }

        public void OpenUrlIBM()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/IBM%E5%A4%A7%E6%A5%BC.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669948201&Signature=Wrhir8eSkK9CAAW%2FNAkt7RseWJA%3D");
            OpenUrlFromUnityClicked("2", "IBM.png");

        }

        public void OpenUrlLianReng()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E8%81%94%E4%BB%81%E5%81%A5%E5%BA%B7.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1670480769&Signature=Sds91A3H7gd7l%2BDH80sxRCtGc7A%3D");
            OpenUrlFromUnityClicked("2", "LianReng.png");

        }

        public void OpenUrlALi()
        {
            //OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%B9%B3%E5%A4%B4%E5%93%A5.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669987747&Signature=3YE5sZu4L63cHJt7Sg4d26p%2FXME%3D");
            OpenUrlFromUnityClicked("2", "ALi.png");

        }

        public void OpenUrl1()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E6%97%A0%E4%BA%BA%E6%9C%BA.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669947700&Signature=ppRIp4BaRsJKbDHG3CBNF1OCWUU%3D");

        }

        public void OpenUrl2()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E6%97%A0%E4%BA%BA%E8%BD%A6.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669947735&Signature=LUYbTuEbFi9KAmQBtMtHVGr3mDI%3D");

        }

        public void OpenUrl3()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E5%9E%83%E5%9C%BE%E6%A1%B6.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669947755&Signature=A6PDtJS9dtZ2NUQ1UJcQ0h0%2B7kk%3D");

        }

        public void OpenUrl4()
        {
            OpenUrlFromUnityClicked("2", "https://zjar-rawdata.s3.cn-north-1.jdcloud-oss.com/picture/zjrgznd/%E6%99%BA%E8%83%BD%E7%81%AF%E6%9D%86.png?AWSAccessKeyId=D9CCB23C99FCD04A1195FAA92BF26B86&Expires=1669947774&Signature=ywN1zYC8%2F17FTWAcbwa%2BMdjJQso%3D");

        }
        public void OpenUrlFromUnityClicked(string type, string url)
        {
            Debug.Log("OpenUrlFromUnityClikd...");
            UrlFromUnity urlFromUnity = new UrlFromUnity();
            //type 2ΪͼƬ
            urlFromUnity.type = type;

            //urlFromUnity.url = "http://www.wayz.ai/";
            urlFromUnity.url = url;
            string json = JsonMapper.ToJson(urlFromUnity);
            Debug.Log(json);
            SDKManager.PhoneMehtodForOpenUrlFromUnity(json);
        }

        //点击后显示子物体
        public void Button009OnClicked()
        {
            Debug.Log("clicked...");
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void ButtonHideAParent()
        {
            Debug.Log("clicked...");
            transform.parent.parent.parent.gameObject.SetActive(false);
        }
    }
}