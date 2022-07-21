/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples
{

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System;

    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public class CameraPreview : MonoBehaviour
    {
        public WebCamTexture cameraTexture { get; private set; }
        public int width
        {
            get
            {
                bool isPortrait = cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270;
                return isPortrait ? cameraTexture.height : cameraTexture.width;
            }
        }
        public int height
        {
            get
            {
                bool isPortrait = cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270;
                return isPortrait ? cameraTexture.width : cameraTexture.height;
            }
        }

        public bool useFrontCamera;
        //public MirrorFlipCamera mirrorFlipCamera;

        private RawImage rawImage;
        private AspectRatioFitter aspectFitter;

        void Start()
        {
            //AREventUtil.AddListener(GlobalOjbects.SWITCH_CAMERA, SwitchCameraHandler);
            //AREventUtil.AddListener(GlobalOjbects.LOCK_SCREEN, LockScreenHandler);
            StartCoroutine(StartCamera());
        }

        private void LockScreenHandler(AREventArgs ev)
        {
            print("LockScreenHandler...");
            if (ev == null) return;
            if (ev.args[0] == null) return;
            bool isLock = (bool)ev.args[0];
            if (isLock)
                PauseDefaultCamera();
            else
                PlayDefaultCamera();
                
        }

        private void OnDestroy()
        {
            //AREventUtil.RemoveListener(GlobalOjbects.SWITCH_CAMERA, SwitchCameraHandler);
        }
        private void SwitchCameraHandler(AREventArgs ev)
        {
            print("SwitchCameraHandler...");
            if (ev == null) return;
            if (ev.args[0] == null) return;
            bool isFront = (bool)ev.args[0];
            StopDefaultCamera();
            useFrontCamera = isFront;
            StartCoroutine(StartCamera());
            transform.localScale = new Vector3(isFront ? -1 : 1, 1, 1);
        }

        public IEnumerator StartCamera()
        {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
            // Request microphone and camera
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) yield break;
            // Start the WebCamTexture
            string cameraName = null;
            foreach (var camera in WebCamTexture.devices)
            {
                if (useFrontCamera && camera.isFrontFacing)
                {
                    cameraName = camera.name;
                    break;
                }
            }
            cameraTexture = new WebCamTexture(cameraName, 1280, 720);
            cameraTexture.Play();
            yield return new WaitUntil(() => cameraTexture.width != 16 && cameraTexture.height != 16); // Workaround for weird bug on macOS
            // Borrow the GreyWorld shader because it supports rotation and mirroring
            rawImage.texture = cameraTexture;
            rawImage.material = new Material(Shader.Find("Hidden/NatCorder/GreyWorld"));
            // Orient the preview panel
            rawImage.material.renderQueue = 1999;
            rawImage.color = Color.white;
            rawImage.material.SetFloat("_Rotation", cameraTexture.videoRotationAngle * Mathf.PI / 180f);

            rawImage.material.SetFloat("_Scale", cameraTexture.videoVerticallyMirrored ? -1 : 1);
            // Scale the preview panel
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            if (cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270)
                aspectFitter.aspectRatio = (float)cameraTexture.height / cameraTexture.width;
            else
                aspectFitter.aspectRatio = (float)cameraTexture.width / cameraTexture.height;
        }

       public void test()
        {
            
        }

        public void PlayDefaultCamera()
        {
            if (cameraTexture != null)
                cameraTexture.Play();
        }

        public void PauseDefaultCamera()
        {
            if(cameraTexture!=null)
            {
                cameraTexture.Pause();
            }
        }

        public void StopDefaultCamera()
        {
            if (cameraTexture != null)
                cameraTexture.Stop();
        }

        public void ResetDefaultCamera()
        {
            StopDefaultCamera();
            StartCoroutine(StartCamera());
        }

        private void OnEnable()
        {
            ResetDefaultCamera();
        }
    }
}