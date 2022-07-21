using System;
using AppLogic;
using UnityEngine;

namespace KleinEngine
{
    //也可以单独做个namespace,避免外层访问到
    public class AppFacade : Facade
    {
        public const string STARTUP = "startup";
        public const string SHUTDOWN = "shutdown";

        public static AppFacade GetInstance()
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null) m_instance = new AppFacade();
                }
            }

            return m_instance as AppFacade;
        }

        protected override void initializeFacade()
        {
           // registerCommand(SHUTDOWN, typeof(StartupCommand));
           //// 常用的模块固定创建，非常用模块动态创建
           // RegisterMediator(new ControlMediator(MEDIATOR.CONTROL));
        }

        public void startUp()
        {
            dispatchEvent(STARTUP);
            removeCommand(STARTUP); //PureMVC初始化完成，注销STARUP命令
        }

        public void update()
        {
            //可以优化
            //foreach (IMediator mediator in m_mediatorDic.Values)
            //{
            //    mediator.onUpdate();
            //}
            Singleton<NetworkManager>.GetInstance().update();
            TickManager.GetInstance().update(Time.deltaTime);
            ResourceManager.GetInstance().update();
        }

        public void lateUpdate()
        {
            Singleton<NetworkManager>.GetInstance().handleReceive();
        }

        public void shutDown()
        {
            //foreach(IMediator mediator in m_mediatorDic.Values)
            //{
            //    mediator.onRemove();
            //}
            //m_mediatorDic.Clear();
            dispatchEvent(SHUTDOWN);
        }
    }
}
