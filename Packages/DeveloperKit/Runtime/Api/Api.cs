using UnityEngine;

namespace UnityDeveloperKit.Runtime.Api
{
    /// <summary>
    /// unity 对象接口
    /// </summary>
    public interface IObject
    {
    }

    /// <summary>
    /// 脚本拥有transform属性
    /// </summary>
    public interface IHasTransform : IObject
    {
        public Transform transform { get; }
    }

    /// <summary>
    /// 脚本拥有game object属性
    /// </summary>
    public interface IHasGameObject : IObject
    {
        public GameObject gameObject { get; }
    }

    /// <summary>
    /// 脚本必须继承Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComponent : IHasTransform, IHasGameObject
    {
    }
}