using System;
using System.Collections.Generic;

namespace KleinEngine
{
    public class BaseMediator : BaseMVC,IMediator
    {
        public static string STATE_ENTER = "state_enter";
        public static string STATE_EXIT = "state_exit";

        protected string m_mediatorName;
        protected string m_mediatorState = STATE_EXIT;
        protected BaseView m_baseView;

        List<BaseMediator> m_subMediatorList = new List<BaseMediator>();
        //protected Dictionary<string,BaseMediator> m_SubMediatorDic;

        public virtual string name
        {
            get { return m_mediatorName; }
        }

        public string state
        {
            get { return m_mediatorState; }
        }

        public virtual BaseView viewComponent
        {
            get { return m_baseView; }
            set { m_baseView = value; }
        }

        /// <summary>
        /// 初始化函数，只在首次实例化调用
        /// </summary>
        public virtual void onRegister()
        {
            if(null != m_baseView) m_baseView.addEvent(BaseView.BUTTON_CLICK, onButtonClick);
            m_mediatorState = STATE_ENTER;
            onInit();
        }

        protected virtual void onInit()
        {

        }

        public void registerSubMediator<T>(BaseView view = null) where T : BaseMediator,new()
        {
            BaseMediator mediator = new T();//BaseMediator mediator = Activator.CreateInstance<T>();
            m_subMediatorList.Add(mediator);
            mediator.viewComponent = view;
            mediator.onRegister();
        }

        protected void addModuleEvent(string type, Action<EventObject> handle)
        {
            Facade.Instance.addEvent(type,handle);
        }

        protected void removeModuleEvent(string type, Action<EventObject> handle)
        {
            Facade.Instance.removeEvent(type, handle);
        }

        protected virtual void onButtonClick(EventObject obj)
        {
            foreach(BaseMediator mediator in m_subMediatorList)
            {
                if (null != mediator) mediator.onButtonClick(obj);
            }
//		    Debug.Log ("没有重载点击处理：" + this);
        }

        public virtual void onEnter()
        {
            m_mediatorState = STATE_ENTER;
            foreach(BaseMediator mediator in m_subMediatorList)
            {
                mediator.onEnter();
            }
            if (null != m_baseView) m_baseView.onEnter();
        }

        public virtual void onExit()
        {
            m_mediatorState = STATE_EXIT;
            foreach (BaseMediator mediator in m_subMediatorList)
            {
                mediator.onExit();
            }
            if (null != m_baseView) m_baseView.onExit();
        }

        public virtual void onRemove()
        {
            foreach (BaseMediator mediator in m_subMediatorList)
            {
                mediator.onRemove();
            }
            if (null != m_baseView) m_baseView.dispose();
        }

        public virtual void onUpdate()
        {

        }
    }
}
