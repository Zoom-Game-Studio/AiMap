using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace KleinEngine
{
    public class BaseView : EventDispatcher
    {
        public const string BUTTON_CLICK = "button_click";
        protected const string VIEW_CLOSE = "view_close";
        private static readonly Type COMPONENT_TYPE = typeof(Component);
        //要搜索的成员类型
        private static readonly BindingFlags BIND_FLAG = BindingFlags.Public | BindingFlags.Instance;
        private static readonly Type UISIGN_TYPE = typeof(UISign);

        public GameObject gameObject;
        public RectTransform rectTrans;

        public void init(GameObject go)
        {
            if (null == go) return;
            onParse(this,go);
        }

        protected virtual void start()
        {
            
        }

        public virtual void dispose()
        {
            GameObject.DestroyImmediate(gameObject, true);
        }

        protected virtual void onClick(object obj)
        {            
            dispatchEvent(BUTTON_CLICK, obj);
        }

        public virtual void onEnter()
        {
            if(null != rectTrans) rectTrans.SetAsLastSibling();
            setActive(true);
        }

        public virtual void onExit()
        {
            setActive(false);
        }

        public void setActive(bool flag)
        {
            if (null != gameObject) gameObject.SetActive(flag);
        }

        protected void onCloseView()
        {
            dispatchEvent(VIEW_CLOSE);
        }

        protected void onParse(object o, GameObject go)
        {
            BaseView view = o as BaseView;
            if (null != view)
            {
                view.gameObject = go;
                view.rectTrans = go.transform as RectTransform;
            }
            Type type = o.GetType();
            FieldInfo[] filds = type.GetFields(BIND_FLAG);
            Transform goTrans = go.transform;
            int len = filds.Length;
            for (int i = 0; i < len; ++i)
            {
                FieldInfo m = filds[i];
                //反射获得用户自定义属性
                Attribute attr = CustomAttributeExtensions.GetCustomAttribute(m, UISIGN_TYPE);
                //Attribute attr1 = Attribute.GetCustomAttribute(m, UISIGN_TYPE);
                if (attr == null) continue;
                UISign uilabel = attr as UISign;
                string path = uilabel.Path.Length > 0 ? (uilabel.Path + m.Name) : m.Name;
                //string path = m.Name;               
                Transform child = goTrans.Find(path);
                if (child == null)
                {
                    Debug.LogWarning("不存在组件:" + m.Name);
                    continue;
                }
                //if (m.FieldType.IsSubclassOf(COMPONENT_TYPE))////Component c = child.GetComponent(m.FieldType); if(null != c)
                if (m.FieldType.GetTypeInfo().IsSubclassOf(COMPONENT_TYPE))
                {
                    Component c = child.GetComponent(m.FieldType);
                    m.SetValue(o, c);
                    if (m.FieldType.Name.Contains("Button"))
                    {
                        ((Button)c).onClick.AddListener(delegate ()
                        {
                            onClick(c);//onClick(m.Name);
                        });
                    }
                }
                else if (m.FieldType.Name.Equals("GameObject"))//if (m.FieldType == typeof(GameObject))
                {
                    m.SetValue(o, child.gameObject);
                }
                else//if (m.FieldType.IsSubclassOf(typeof(ViewComponent)))
                {
                    object childObject = Activator.CreateInstance(m.FieldType);
                    m.SetValue(o, childObject);
                    onParse(childObject, child.gameObject);
                }
            }
            if (null != view)
            {
                view.start();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class UISign : Attribute
    {
        string path = string.Empty;
        public UISign(string str)
        {
            path = str;
        }
        public UISign()
        {
        }
        public string Path
        {
            get
            {
                return path;
            }
        }
    }
}

