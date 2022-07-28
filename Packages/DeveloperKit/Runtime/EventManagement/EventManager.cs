using System;
using System.Collections.Generic;

namespace EventManagement
{
    /// <summary>
    /// 事件管理器的基类
    /// </summary>
    public class EventManager
    {
        private static EventManager _instance;

        public static EventManager Instance => _instance ?? (_instance = new EventManager());

        //回调字典
        private Dictionary<EventTrigger, CallbackCollection> dict =
            new Dictionary<EventTrigger, CallbackCollection>();


        /// <summary>
        /// 清空事件管理器
        /// </summary>
        public void Clear()
        {
            dict.Clear();
        }

        /// <summary>
        /// 绑定泛型回调,如果事件管理器正在执行回调，则会在回调执行完毕后进行此绑定
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public void Bind<T>(EventTrigger trigger, Action<T> action)
        {
            if (dict.TryGetValue(trigger, out var collection))
            {
                collection.Add(action);
            }
            else
            {
                collection = new CallbackCollection();
                collection.Add(action);
                dict.Add(trigger, collection);
            }
        }

        /// <summary>
        /// 绑定无参回调,如果事件管理器正在执行回调，则会在回调执行完毕后进行此绑定
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        public void Bind(EventTrigger trigger, Action action)
        {
            if (dict.TryGetValue(trigger, out var collection))
            {
                collection.Add(action);
            }
            else
            {
                collection = new CallbackCollection();
                collection.Add(action);
                dict.Add(trigger, collection);
            }
        }

        /// <summary>
        /// 移除泛型回调,如果事件管理器正在执行回调，则会在回调执行完毕后进行此移除操作
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>(EventTrigger trigger, Action<T> action)
        {
            if (dict.TryGetValue(trigger, out var collection))
            {
                collection.Remove(action);
            }
        }

        /// <summary>
        /// 移除无参回调,如果事件管理器正在执行回调，则会在回调执行完毕后进行此移除操作
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="action"></param>
        public void Remove(EventTrigger trigger, Action action)
        {
            if (dict.TryGetValue(trigger, out var collection))
            {
                collection.Remove(action);
            }
        }

        /// <summary>
        /// 触发无参回调
        /// </summary>
        /// <param name="trigger"></param>
        public void Trigger(EventTrigger trigger)
        {
            if (dict.TryGetValue(trigger, out var c))
            {
                c.Invoke();
            }
        }

        /// <summary>
        /// 触发泛型回调
        /// </summary>
        /// <param name="trigger">触发信号</param>
        /// <param name="param">参数</param>
        /// <param name="includeVoidMethod">同时调用无参方法</param>
        /// <typeparam name="T"></typeparam>
        public void Trigger<T>(EventTrigger trigger, T param, bool includeVoidMethod = true)
        {
            if (dict.TryGetValue(trigger, out var c))
            {
                c.Invoke(param);
                if (includeVoidMethod)
                {
                    c.Invoke();
                }
            }
        }
    }
}