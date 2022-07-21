using UnityEngine;

/// <summary> 处理事件测试 </summary>
public class EventHandleTest : MonoBehaviour
{
    // 定义事件名称
    public const string ON_CLICK = "ON_CLICK";
    // 定义事件名称
    public const string ON_CLICK2 = "ON_CLICK2";

    private void Start()
    {
        // 添加监听器
        if (!AREventUtil.HasListener(ON_CLICK)) AREventUtil.AddListener(ON_CLICK, OnClick);
        if (!AREventUtil.HasListener(ON_CLICK2)) AREventUtil.AddListener(ON_CLICK2, OnClick2);
    }

    // 处理点击事件
    public void OnClick(AREventArgs evt)
    {
        print(evt.type);
        print(evt.args);
    }

    // 带参数的点击事件
    public void OnClick2(AREventArgs evt)
    {
        print(evt.type);
        print(evt.args[0]);
    }

    // 移除监听器
    private void OnDestroy()
    {
        AREventUtil.RemoveListener(ON_CLICK, OnClick);
        AREventUtil.RemoveListener(ON_CLICK2, OnClick2);
    }

}