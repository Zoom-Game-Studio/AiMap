using System;
using Architecture;
using Architecture.Command;
using Architecture.TypeEvent;
using NRKernal;
using QFramework;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Waku.Module;
using zoomgame.Scripts.Architecture.Query;

namespace WeiXiang
{
    public class GameManager : AbstractMonoController
    {
        [SerializeField] private Text placeTip;
        [SerializeField] private GameObject infoTip;
        [SerializeField] private AudioClip intervalClip;
        [Header("定位接口设定")]
        [SerializeField, TextArea] private string token = "5bc251ab113f1510e3e1509b2442d52b";
        [SerializeField, TextArea] private string url = @"http://dev-hdmap.newayz.com:8800/wayzoom/v1/vps/single";
        [Header("抽帧定位，参数设定")]
        [SerializeField,LabelText("截图分辨率")] private CaptureResolution captureResolution = new CaptureResolution()
        {
            width = 640,
            height = 360,
        };

        [SerializeField,LabelText("相机内参")] private Intrinsic intrinsic = new Intrinsic()
        {
            px = 774,
            py = 774,
            fx = 320,
            fy = 180,
        };

        private bool isCaptureComplete = true;

        private BoolReactiveProperty captureCD = new BoolReactiveProperty(true);

        [Header("模型加载设定")] [LabelText("根据GPS坐标加载模型")]
        public bool loadByGps = true;
        [LabelText("覆盖高德的GPS坐标")] public bool overrideGps = false;
        public Vector2 overrideGpsPostion = new Vector2
        {
            x = 121.60449397773354f,
            y = 31.17980398552158f,
        };
        [LabelText("不根据GPS坐标加载模型，加载指定Ab包")] public string constTileInfoId;


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

        private BoolReactiveProperty _click = new BoolReactiveProperty();

        private void Start()
        {
            Console.Warning("[Console] init complete");
            NRInput.SetInputSource(InputSourceEnum.Controller);
            NRInput.RaycastMode = RaycastModeEnum.Laser;
            this.SendCommand(new SetVisLocationModel()
            {
                token = token,
                url = url,
                intrinsic = intrinsic,
                captureResolution = captureResolution,
            });
            this.SendCommand<StartLocationCommand>();
            this.RegisterEvent<CaptureFinishEvent>(OnFinishCapture).UnRegisterWhenGameObjectDestroyed(gameObject);
            var origin = LocalizationConvert.Origin;
            // Observable.Interval(TimeSpan.FromSeconds(5f)).Subscribe(OnInterval).AddTo(this);
            // Invoke(nameof(DownloadAssetBundleAndBuild), 2);
            NRInput.SetInputSource(InputSourceEnum.Controller);

            MessageBroker.Default.Receive<BuildTileEvent>().Subscribe(OnBuild);

            var clickStream = Observable.EveryUpdate().Where(_ => NRInput.GetButtonDown(ControllerButton.TRIGGER));
            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(xs => OnDoubleClick()).AddTo(this);
        }

        private void OnDoubleClick()
        {
            OnInterval(0);
        }

        void OnBuild(BuildTileEvent evt)
        {
            placeTip.text = $"{evt.place}:{evt.name}";
        }

        /// <summary>
        ///  按下进行抽帧定位
        /// </summary>
        private void Update()
        {
            if (NRInput.GetButtonDown(ControllerButton.TRIGGER) || Input.GetKeyDown(KeyCode.Space))
            {
                // OnInterval(0);
                _click.Value = true;
            }

            {
                _click.Value = false;
            }
        }

        private void OnFinishCapture(CaptureFinishEvent obj)
        {
            isCaptureComplete = true;
        }

        void OnInterval(long _)
        {
            if (isCaptureComplete && captureCD)
            {
                captureCD.Value = false;
                isCaptureComplete = false;
                captureCD.Where(v => !v).Throttle(TimeSpan.FromSeconds(5)).Subscribe(_ => captureCD.Value = true);
                AudioSource.PlayClipAtPoint(intervalClip, Camera.main ? Camera.main.transform.position : Vector3.zero);
                infoTip.SetActive(true);
                // 抽帧定位
                this.SendCommand<CaptureAndLocationCommand>();
                // var mat = NRFrame.GetRGBCameraIntrinsicMatrix();
                // Debug.Log("GetRGBCameraIntrinsicMatrix" + mat.ToString());
            }
            else
            {
                WeiXiang.Console.Warning(
                    $"Capture has not complete! isCaptureComplete:{isCaptureComplete},captureCD{captureCD}");
            }
        }

        /// <summary>
        /// 下载并执行场景构建
        /// </summary>
        void DownloadAssetBundleAndBuild()
        {
            if (loadByGps)
            {
                var gps = overrideGps ? overrideGpsPostion : this.SendQuery(new QueryGpsCommand());
                var amap = this.GetArchitecture().GetUtility<IAmap>();
                if (AssetDownloader.Instance && (amap.IsRunning || overrideGps))
                {
                    var success = AssetDownloader.Instance.TryBuildAssetByGps(gps);
                    if (!success)
                    {
                        Invoke(nameof(DownloadAssetBundleAndBuild), 2);
                    }
                    else
                    {
                        Debug.LogWarning("tile build done ,cancel invoke!");
                    }
                }
            }
            else
            {
                if (AssetDownloader.Instance)
                {
                    var info = AssetDownloader.Instance.ServerList.Find(e=> e.id.Equals(constTileInfoId));
                    if (info != null)
                    {
                        AssetDownloader.Instance.AddToDownloadList(info);
                    }
                }
            }

        }
    }

    public class BuildTileEvent
    {
        public string place;
        public string name;
    }
}