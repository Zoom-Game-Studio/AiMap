using System;
using HttpData;
using QFramework;
using UnityEngine;
using WeiXiang;
using Console = WeiXiang.Console;

namespace Architecture.Command
{
    /// <summary>
    /// 对齐坐标系
    /// </summary>
    public class AlignCoordinateCommand : AbstractCommand
    {
        public string data;
        public Transform transform;

        protected override void OnExecute()
        {
            try
            {
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<LocationResponse>(data);
                var location = DataMapper.ToLocation(response);
                if (location.success)
                {
                    var pose = LocalizationConvert.LocationToUnityPose(location);
                    LocalizationConvert.CoordinatesAlignWithView(pose, transform);
                }

                this.SendEvent(new PlayAudioEvent()
                {
                    success = location.success,
                });
            }
            catch (Exception e)
            {
                Console.Error("Align coordinate fail: " + e.Message);
                Console.Error(e.StackTrace);
            }
        }
    }
}