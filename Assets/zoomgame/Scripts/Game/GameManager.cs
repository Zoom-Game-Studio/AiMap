using System;
using Architecture;
using Architecture.Command;
using Architecture.TypeEvent;
using HttpData;
using NRKernal;
using QFramework;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace WeiXiang
{
    public class GameManager : AbstractMonoController
    {
        [SerializeField] private AudioClip intervalClip;
        [SerializeField,TextArea] private string token;
        [SerializeField,TextArea] private string url;
        [SerializeField] private CaptureResolution captureResolution;
        [SerializeField] private Intrinsic intrinsic;

        [System.Serializable]
        public class Intrinsic
        {
            public float px, py, fx, fy;
        }

        [System.Serializable]
        public class CaptureResolution
        {
            public int width, height;
        }

        private void Start()
        {
            Console.Warning("[Console] init complete");
            this.SendCommand(new SetVisLocationModel()
            {
                token = token,
                url = url,
                intrinsic = intrinsic,
                captureResolution = captureResolution,
            });
            this.SendCommand<StartLocationCommand>();
            this.RegisterEvent<LocationResponseEvent>(OnFinishRequest).UnRegisterWhenGameObjectDestroyed(gameObject);
            NRInput.SetInputSource(InputSourceEnum.Controller);
            var origin = LocalizationConvert.Origin;
            Observable.Interval(TimeSpan.FromSeconds(5f)).Subscribe(OnInterval).AddTo(this);
        }

        /// <summary>
        /// 定位请求结束
        /// </summary>
        /// <param name="obj"></param>
        private void OnFinishRequest(LocationResponseEvent obj)
        {
            if (obj.isFinish && obj.isSuccess)
            {
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<LocationResponse>(obj.data);
                //this.SendCommand(new LoadAssetBundleModelCommand(response.maptile_name));
            }
            else
            {
                Console.Error("Cant get LocationResponseEvent or location fail");
            }
        }


        

        void OnInterval(long _)
        {
            AudioSource.PlayClipAtPoint(intervalClip,Vector3.zero);
            // 抽帧定位
            this.SendCommand<CaptureAndLocationCommand>();
        }

        [Button("CaptureAndLocationCommand")]
        void TestLocation()
        {
            this.SendCommand<CaptureAndLocationCommand>();
        }
    }
}