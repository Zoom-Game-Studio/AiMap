using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventManagement
{
    /// <summary>
    /// 事件回调的集合
    /// </summary>
    public class CallbackCollection
    {
        /// <summary>
        /// 参数类型对应的方法集合
        /// </summary>
        private Dictionary<Type, ICallBack> dict = new Dictionary<Type, ICallBack>();

        /// <summary>
        /// 添加void方法作为回调
        /// </summary>
        /// <param name="act"></param>
        public void Add(Action act)
        {
            var t = typeof(void);
            if (dict.TryGetValue(t, out var callBack))
            {
                callBack.Add(act);
            }
            else
            {
                callBack = Callback.Create(act);
                dict.Add(t, callBack);
            }
        }

        /// <summary>
        /// 添加泛型方法作为回调
        /// </summary>
        /// <param name="act"></param>
        /// <typeparam name="T"></typeparam>
        public void Add<T>(Action<T> act)
        {
            var t = typeof(T);
            if (dict.TryGetValue(t, out var callBack))
            {
                callBack.Add(act);
            }
            else
            {
                callBack = Callback<T>.Create(act);
                dict.Add(t, callBack);
            }
        }

        /// <summary>
        /// 移除方法
        /// </summary>
        /// <param name="t">方法参数类型</param>
        /// <param name="act">方法的引用</param>
        private void Remove(Type t, object act)
        {
            if (dict.TryGetValue(t, out var callback))
            {
                callback.Remove(act);
            }
        }

        /// <summary>
        /// 移除void 方法
        /// </summary>
        /// <param name="act"></param>
        public void Remove(Action act)
        {
            var t = typeof(void);
            Remove(t, act);
        }

        /// <summary>
        /// 移除泛型方法
        /// </summary>
        /// <param name="act"></param>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>(Action<T> act)
        {
            var t = typeof(T);
            Remove(t, act);
        }

        public void Invoke()
        {
            if (dict.TryGetValue(typeof(void), out var c))
            {
                try
                {
                    c.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        public void Invoke<T>(T param)
        {
            if (dict.TryGetValue(typeof(T), out var callBack))
            {
                try
                {
                    callBack.Invoke(param);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}