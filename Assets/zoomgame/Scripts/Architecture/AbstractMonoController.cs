using QFramework;
using UnityEngine;

namespace Architecture
{
    public abstract class AbstractMonoController : MonoBehaviour,IController
    {
        public IArchitecture GetArchitecture()
        {
            return WeiXiangArchitecture.Interface;
        }
    }
}