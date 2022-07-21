using System;
using UnityEngine;

namespace KleinEngine
{
    public class ItemComponent : BaseView
    {
        [HideInInspector]
        public int index;

        private object data;
        protected Action<ItemComponent> clickHandle;
        protected int clickType = 0;

        public void setClickHandle(Action<ItemComponent> handle)
        {
            clickHandle = handle;
        }

        public virtual void setData(object value)
        {
            data = value;
        }

        public object getData()
        {
            return data;
        }

        public int getClickType()
        {
            return clickType;
        }

        public override void onClear()
        {
            setData(null);
            setSelect(false);
            setActive(false);
        }

        public virtual void setSelect(bool flag)
        {

        }

        protected virtual void handleClickType(object obj)
        {

        }

        protected override void onClick(object obj)
        {
            clickType = 0;
            handleClickType(obj);
            if (null != clickHandle)
                clickHandle(this);
        }

        public override void dispose()
        {
            setData(null);
            clickHandle = null;
            setActive(false);
            base.dispose();
        }
    }
}