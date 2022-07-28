using System.Collections.Generic;
using UnityDeveloperKit.Runtime.Api;
using UnityEngine;

namespace DeveloperKit.Runtime.Pool
{
    public interface IPool<T> : ICanPopItem<T>, ICanRecycleItem<T>, ICanNewItem<T>, IHasContent<T>
    {
    }

    public interface IHasContent<out T>
    {
        public IEnumerable<T> Content { get; }
    }

    public interface IHasPrefab<out T>
    {
        public T Prefab { get; }
    }
    
    public interface IHasPrefabGameObject
    {
        public GameObject Prefab { get; }
    }


    /// <summary>
    /// 对象池弹出物体的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICanPopItem<out T>
    {
        /// <summary>
        /// 弹出物体
        /// </summary>
        /// <returns></returns>
        T PopItem();
    }

    /// <summary>
    /// 实例化方法的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICanNewItem<out T>
    {
        /// <summary>
        /// 对象池物体不够时，从此实例化新的物体
        /// </summary>
        /// <returns></returns>
        T NewItem();
    }

    /// <summary>
    /// 对象池回收物体的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICanRecycleItem<in T>: IObject
    {
        /// <summary>
        /// 回收物体
        /// </summary>
        /// <param name="item"></param>
        void RecycleItem(T item);
    }

    public interface IHasUseOfState:IComponent
    {
        /// <summary>
        /// 是否在使用中
        /// </summary>
        bool IsInUse { get; set; }
    }

    /// <summary>
    /// 对象池物体的接口
    /// </summary>
    public interface IHasPopCallback
    {
        /// <summary>
        /// 弹出时的回调
        /// </summary>
        public void OnPop();
    }

    /// <summary>
    /// 对象池物体的接口
    /// </summary>
    public interface IHasRecycleCallback
    {
        /// <summary>
        /// 回收时的回调
        /// </summary>
        public void OnRecycle();
    }

    /// <summary>
    ///  对象池物体的制造者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasCreator<T>
    {
        /// <summary>
        /// 实例化此物体的对象池
        /// </summary>
        public ICanRecycleItem<T> Creator { get; set; }
    }
}