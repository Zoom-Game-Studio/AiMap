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
using System.IO;
using UnityEngine.UI;



namespace AppLogic
{
    public class TakePhotoMediator : BaseMediator
    {
        [Header("Recording")]
        public int videoWidth = 720;
        public int videoHeight = 1280;

        private IClock recordingClock;
        private CameraInput cameraInput;

        private TakePhotoView takePhotoView;
        private Sprite logoSprite;


        public Texture2D currentPhotoTexture;
        public string currentPhotoPath;



        public TakePhotoMediator()
        {
            m_mediatorName = MEDIATOR.RECORD;
        }

        protected override void onInit()
        {
            //takePhotoView = viewComponent as TakePhotoView;

        }

        /// <summary>
        /// 拍照按钮点击
        /// </summary>
        public void PhotoBtnOnClick()
        {
            Texture2D captureTexture = UnityUtilties.CaptureScreen(Camera.main, new Rect(0, 0, Screen.width, Screen.height));
            //AppManager.Instance.CaptureScreen(Camera.main, new Rect(0, 0, Screen.width, Screen.height));

            if (currentPhotoTexture != null)
            {
                Texture2D i = currentPhotoTexture;//截图
                Texture2D t;
                string bannerPath = PlayerPrefs.GetString("BannerPath");

                if (bannerPath != null && bannerPath.Equals(String.Empty) && logoSprite != null)
                {
                    int logoSpriteWidth = Screen.width;
                    int logoSpriteHeight = logoSprite.texture.height * Screen.width / logoSprite.texture.width;
                    Sprite tempSprite = UnityUtilties.GetSpriteFormPng(bannerPath, logoSpriteWidth, logoSpriteHeight);
                    if (tempSprite != null)
                    {
                        logoSprite = tempSprite;
                        Debug.Log("tempsprite is not null");
                    }
                    t = UnityUtilties.AddLogo(i, UnityUtilties.ReSetTextureSize(logoSprite.texture, logoSpriteWidth, logoSpriteHeight));
                    Debug.Log("Add Logo...====");
                }
                else
                    t = i;
                currentPhotoTexture = t;
                Texture texture = (Texture)t;
                //Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                if (texture != null)
                {
                    sendModuleEvent(ModuleEventType.MODULE_ENTER, MEDIATOR.PREVIEW);
                    sendModuleEvent(ModuleEventType.SET_PREVIEW_IMAGE, texture);
                }
            }
            else
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
            }
        }

        public void SaveCaptureScreen(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            //        string filename = Application.dataPath + "/ScreenShot.png";
            string filename = Application.persistentDataPath + "/ScreenShot.png";
            Debug.Log("save file to ====" + filename);
            currentPhotoPath = filename;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            System.IO.File.WriteAllBytes(filename, bytes);
        }

        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(takePhotoView.TakePhotoBtn))
            {
                PhotoBtnOnClick();
                Debug.Log("TakePhotoBtnclick");
            }
            else if (ev.param.Equals(""))
            {

            }
        }
    }
}
