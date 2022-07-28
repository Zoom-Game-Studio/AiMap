using System;
using System.Text;

namespace HttpData
{
    [System.Serializable]
    public class BodyData
    {
        /// <summary>
        /// 设备相关信息
        /// </summary>
        public string UUID;

        /// <summary>
        /// pinhole model k1, k2, p1, p2
        /// </summary>
        public Float4 distortion;

        /// <summary>
        /// 先验定位在水平方向上的标准差 m
        /// </summary>
        // public float horizontal_deviation;

        /// <summary>
        /// 先验定位在垂直方向上的标准差 m
        /// </summary>
        // public float vertical_deviation;
        
        /// <summary>
        /// fx, fy, cx, cy in px
        /// </summary>
        public Float4 intrinsic;

        /// <summary>
        /// 手机朝向（1:左横屏；2:倒立竖屏；3:右横屏；4:竖屏），因目前视觉定位只能定位一个方向，需要根据手机朝向改变图片方向
        /// </summary>
        public int orientation;

        // /// <summary>
        // /// 先验场景名称
        // /// </summary>
        // public string prior_maptile_name = "shanghai";

        /// <summary>
        /// 手机姿态信息
        /// </summary>
        public PriorRotation prior_rotation;

        /// <summary>
        /// 手机gps信息
        /// </summary>
        public PriorTranslation prior_translation;

        /// <summary>
        /// 定位数据收集的时间戳（UTC 时间，单位：毫秒）
        /// </summary>
        public Int64 timestamp;

        public static Int64 GetUTCTimeStamp()
        {
            var dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var ts = DateTime.UtcNow - dd;
            return (Int64) ts.TotalMilliseconds;
        }
    }
}