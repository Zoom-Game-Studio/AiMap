using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppLogic
{
    public class SuperDemoAIMapGroupController : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        private IEnumerator AudioCallBack(AudioSource AudioObject, System.Action action)
        {
            Debug.Log(AudioObject.isPlaying);
            while (AudioObject.isPlaying)
            {
                yield return new WaitForSecondsRealtime(0.1f);//延迟零点一秒执行
            }
            action();
        }
    }
}
