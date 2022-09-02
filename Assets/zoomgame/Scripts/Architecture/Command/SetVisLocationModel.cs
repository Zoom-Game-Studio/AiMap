using HttpData;
using QFramework;
using UnityEngine;
using WeiXiang;

namespace Architecture.Command
{
    public class SetVisLocationModel : AbstractCommand
    {
        public string token;
        public string url;
        public WeiXiang.GameManager.CaptureResolution captureResolution;
        public WeiXiang.GameManager.Intrinsic intrinsic;

        protected override void OnExecute()
        {
            var model = this.GetModel<ILocationModel>();
            model.Url = url;
            model.Token = token;
            model.Intrinsic = new Float4(intrinsic.fx,intrinsic.fy,intrinsic.px,intrinsic.py);
            model.CaptureResolution = new Resolution()
            {
                width = captureResolution.width,
                height = captureResolution.height,
            };
        }
    }
}