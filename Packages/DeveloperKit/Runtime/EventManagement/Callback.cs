using System;

namespace EventManagement
{
    public class Callback : ICallBack
    {
        public event Action callback;

        public void Invoke()
        {
            callback?.Invoke();
        }

        public void Invoke(object param)
        {
            throw new NotImplementedException();
        }

        public void Add(object action)
        {
            callback += (Action) action;
        }

        public void Remove(object action)
        {
            if (callback != null)
            {
                callback -= (Action) action;
            }
        }
        
        public static Callback Create(Action action)
        {
            var c = new Callback();
            c.callback = action;
            return c;
        }
    }


    /// <summary>
    /// 回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Callback<T> : ICallBack
    {
        public event Action<T> callback;

        public void Invoke()
        {
            throw new NotImplementedException();
        }

        public void Invoke(object param)
        {
            callback?.Invoke((T) param);
        }

        public void Add(object action)
        {
            callback += (Action<T>) action;
        }

        public void Remove(object action)
        {
            if (callback != null)
            {
                callback -= (Action<T>) action;
            }
        }

        public static Callback<T> Create(Action<T> action)
        {
            var c = new Callback<T>();
            c.callback = action;
            return c;
        }
    }
}