using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AREventArgs
{
    /// <summary> 事件类型 </summary>
    public readonly string type;
    /// <summary> 事件参数 </summary>
    public readonly object[] args;

    public AREventArgs(string type)
    {
        this.type = type;
    }

    public AREventArgs(string type, params object[] args)
    {
        this.type = type;
        this.args = args;
    }
}

