using Architecture;
using Architecture.Command;
using Architecture.TypeEvent;
using HttpData;
using NRKernal;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WeiXiang
{
    public class GameManager : AbstractMonoController
    {

        [SerializeField] private Transform debugAlign;

        [SerializeField] private AudioClip success, fail,hit;

        private void Start()
        {
            this.SendCommand<StartLocationCommand>();
            this.RegisterEvent<LocationResponseEvent>(OnFinishRequest).UnRegisterWhenGameObjectDestroyed(gameObject);

            NRInput.SetInputSource(InputSourceEnum.Controller);

            var origin = LocalizationConvert.Origin;

            this.RegisterEvent<PlayAudioEvent>(OnFinish);
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
                this.SendCommand(new LoadAssetBundleModelCommand(response.maptile_name));
            }
            else
            {
                Console.Error("cant get LocationResponseEvent");
            }
        }

        private void OnFinish(PlayAudioEvent obj)
        {
            if (obj.success)
            {
                AudioSource.PlayClipAtPoint(success,Vector3.zero);
            }
            else
            {
                AudioSource.PlayClipAtPoint(fail,Vector3.zero);
            }
        }
        
        private void Update()
        {
            if (NRInput.GetButtonDown(ControllerButton.TRIGGER))
            {
                AudioSource.PlayClipAtPoint(hit,Vector3.zero);

                // 抽帧定位
                this.SendCommand<CaptureAndLocationCommand>();
            }
        }

        [Button]
        void TestLocation()
        {
            this.SendCommand<CaptureAndLocationCommand>();
        }

    }
}