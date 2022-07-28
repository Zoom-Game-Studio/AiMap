using System.Collections.Generic;
using System.Linq;
using UnityDeveloperKit.Runtime.Api;

namespace UnityDeveloperKit.Runtime.Extension
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 向数组中添加unity object，在对象存在时才可以成功添加
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="element"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddUnityObject<T>(this List<T> collect, T element) where  T : UnityEngine.Object
        {
            if (element)
            {
                if (!collect.Contains(element))
                {
                    collect.Add(element);
                }
            }
        }
        
        /// <summary>
        /// 向数组中添加object，在对象存在或者不为null时才可以成功添加
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="element"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddObject<T>(this List<T> collect, T element) where T : IObject
        {
            if (!element.IsNull())
            {
                if (!collect.Contains(element))
                {
                    collect.Add(element);
                }
            }
        }

        public static List<T> Clone<T>(this List<T> collect)
        {
            var list = new T[collect.Count];
            collect.CopyTo(list);
            return list.ToList();
        }

        public static void RemoveNullElement<T>(this List<T> collect) where T: IComponent 
        {
            for (int i = 0; i < collect.Count; i++)
            {
                if (collect[i].IsNull())
                {
                    collect.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}