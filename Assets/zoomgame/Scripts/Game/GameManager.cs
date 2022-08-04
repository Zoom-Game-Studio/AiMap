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
using Waku.Module;
using zoomgame.Scripts.Architecture.Query;

namespace WeiXiang
{
    public class GameManager : AbstractMonoController
    {
        [SerializeField] private AudioClip intervalClip;
        [SerializeField, TextArea] private string token ="5bc251ab113f1510e3e1509b2442d52b";
        [SerializeField, TextArea] private string url = @"http://dev-hdmap.newayz.com:8800/wayzoom/v1/vps/single";
        [SerializeField] private CaptureResolution captureResolution = new CaptureResolution()
        {
            width = 640,
            height = 360,
        };
        [SerializeField] private Intrinsic intrinsic = new Intrinsic()
        {
            px = 774,
            py = 774,
            fx = 320,
            fy = 180,
        };
        [SerializeField] private bool isCaptureComplete = true;

        [Header("覆盖高德的GPS坐标")] public bool overrideGps = false;
        public Vector2 overrideGpsPostion = new Vector2
        {
            x =121.60449397773354f,
            y = 31.17980398552158f,
        };

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
            this.RegisterEvent<CaptureFinishEvent>(OnFinishCapture).UnRegisterWhenGameObjectDestroyed(gameObject);
            NRInput.SetInputSource(InputSourceEnum.Controller);
            var origin = LocalizationConvert.Origin;
            Observable.Interval(TimeSpan.FromSeconds(5f)).Subscribe(OnInterval).AddTo(this);
            Invoke(nameof(DownloadAssetBundleAndBuild),2);
        }

        private void OnFinishCapture(CaptureFinishEvent obj)
        {
            isCaptureComplete = true;
        }

        void OnInterval(long _)
        {
            if (isCaptureComplete)
            {
                AudioSource.PlayClipAtPoint(intervalClip, Vector3.zero);
                // 抽帧定位
                this.SendCommand<CaptureAndLocationCommand>();
                isCaptureComplete = false;
            }
            else
            {
                WeiXiang.Console.Warning("Capture has not complete!");
            }
        }

        /// <summary>
        /// 下载并执行场景构建
        /// </summary>
        void DownloadAssetBundleAndBuild()
        {
            var gps = overrideGps ? overrideGpsPostion : this.SendQuery(new QueryGpsCommand());
            if (AssetDownloader.Instance)
            {
                var success = AssetDownloader.Instance.TryBuildAssetByGps(gps);
                if (!success)
                {
                    Invoke(nameof(DownloadAssetBundleAndBuild),2);
                }
                else
                {
                    Debug.LogWarning("tile build done ,cancel invoke!");
                }
            }
        }
        
    }
}