using System;

namespace EventManagement
{
    public class EvtMgr
    {
        public static void Clear()
        {
            EventManager.Instance.Clear();
        }
        public static void Bind<T>(EventTrigger trigger, Action<T> action)
        {
            EventManager.Instance.Bind(trigger,action);
        }

        public static void Bind(EventTrigger trigger, Action action)
        {
            EventManager.Instance.Bind(trigger, action);
        }

        public static void Remove<T>(EventTrigger trigger, Action<T> action)
        {
            EventManager.Instance.Remove(trigger, action);

        }

        public static void Remove(EventTrigger trigger, Action action)
        {
            EventManager.Instance.Remove(trigger, action);

        }


        public static void Trigger(EventTrigger trigger)
        {
            EventManager.Instance.Trigger(trigger);
        }

        public static void Trigger<T>(EventTrigger trigger, T param, bool includeVoidMethod = true)
        {
            EventManager.Instance.Trigger(trigger,param,includeVoidMethod);
        }
    }
}