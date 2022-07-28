using UnityEngine;

namespace HttpData
{
    /// <summary>
    /// 定位响应
    /// </summary>
    public class LocationResponse
    {
        /// <summary>
        /// 业务返回值
        /// </summary>
        public int causeValue;
        /// <summary>
        /// 业务原因描述
        /// </summary>
        public string description;
        /// <summary>
        /// 定位返回UTM坐标下位置
        /// </summary>
        public string translation;
        /// <summary>
        /// 定位返回姿态
        /// </summary>
        public string rotation;
        /// <summary>
        /// 定位编号
        /// </summary>
        public string timestamp;
        /// <summary>
        /// 地图tile的名称
        /// </summary>
        public string maptile_name;
        /// <summary>
        /// 位置和姿态的标准差
        /// </summary>
        public float[] deviation;
        /// <summary>
        /// 地图tile的xyz偏移量
        /// </summary>
        public float[] ori_tvec;
        /// <summary>
        /// 地图tile的rotation偏移量
        /// </summary>
        public float[] ori_qvec;
    }

    /// <summary>
    /// 定位信息
    /// </summary>
    [System.Serializable]
    public class Location
    {
        /// <summary>
        /// 定位是否成功
        /// </summary>
        public bool success;

        /// <summary>
        /// 定位返回UTM坐标下位置
        /// </summary>
        public Vector3 translation;

        /// <summary>
        /// 定位返回姿态
        /// </summary>
        public Quaternion rotation;

        /// <summary>
        /// 地图tile的名称
        /// </summary>
        public string maptile_name;

        /// <summary>
        /// 地图tile的xyz偏移量
        /// </summary>
        public float[] ori_tvec;
        
        /// <summary>
        /// 地图tile的rotation偏移量
        /// </summary>
        public float[] ori_qvec;

        /// <summary>
        /// 位置和姿态的标准差
        /// </summary>
        public float[] deviation;
    }
}