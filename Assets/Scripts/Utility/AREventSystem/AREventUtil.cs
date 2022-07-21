using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AREventUtil
{
    /// <summary> 事件派发器 </summary>
    private static AREventDispatcher dispatcher = new AREventDispatcher();

    internal static bool HasListener(string gIF_START_RECODERING, object startRecodering)
    {
        throw new NotImplementedException();
    }

    /// <summary> 添加事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void AddListener(string eventType, AREventListener.AREventHandler eventHandler)
    {
        dispatcher.AddListener(eventType, eventHandler);
    }

    /// <summary> 移除事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void RemoveListener(string eventType, AREventListener.AREventHandler eventHandler)
    {
        dispatcher.RemoveListener(eventType, eventHandler);
    }

    /// <summary> 是否已经拥有该类型的事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static bool HasListener(string eventType)
    {
        return dispatcher.HasListener(eventType);
    }

    /// <summary> 派发事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static void DispatchEvent(string eventType, params object[] args)
    {
        dispatcher.DispatchEvent(eventType, args);
    }

    /// <summary> 清理所有事件监听器 </summary>
    public static void Clear()
    {
        dispatcher.Clear();
    }
}
