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
                if (location.IsSuccess)
                {
                    var pose = LocalizationConvert.LocationToUnityPose(location);
                    LocalizationConvert.CoordinatesAlignWithView(pose, transform);
                    var o = LocalizationConvert.Origin;
                    Console.Warning($"{o.position},{o.eulerAngles}");
                }
                
                this.SendEvent(new PlayAudioEvent()
                {
                    success = location.IsSuccess,
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