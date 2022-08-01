using System;
using System.IO;
using System.Linq;
using NRKernal;
using NRKernal.Record;
using QFramework;
using UnityEngine;

namespace WeiXiang
{
    public interface ICanCapturePhoto : IUtility
    {
        /// <summary>
        /// 抽帧的最大分辨率
        /// </summary>
        Resolution CaptureResolution { get; }

        /// <summary>
        /// 抽帧
        /// </summary>
        void TakePhoto(Action<bool, byte[]> callback);
    }

    /// <summary>
    /// PhotoCaptureExample
    /// </summary>
    public class PhotoCapture : ICanCapturePhoto
    {
        /// <summary> The photo capture object. </summary>
        private NRPhotoCapture _photoCaptureObject;

        /// <summary> The camera resolution. </summary>
        // private Resolution _cameraResolution;
        private bool _isOnPhotoProcess = false;

        private ILocationModel _locationModel;
        public Resolution CaptureResolution => _locationModel.CaptureResolution;


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
                        SavePhoto(frame.Data);
                        callback?.Invoke(result.success, frame.Data);
                        this.Close();
                    });
                });
            }
            else
            {
                _photoCaptureObject.TakePhotoAsync((r, f) =>
                {
                    SavePhoto(f.Data);
                    callback.Invoke(r.success, f.Data);
                    Close();
                });
            }
        }

        /// <summary> Use this for initialization. </summary>
        void Create(Action<NRPhotoCapture> onCreated)
        {
            if (_photoCaptureObject != null)
            {
                Console.Log("The NRPhotoCapture has already been created.");
                return;
            }

            // Create a PhotoCapture object
            NRPhotoCapture.CreateAsync(false, delegate(NRPhotoCapture captureObject)
            {
                if (captureObject == null)
                {
                    NRDebugger.Error("Can not get a captureObject.");
                    return;
                }

                _photoCaptureObject = captureObject;

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.cameraResolutionWidth = CaptureResolution.width;
                cameraParameters.cameraResolutionHeight = CaptureResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
                cameraParameters.frameRate = NativeConstants.RECORD_FPS_DEFAULT;
                cameraParameters.blendMode = BlendMode.RGBOnly;

                // Activate the camera
                _photoCaptureObject.StartPhotoModeAsync(cameraParameters,
                    delegate(NRPhotoCapture.PhotoCaptureResult result)
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

        public void BindData(ILocationModel locationModel)
        {
            this._locationModel = locationModel;
        }

        void SavePhoto(byte[] data)
        {
            var path = Path.Combine(Application.persistentDataPath, "ScreenShot");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var now = DateTime.Now;
            var name = $"{now.Year}_{now.Month}_{now.Day}_{now.Hour}_{now.Minute}_{now.Second}_{now.Millisecond}";
            var fullPath = Path.Combine(path, name);
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                fs.Write(data);
            }
        }
    }
}