using System.Collections.Generic;
using UnityEngine;

namespace KleinEngine
{
    public class ListComponent : BaseView
    {
        protected const string ITEM_PRE = "item";
        public const string SELECT_ITEM = "select_item";
        protected Dictionary<int, ItemComponent> itemList = new Dictionary<int, ItemComponent>();
        protected ItemComponent currentItem;
        protected GameObject itemPrefab;
        private bool repeatSelectFlag = false;//不论是否已选中，可重复选中
        private bool alwaysSelectFlag = false;//不论item里面选中了任何可选择元素，item本身一直被选中
        protected int currentIndex = 0;

        public int Length
        {
            get
            {
                return itemList.Count;
            }
        }

        /*
protected virtual void init()
{
   foreach (Transform child in rectTrans)
   {
       if (child.name.Contains(ITEM_PRE))
       {
           GameObject obj = child.gameObject;
               ItemComponent item = new XXXItemComponent();
               currentIndex += 1;
               item.index = int.Parse(obj.name.Substring(4));//item字符串位置4
               item.init(obj);
               initItem(item);
       }
   }
}
*/
        public void setItemPrefab(GameObject prefab)
        {
            itemPrefab = prefab;
        }

        public ItemComponent getItem(int index)
        {
            if (itemList.ContainsKey(index)) return itemList[index];
            return null;
        }

        public Dictionary<int, ItemComponent> getAllItem()
        {
            return itemList;
        }

        public ItemComponent getItemByData(object data)
        {
            foreach (ItemComponent item in itemList.Values)
            {
                if (data.Equals(item.getData()))
                    return item;
            }
            return null;
        }

        public ItemComponent getFreeItem()
        {
            foreach (ItemComponent item in itemList.Values)
            {
                if (null == item.getData())
                    return item;
            }
            return null;
        }

        public void enableRepeatSelect(bool flag)
        {
            repeatSelectFlag = flag;
        }

        public void enableAlwaysSelect(bool flag)
        {
            alwaysSelectFlag = flag;
        }

        protected virtual void onClick(ItemComponent item)
        {
            if (!alwaysSelectFlag)
            {
                if ((null != item) && (item.getClickType() != 0))
                {
                    dispatchEvent(SELECT_ITEM, item);
                    return;
                }
            }
            if (!repeatSelectFlag)
            {
                if (currentItem == item)
                    return;
            }
            if (null != currentItem)
                currentItem.setSelect(false);
            currentItem = item;
            if (null != currentItem)
            {
                currentItem.setSelect(true);
                dispatchEvent(SELECT_ITEM, currentItem);
            }
        }

        public void removeItem(ItemComponent item)
        {
            if (null != item)
            {
                if (currentItem == item) currentItem = null;
                itemList.Remove(item.index);
                item.dispose();
            }
        }

        public void removeItemByData(object data)
        {
            ItemComponent item = getItemByData(data);
            if (null != item) removeItem(item);
        }
        private bool isOrder = true;//是否顺序添加到父transform,false:倒序添加
        public T addItem<T>(bool useFree = true, bool isOrder = true) where T : ItemComponent, new()
        {
            if (null == itemPrefab)
            {
                Debug.LogError("No ItemPrefab");
                return null;
            }
            ItemComponent item = null;
            if (useFree) item = getFreeItem();
            if (null == item)
            {
                item = new T();
                currentIndex += 1;
                item.index = currentIndex;
                GameObject obj = GameObject.Instantiate<GameObject>(itemPrefab);
                obj.transform.SetParent(rectTrans);
                if (!isOrder) obj.transform.SetAsFirstSibling();
                obj.transform.localScale = Vector3.one;
                obj.name = ITEM_PRE + item.index;
                item.init(obj);
                initItem(item);
            }
            item.setActive(true);
            return item as T;
        }

        protected void initItem(ItemComponent item)
        {
            if (null == item)
                return;
            item.setClickHandle(onClick);
            if (itemList.ContainsKey(item.index))
            {
                Debug.LogWarning("List初始化重复Item序列号：" + item.index);
                return;
            }
            itemList.Add(item.index, item);
        }

        public void selectIndex(int index)
        {
            ItemComponent item = getItem(index);
            onClick(item);
        }

        public void clearAllItem()
        {
            currentItem = null;
            foreach (ItemComponent item in itemList.Values)
            {
                item.onClear();
            }
        }

        public void disposeAllItem()
        {
            foreach (ItemComponent item in itemList.Values)
            {
                item.dispose();
            }
            itemList.Clear();
        }
    }

}