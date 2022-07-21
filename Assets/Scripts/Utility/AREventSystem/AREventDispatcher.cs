using System.Collections.Generic;


public class AREventDispatcher
{
    /// <summary> 事件Map </summary>
    private Dictionary<string, AREventListener> dic = new Dictionary<string, AREventListener>();

    /// <summary> 添加事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public void AddListener(string eventType, AREventListener.AREventHandler eventHandler)
    {
        AREventListener invoker;
        if (!dic.TryGetValue(eventType, out invoker))
        {
            invoker = new AREventListener();
            dic.Add(eventType, invoker);
        }
        invoker.eventHandler += eventHandler;
    }

    /// <summary> 移除事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public void RemoveListener(string eventType, AREventListener.AREventHandler eventHandler)
    {
        AREventListener invoker;
        if (dic.TryGetValue(eventType, out invoker)) invoker.eventHandler -= eventHandler;
    }

    /// <summary> 是否已经拥有该类型的事件 </summary>
    /// <param name="eventType">事件类型</param>
    public bool HasListener(string eventType)
    {
        return dic.ContainsKey(eventType);
    }

    /// <summary> 派发事件 </summary>
    /// <param name="eventType">事件类型</param>
    public void DispatchEvent(string eventType, params object[] args)
    {
        AREventListener invoker;
        if (dic.TryGetValue(eventType, out invoker))
        {
            AREventArgs evt;
            if (args == null || args.Length == 0)
            {
                evt = new AREventArgs(eventType);
            }
            else
            {
                evt = new AREventArgs(eventType, args);
            }
            invoker.Invoke(evt);
        }
    }

    /// <summary> 清理所有事件监听器 </summary>
    public void Clear()
    {
        foreach (AREventListener value in dic.Values)
        {
            value.Clear();
        }
        dic.Clear();
    }
}
