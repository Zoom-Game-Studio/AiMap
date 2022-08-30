using AppLogic;
using QFramework;
using UniRx;

namespace WeiXiang
{
    public interface IGuessMiyu : IModel
    {
        public MiYu currentMiyu
        {
            get;
            set;
        }
        public string answer { get; set; }
        
        public IntReactiveProperty updateTime { get; }
    }
}