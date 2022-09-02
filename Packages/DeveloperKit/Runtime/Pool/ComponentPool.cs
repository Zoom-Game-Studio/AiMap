using System.Collections.Generic;
using UnityDeveloperKit.Runtime.Extension;
using UnityEngine;

namespace DeveloperKit.Runtime.Pool
{
    /// <summary>
    /// 组件对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ComponentPool<T> : IHasContent<T>, ICanPopItem<T>, ICanRecycleItem<T>, ICanNewItem<T>,
        IHasPrefabGameObject where T : IHasUseOfState
    {
        [SerializeField]private List<T> content = new List<T>();
        [SerializeField] private GameObject prefab;
        
        public IEnumerable<T> Content => content;
        
        public GameObject Prefab => prefab;

        public virtual T PopItem()
        {
            var component = content.Find(e => !e.IsInUse);
            if (component.IsNull())
            {
                component = NewItem();
                content.AddObject(component);
            }
            
            component.IsInUse = true;
            if (component is IHasPopCallback canRecycle)
            {
                canRecycle.OnPop();
            }
            return component;
        }

        public virtual void RecycleItem(T item)
        {
            if (item is IHasRecycleCallback canRecycle)
            {
                canRecycle.OnRecycle();
            }
            item.IsInUse = false;
            content.AddObject(item);
        }

        public virtual T NewItem()
        {
            var t = Object.Instantiate(Prefab).GetComponent<T>();
            if (t is IHasCreator<T> hasCreator)
            {
                hasCreator.Creator = this;
            }
            return t;
        }
    }
}