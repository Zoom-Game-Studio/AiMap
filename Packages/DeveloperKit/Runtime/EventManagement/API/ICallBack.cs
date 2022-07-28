using System;
using UnityEngine.AI;

namespace EventManagement
{
    public interface ICallBack
    {
        /// <summary>
        /// 调用无参方法
        /// </summary>
        void Invoke();
        
        /// <summary>
        /// 调用泛型方法
        /// </summary>
        /// <param name="param"></param>
        void Invoke(object param);

        /// <summary>
        /// 添加监听（泛型）方法
        /// </summary>
        /// <param name="action"></param>
        void Add(object action);
        
        /// <summary>
        /// 移除监听（泛型）方法
        /// </summary>
        /// <param name="action"></param>
        void Remove(object action);
    }
}