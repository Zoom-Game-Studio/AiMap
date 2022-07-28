using System.Collections.Generic;
using UnityDeveloperKit.Runtime.Extension;
using UnityEngine;

namespace DeveloperKit.Runtime.Pool
{
    /// <summary>
    /// Unity object pool
    /// </summary>
    [System.Serializable]
    public class GameObjectPool : IPool<UnityEngine.GameObject>, IHasPrefabGameObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform prefabParent;
        private List<GameObject> content = new List<GameObject>();
        public IEnumerable<GameObject> Content => content;
        public GameObject Prefab => prefab;

        public  virtual GameObject PopItem()
        {
            var gameObject = content.Find(e=> !e.activeInHierarchy);
            if (!gameObject)
            {
                gameObject = NewItem();
                content.AddUnityObject(gameObject);
            }
            
            return gameObject;
        }

        /// <summary>
        /// 回收物体，把物体压入对象池
        /// </summary>
        /// <param name="item"></param>
        public virtual void RecycleItem(GameObject item)
        {
            content.AddUnityObject(item);
        }

        public virtual GameObject NewItem()
        {
            return GameObject.Instantiate(prefab,prefabParent);
        }

    }

}