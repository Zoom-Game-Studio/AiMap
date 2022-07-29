using System;
using QFramework;
using WeiXiang;

namespace Architecture.Command
{
    /// <summary>
    /// 修改相机定位偏移
    /// </summary>
    public class ChangeCameraOffsetCommand : AbstractCommand
    {
        private CameraCorrect.Axis _axis;
        private float _step;
        public ChangeCameraOffsetCommand(CameraCorrect.Axis axis, float step)
        {
            _axis = axis;
            _step = step;
        }

        protected override void OnExecute()
        {
            var data = this.GetModel<ICameraOffset>();
            switch (_axis)
            {
                case CameraCorrect.Axis.x:
                    data.X.Value += _step;
                    break;
                case CameraCorrect.Axis.y:
                    data.Y.Value += _step;
                    break;
                case CameraCorrect.Axis.z:
                    data.Z.Value += _step;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}