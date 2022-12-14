using QFramework;
using WeiXiang;

namespace Architecture
{
    public class WeiXiangArchitecture : Architecture<WeiXiangArchitecture>
    {
        protected override void Init()
        {
            var photoCapture = new PhotoCapture();
            this.RegisterUtility<ICanCapturePhoto>(photoCapture);
            this.RegisterUtility<ITileResCache>(new TileBuilder());
            this.RegisterModel<ICameraOffset>(new CameraOffsetData());
            var locationModel = new LocationModel();
            this.RegisterModel<ILocationModel>(locationModel);
            var visionLocation = new VisionLocation();
            this.RegisterUtility<ICanLocation>(visionLocation);
            visionLocation.BindData(locationModel);
            photoCapture.BindData(locationModel);
            var amapLocation = new AmapLocation();
            amapLocation.StartLocation();
            this.RegisterUtility<IAmap>(amapLocation);
        }
    }
}