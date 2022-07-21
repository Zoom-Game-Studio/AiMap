using System;

namespace KleinEngine
{
    public class BaseCommand : BaseMVC,ICommand
    {
        public virtual void onExecute(object param)
        {

        }

        protected void addModuleEvent(string type, Action<EventObject> handle)
        {
            Facade.Instance.addEvent(type, handle);
        }

        protected void removeModuleEvent(string type, Action<EventObject> handle)
        {
            Facade.Instance.removeEvent(type, handle);
        }
    }
}
