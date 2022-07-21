using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KleinEngine
{
    public class Facade : EventDispatcher
    {
        protected static Facade m_instance;//protected static volatile Facade m_instance;
        protected static readonly object m_staticSyncRoot = new object();

        public static Facade Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new Facade();
                    }
                }

                return m_instance;
            }
        }
        //static Facade()
        //{

        //}
        protected Facade()
        {
            initializeFacade();
        }

        protected virtual void initializeFacade()
        {

        }

        private Dictionary<string, IProxy> m_proxyDic = new Dictionary<string, IProxy>();

        public void registerProxy<T>() where T : IProxy
        {
            IProxy proxy = Activator.CreateInstance<T>();
            m_proxyDic.Add(typeof(T).Name, proxy);
            proxy.onRegister();
        }

        public T retrieveProxy<T>()
        {
            string name = typeof(T).Name;
            if (m_proxyDic.ContainsKey(name)) return (T)m_proxyDic[name];
            return default(T);
        }

        public void removeProxy<T>()
        {
            string name = typeof(T).Name;
            if (m_proxyDic.ContainsKey(name))
            {
                IProxy proxy = m_proxyDic[name];
                m_proxyDic.Remove(name);
                proxy.onRemove();
            }
        }

        private Dictionary<string, Type> m_commandDic = new Dictionary<string, Type>();
        public void registerCommand(string notificationName, Type commandType)
        {
            if (!m_commandDic.ContainsKey(notificationName))
                m_commandDic.Add(notificationName, commandType);
        }

        public void removeCommand(string notificationName)
        {
            if (!m_commandDic.ContainsKey(notificationName))
                m_commandDic.Remove(notificationName);
        }

        protected Dictionary<string, IMediator> m_mediatorDic = new Dictionary<string, IMediator>();
        public void registerMediator(IMediator mediator)
        {
            if (null == mediator) return;
            if (m_mediatorDic.ContainsKey(mediator.name)) return;
            m_mediatorDic.Add(mediator.name, mediator);
            mediator.onRegister();
        }

        public IMediator retrieveMediator(string mediatorName)
        {
            if (!m_mediatorDic.ContainsKey(mediatorName)) return null;
            return m_mediatorDic[mediatorName];
        }

        public void removeMediator(string mediatorName)
        {
            if (!m_mediatorDic.ContainsKey(mediatorName)) return;
            IMediator mediator = m_mediatorDic[mediatorName];
            m_mediatorDic.Remove(mediatorName);
            mediator.onRemove();
        }

        public override void dispatchEvent(string type, object obj = null, object sender = null)
        {

            if (m_commandDic.ContainsKey(type))
            {
                //可以优化为常用ICommand和一次性ICommand
                ICommand command = Activator.CreateInstance(m_commandDic[type]) as ICommand;
                if (null != command) command.onExecute(obj);
//                return;//mediator type可能和command type重复
            }
            base.dispatchEvent(type, obj, sender);
        }
    }
}
