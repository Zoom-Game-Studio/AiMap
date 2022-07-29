using QFramework;
using UniRx;

namespace WeiXiang
{
    public interface ICameraOffset : IModel
    {
        FloatReactiveProperty X { get; }
        FloatReactiveProperty Y { get; }
        FloatReactiveProperty Z { get; }
    }

    public class CameraOffsetData : ICameraOffset
    {
        private IArchitecture _arc;
        public FloatReactiveProperty X { get; } = new FloatReactiveProperty();
        public FloatReactiveProperty Y { get; } = new FloatReactiveProperty();
        public FloatReactiveProperty Z { get; } = new FloatReactiveProperty();
        public IArchitecture GetArchitecture()
        {
            return _arc;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            this._arc = architecture;
        }

        public void Init()
        {
        }
    }
}