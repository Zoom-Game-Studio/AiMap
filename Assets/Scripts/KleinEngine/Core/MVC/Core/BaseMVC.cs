using System;

namespace KleinEngine
{
    public class BaseMVC
    {
        protected void registerProxy<T>() where T : IProxy
        {
            Facade.Instance.registerProxy<T>();
        }

        protected T getProxy<T>()
        {
            return Facade.Instance.retrieveProxy<T>();
        }

        public void removeProxy<T>()
        {
            Facade.Instance.removeProxy<T>();
        }

        public void registerCommand(string notificationName, Type commandType)
        {
            Facade.Instance.registerCommand(notificationName, commandType);
        }

        public void removeCommand(string notificationName)
        {
            Facade.Instance.removeCommand(notificationName);
        }

        protected void sendModuleEvent(string type, object obj = null)
        {
            Facade.Instance.dispatchEvent(type, obj);
        }
    }
}
