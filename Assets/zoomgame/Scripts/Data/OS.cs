namespace HttpData
{
    [System.Serializable]
    public class OS
    {
        /// <summary>
        /// 手机系统名称
        /// </summary>
        public string type;

        /// <summary>
        /// 系统的版本信息，Android 系统的话，只填写原生系统版本即可
        /// </summary>
        public string version;
    }
}