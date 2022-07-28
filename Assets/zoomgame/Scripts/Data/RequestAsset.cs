namespace HttpData
{
    [System.Serializable]
    public class RequestAsset
    {
        /// <summary>
        /// 设备 ID，使用用户自定义的值，如 UUID；
        /// </summary>
        public string id;

        /// <summary>
        /// 设备 IMEI 号的 MD5 值
        /// </summary>
        public string imeiMd5;

        /// <summary>
        /// 设备网卡的 MAC 地址
        /// </summary>
        public string macAddress;

        /// <summary>
        /// 设备制造厂商名称
        /// </summary>
        public string manufacturer;

        /// <summary>
        /// 设备型号
        /// </summary>
        public string model;

        /// <summary>
        /// 设备的操作系统信息
        /// </summary>
        public OS os;

        /// <summary>
        /// 设备的序列号
        /// </summary>
        public string serialNumber;

        /// <summary>
        /// 系统提供的唯一码，Android 系统的 android_id，iOS 系统的 IDFA
        /// </summary>
        public string uniqueId;
    }
}