using BestHTTP;
using UnityEngine;

namespace Architecture.TypeEvent
{
    /// <summary>
    /// 定位相应事件
    /// </summary>
    public class LocationResponseEvent
    {
        /// <summary>
        /// 发送请求的时间戳
        /// </summary>
        public long ticks;
        /// <summary>
        /// 发送请求的装填
        /// </summary>
        public HTTPRequestStates state;
        /// <summary>
        /// 请求返回的数据
        /// </summary>
        public string data;

        /// <summary>
        /// 是否定位成功
        /// </summary>
        public bool isSuccess => !string.IsNullOrEmpty(data) && data.Contains("succeed");

        public bool isFinish => state == HTTPRequestStates.Finished;
    }
}