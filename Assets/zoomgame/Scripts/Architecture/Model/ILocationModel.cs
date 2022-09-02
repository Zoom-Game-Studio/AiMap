using HttpData;
using QFramework;
using UnityEngine;

namespace WeiXiang
{
    public interface ILocationModel : IModel
    {
        string Token { get; set; }
        /// <summary>
        /// 定位接口
        /// </summary>
        string Url { get; set; }
        /// <summary>
        /// 抽帧分辨率
        /// </summary>
        Resolution CaptureResolution { get; set; }
        
        /// <summary>
        /// RGB Camera 参数
        /// </summary>
        Float4 Intrinsic { get; set; }
    }

    public class LocationModel : ILocationModel
    {
        // public const string TokenExample = "5bc251ab113f1510e3e1509b2442d52b";
        // public const string Interface1 = @"https://hdmap.newayz.com/wayzoom/v1/vps/single";
        // public const string Interface2 = @"http://dev-hdmap.newayz.com:8800/wayzoom/v1/vps/single";
        private IArchitecture _arc;

        public IArchitecture GetArchitecture()
        {
            return this._arc;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            this._arc = architecture;
        }

        public void Init()
        {
        }

        public string Token { get; set; }
        public string Url { get; set; }
        public Resolution CaptureResolution { get; set; }
        public Float4 Intrinsic { get; set; }
    }
}