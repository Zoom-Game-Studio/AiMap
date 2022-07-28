using System;
using Architecture.TypeEvent;
using BestHTTP;
using QFramework;
using UnityEngine;
using WeiXiang;
using Console = WeiXiang.Console;

namespace Architecture.Command
{
    public class CaptureAndLocationCommand : AbstractCommand
    {
        private ICanCapturePhoto _capturePhoto;
        protected override void OnExecute()
        {
            _capturePhoto = this.GetUtility<ICanCapturePhoto>();
            _capturePhoto?.TakePhoto(OnCaptureComplete);
        }

        void OnCaptureComplete(bool success, byte[] data)
        {
            if (!success)
            {
                Console.Error("CaptureAndLocation :  Capture photo fail!");
                return;
            }
            try
            {
                if (data != null)
                {
                    var now = DateTime.Now.Ticks;
                    var location = this.GetUtility<ICanLocation>();
                    //记录截图时的相机姿态
                    var capturePose = new GameObject().AddComponent<CapturePoseNode>();
                    var main = Camera.main.transform;
                    capturePose.transform.position = main.position;
                    capturePose.transform.rotation = main.rotation;
                    capturePose.ticks = now;
                    location.RequestLocation(Convert.ToBase64String(data),(state,dataStr)=>
                    {
                        this.SendEvent(new LocationResponseEvent()
                        {
                            state = state,
                            ticks = now,
                            data = dataStr
                        });
                    });
                }
                else
                {
                    Console.Error("Capture ,but cant get data!");
                }
            }
            catch (Exception e)
            {
                Console.Error(e.Message);
                Console.Error(e.StackTrace);
            }
        }
    }
}