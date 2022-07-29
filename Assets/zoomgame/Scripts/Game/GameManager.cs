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
        [SerializeField] private AudioClip success, fail,hit;

        private void Start()
        {
            Console.Warning("Console init complete");

            this.SendCommand<StartLocationCommand>();
            this.RegisterEvent<LocationResponseEvent>(OnFinishRequest).UnRegisterWhenGameObjectDestroyed(gameObject);

            NRInput.SetInputSource(InputSourceEnum.Controller);

            var origin = LocalizationConvert.Origin;

            this.RegisterEvent<PlayAudioEvent>(OnFinish);

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

        private void OnFinish(PlayAudioEvent obj)
        {
            if (obj.success)
            {
                AudioSource.PlayClipAtPoint(success,Camera.main.transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(hit, Camera.main.transform.position);
            }
        }
        

        void OnInterval(long _)
        {
            AudioSource.PlayClipAtPoint(fail,Vector3.zero);
            // 抽帧定位
            this.SendCommand<CaptureAndLocationCommand>();
        }

        [Button]
        void TestLocation()
        {
            this.SendCommand<CaptureAndLocationCommand>();
        }

    }
}