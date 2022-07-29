using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Architecture;
using Architecture.Command;
using Architecture.TypeEvent;
using BestHTTP;
using DeveloperKit.Runtime.Pool;
using HttpData;
using Newtonsoft.Json;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using WeiXiang;

namespace C_ScriptsTest
{
    public class LocationTest : AbstractMonoController
    {
        public static LocationTest instance { private set; get; }
        private ICanLocation location;
        [SerializeField] private ComponentPool<LocationPointNode> pool;

        [Header("照片路径")] [SerializeField] private string path;
        [Header("图像类型")] [SerializeField] private string fileType = ".jpg";

        [Header("所有图像")] [SerializeField, TableList]
        private List<ImageItem> allImage;

        [TextArea] [SerializeField] private string currentFile;

        [Header("手机参数")]
        public Gps gps = Gps.ChengDu;
        public Orientation orientation = Orientation.left_h;

        public enum Gps
        {
            ShangHai9L,
            ChengDu,
        }

        public enum Orientation
        {
            /// <summary>
            /// 左横屏
            /// </summary>
            left_h = 1,
            /// <summary>
            /// 倒立竖屏
            /// </summary>
            inverse_v =2,
            /// <summary>
            /// 右横屏
            /// </summary>
            right_h = 3,
            /// <summary>
            /// 竖屏
            /// </summary>
            v = 4,
        }



        private void Start()
        {
            instance = this;
            this.location = this.GetArchitecture().GetUtility<ICanLocation>();
        }

        [Button("清除全部图像"), ButtonGroup("set")]
        void ClearAll()
        {
            allImage.Clear();
        }

        [Button("获取全部图像"), ButtonGroup("set")]
        void GetAllPicture()
        {
            var all = Directory.GetFiles(path).Where(e => e.EndsWith(fileType)).ToList();
            foreach (var n in all)
            {
                var item = new ImageItem();
                item.fullPath = n;
                allImage.Add(item);
            }
        }

        public void SelectItem(string obj)
        {
            currentFile = obj;
        }

        [Button("定位测试"), ButtonGroup("test")]
        void Location()
        {
            if (Application.isPlaying)
            {
                var image = DataMapper.LoadImageFromPath(currentFile);
                var gpsInfo = PriorTranslation.Default;
                switch (gps)
                {
                    case Gps.ShangHai9L:
                        gpsInfo = PriorTranslation.ShangHai;
                        break;
                    case Gps.ChengDu:
                        gpsInfo = PriorTranslation.ChengDu;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                var now = DateTime.Now.Ticks;
                //记录截图时的相机姿态
                var capturePose = new GameObject().AddComponent<CapturePoseNode>();
                var main = Camera.main.transform;
                capturePose.transform.position = main.position;
                capturePose.transform.rotation = main.rotation;
                capturePose.ticks = now;

                location.RequestLocation(image, Float4.intrinsic, gpsInfo,(int)orientation, (s, d) =>
                {
                    OnFinish(s,d);
                    this.GetArchitecture().SendEvent(new LocationResponseEvent()
                    {
                        data = d,
                        state = s,
                        ticks = now,
                    });
                });
            }
            else
            {
                Debug.LogError("进入运行模式才能进行测试");
            }
        }
        private void OnFinish(HTTPRequestStates state, string data)
        {
            Debug.Log(state);
            Debug.Log(data);

            if (state == HTTPRequestStates.Finished && !string.IsNullOrEmpty(data) && data.Contains("succeed"))
            {
                var locationInfo = DataMapper.ToLocation(data);
                var pose = LocalizationConvert.LocationToUnityPose(locationInfo);
                var pointer = pool.PopItem();
                pointer.transform.position = pose.pos;
                pointer.transform.rotation = pose.rot;
            }


        }

        void RunTest()
        {
            //记录截图时的相机姿态
            var capturePose = new GameObject().AddComponent<CapturePoseNode>();
            var main = Camera.main.transform;
            capturePose.transform.position = main.position;
            capturePose.transform.rotation = main.rotation;
            var now = DateTime.Now.Ticks;
            capturePose.ticks = now;
            var data = DataMapper.LoadImageFromPath(currentFile);

            location.RequestLocation(data, (states, dataStr) =>
            {
                this.GetArchitecture().SendEvent(new LocationResponseEvent()
                {
                    state = states,
                    ticks = now,
                    data = dataStr
                });
            });
        }
    }

    [System.Serializable]
    public class ImageItem
    {
        public string fullPath;

        public event Action<string> SelectItem;

        [Button]
        public void Select()
        {
            if (LocationTest.instance)
            {
                LocationTest.instance.SelectItem(fullPath);
            }
        }
    }
}