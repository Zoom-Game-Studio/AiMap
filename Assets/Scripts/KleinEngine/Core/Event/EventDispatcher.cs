using System;
using System.Collections.Generic;
using UnityEngine;

namespace KleinEngine
{
    public class EventDispatcher
    {
        private Dictionary<string, Action<EventObject>> eventHandleDic = new Dictionary<string, Action<EventObject>>();
        public void addEvent(string type, Action<EventObject> handle)
        {
            Action<EventObject> ac;
            if (eventHandleDic.TryGetValue(type, out ac))
            {
                Delegate[] list = ac.GetInvocationList();
                int len = list.Length;
                for (int i = 0; i < len; ++i)
                {
                    if (handle.Equals(list[i]))
                    {
#if UNITY_WSA && !UNITY_EDITOR
                        Debug.LogWarning("重复增加事件处理方法:" + handle);//handle.Method在uwp系统不兼容
#else
                        Debug.LogWarning("重复增加事件处理方法:" + handle.Method.Name);//handle.Method在uwp系统不兼容
#endif
                        return;
                    }
                }
                eventHandleDic[type] += handle;
            }
            else
            {
                eventHandleDic[type] = handle;
            }
        }

        public void removeEvent(string type, Action<EventObject> handle)
        {
            if (eventHandleDic.ContainsKey(type))
            {
                eventHandleDic[type] -= handle;
                if (null == eventHandleDic[type]) eventHandleDic.Remove(type);
            }
        }

        public virtual void dispatchEvent(string type, object obj = null, object sender = null)
        {            
            if (eventHandleDic.ContainsKey(type))
            {
                Action<EventObject> ac = eventHandleDic[type];
                if (null != ac)
                {
                    EventObject e = new EventObject();
                    e.type = type;
                    e.param = obj;
                    if (null != sender) e.sender = sender;
                    else e.sender = this;
                    ac(e);
                }
            }
        }

        public virtual void onClear()
        {
            eventHandleDic.Clear();
        }
    }

    public class EventObject
    {
        public string type;
        public object param;
        public object sender;
    }
}
