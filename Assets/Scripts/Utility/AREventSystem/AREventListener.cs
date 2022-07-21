
public class AREventListener
{

    /// <summary> 事件处理器委托 </summary>
    public delegate void AREventHandler(AREventArgs eventArgs);
    /// <summary> 事件处理器集合 </summary>
    public AREventHandler eventHandler;

    /// <summary> 调用所有添加的事件 </summary>
    public void Invoke(AREventArgs eventArgs)
    {
        if (eventHandler != null) eventHandler.Invoke(eventArgs);
    }

    /// <summary> 清理所有事件委托 </summary>
    public void Clear()
    {
        eventHandler = null;
    }

}

