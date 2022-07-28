using System;
using System.Linq;
using NRKernal;
using NRKernal.Record;
using QFramework;
using UnityEngine;

namespace WeiXiang
{
    public interface ICanCapturePhoto :IUtility
    {
        /// <summary>
        /// 抽帧的最大分辨率
        /// </summary>
        Resolution Resolution { get; }
        /// <summary>
        /// 抽帧
        /// </summary>
        void TakePhoto(Action<bool,byte[]> callback);
    }
    
    /// <summary>
    /// PhotoCaptureExample
    /// </summary>
    public class PhotoCapture : ICanCapturePhoto
    {
        /// <summary> The photo capture object. </summary>
        private NRPhotoCapture _photoCaptureObject;
        /// <summary> The camera resolution. </summary>
        private Resolution _cameraResolution;
        private bool _isOnPhotoProcess = false;
        public Resolution Resolution => _cameraResolution;
        
        
        public void TakePhoto(Action<bool, byte[]> callback)
        {
            if (_isOnPhotoProcess)
            {
                Console.Warning("Currently in the process of taking pictures, Can not take photo .");
                return;
            }

            _isOnPhotoProcess = true;
            if (_photoCaptureObject == null)
            {
                this.Create((capture) =>
                {
                    capture.TakePhotoAsync((result, frame) =>
                    {
                        callback?.Invoke(result.success,frame.Data);
                        this.Close();
                    });
                });
            }
            else
            {
                _photoCaptureObject.TakePhotoAsync((r, f) =>
                {
                    callback.Invoke(r.success,f.Data);
                    Close();
                });
            }
        }

        /// <summary> Executes the 'captured photo memory' action. </summary>
        /// <param name="result">            The result.</param>
        /// <param name="photoCaptureFrame"> The photo capture frame.</param>
        // void OnCapturedPhotoToMemory(NRPhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        // {
        //     var targetTexture = new Texture2D(m_CameraResolution.width, m_CameraResolution.height);
        //     // Copy the raw image data into our target texture
        //     photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        //     
        //
        //     Release camera resource after capture the photo.
        //     this.Close();
        // }
        
        /// <summary> Use this for initialization. </summary>
        void Create(Action<NRPhotoCapture> onCreated)
        {
            if (_photoCaptureObject != null)
            {
                Console.Log("The NRPhotoCapture has already been created.");
                return;
            }

            // Create a PhotoCapture object
            NRPhotoCapture.CreateAsync(false, delegate (NRPhotoCapture captureObject)
            {
                _cameraResolution = NRPhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

                if (captureObject == null)
                {
                    NRDebugger.Error("Can not get a captureObject.");
                    return;
                }

                _photoCaptureObject = captureObject;

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.cameraResolutionWidth = _cameraResolution.width;
                cameraParameters.cameraResolutionHeight = _cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
                cameraParameters.frameRate = NativeConstants.RECORD_FPS_DEFAULT;
                cameraParameters.blendMode = BlendMode.Blend;

                // Activate the camera
                _photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (NRPhotoCapture.PhotoCaptureResult result)
                {
                    Console.Log("Start PhotoMode Async");
                    if (result.success)
                    {
                        onCreated?.Invoke(_photoCaptureObject);
                    }
                    else
                    {
                        _isOnPhotoProcess = false;
                        this.Close();
                        WeiXiang.Console.Error("Start PhotoMode faild." + result.resultType);
                    }
                }, true);
            });
        }
        
        /// <summary> Closes this object. </summary>
        void Close()
        {
            if (_photoCaptureObject == null)
            {
                Console.Error("The NRPhotoCapture has not been created.");
                return;
            }
            // Deactivate our camera
            _photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        
        
        /// <summary> Executes the 'stopped photo mode' action. </summary>
        /// <param name="result"> The result.</param>
        void OnStoppedPhotoMode(NRPhotoCapture.PhotoCaptureResult result)
        {
            // Shutdown our photo capture resource
            _photoCaptureObject?.Dispose();
            _photoCaptureObject = null;
            _isOnPhotoProcess = false;
        }
        
        /// <summary> Executes the 'destroy' action. </summary>
        void OnDestroy()
        {
            // Shutdown our photo capture resource
            _photoCaptureObject?.Dispose();
            _photoCaptureObject = null;
        }

    }

}