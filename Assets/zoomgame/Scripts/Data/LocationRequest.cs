namespace HttpData
{
    /// <summary>
    /// 定位请求 post data value
    /// </summary>
    public class LocationRequest
    {
        /// <summary>
        /// 设备相关信息
        /// </summary>
        public string UUID;

        /// <summary>
        /// pinhole model k1, k2, p1, p2
        /// </summary>
        public string distortion;

        /// <summary>
        /// 先验定位在水平方向上的标准差 m
        /// </summary>
        // public string horizontal_deviation;

        /// <summary>
        /// fx, fy, cx, cy in px
        /// </summary>
        public string intrinsic;

        /// <summary>
        /// 手机朝向（1:左横屏；2:倒立竖屏；3:右横屏；4:竖屏），因目前视觉定位只能定位一个方向，需要根据手机朝向改变图片方向
        /// </summary>
        public int orientation;

        /// <summary>
        /// 先验场景名称
        /// </summary>
        // public string prior_maptile_name = "shanghai";

        /// <summary>
        /// 手机姿态信息
        /// </summary>
        public string prior_rotation;

        /// <summary>
        /// 手机gps信息
        /// </summary>
        public string prior_translation;

        /// <summary>
        /// 定位数据收集的时间戳（UTC 时间，单位：毫秒）
        /// </summary>
        public string timestamp;

        /// <summary>
        /// 先验定位在垂直方向上的标准差 m
        /// </summary>
        // public string vertical_deviation;
    }
}