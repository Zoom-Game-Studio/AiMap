using QFramework;
using UnityEngine;

namespace Architecture.Command
{
    /// <summary>
    /// 开启手机的定位服务
    /// </summary>
    public class StartLocationCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Input.location.Start();
        }
    }
}