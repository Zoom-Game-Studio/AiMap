using System.Threading;
using QFramework;
using WeiXiang;

namespace Architecture
{
    public class WeiXiangArchitecture : Architecture<WeiXiangArchitecture>
    {
        protected override void Init()
        {
            this.RegisterUtility<ICanCapturePhoto>(new PhotoCapture());
            this.RegisterUtility<ICanLocation>(new VisionLocation());
            this.RegisterUtility<ITileResCache>(new TileCache());
            this.RegisterModel<ICameraOffset>(new CameraOffsetData());
        }
    }
}