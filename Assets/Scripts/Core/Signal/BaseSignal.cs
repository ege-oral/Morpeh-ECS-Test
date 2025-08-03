using System;
using System.Linq;

namespace Core.Signal
{
    public class BaseSignal : ISignal
    {
        public event Action<ISignal, object[]> BaseListener;
        private event Action<ISignal> GenericListener;

        private string _hash;

        public void Subscribe(Action<ISignal, object[]> callback)
        {
            if (BaseListener == null || !BaseListener.GetInvocationList().Contains(callback))
            {
                BaseListener += callback;
            }
        }

        public void Subscribe(Action<ISignal> action)
        {
            GenericListener += action;
        }

        public void Unsubscribe(Action<ISignal> action)
        {
            GenericListener -= action;
        }

        public void Invoke(object[] args)
        {
            BaseListener?.Invoke(this, args);
            GenericListener?.Invoke(this);
        }

        public string Hash
        {
            get
            {
                if (string.IsNullOrEmpty(_hash))
                {
                    _hash = GetType().ToString();
                }

                return _hash;
            }
        }
    }

    public class Signal : BaseSignal
    {
        private event Action Callback;

        public void Subscribe(Action handler)
        {
            Callback += handler;
        }

        public void Unsubscribe(Action handler)
        {
            Callback -= handler;
        }

        public void UnsubscribeAll()
        {
            Callback = null;
        }

        public void Invoke()
        {
            Callback?.Invoke();
            base.Invoke(null);
        }
    }

    public class Signal<T> : BaseSignal
    {
        private event Action<T> Callback;
        private T _lastArgument;

        public void Subscribe(Action<T> handler)
        {
            Callback += handler;
        }

        public void Unsubscribe(Action<T> handler)
        {
            Callback -= handler;
        }

        public void UnsubscribeAll()
        {
            Callback = null;
        }

        public T GetLast(T defaultArgument)
        {
            return _lastArgument == null ? defaultArgument : _lastArgument;
        }

        public void Invoke(T arg1)
        {
            _lastArgument = arg1;
            Callback?.Invoke(arg1);
            object[] arg = { arg1 };
            base.Invoke(arg);
        }
    }

    public class Signal<TArg1, TArg2> : BaseSignal
    {
        private event Action<TArg1, TArg2> Callback;

        public void Subscribe(Action<TArg1, TArg2> handler)
        {
            Callback += handler;
        }

        public void Unsubscribe(Action<TArg1, TArg2> handler)
        {
            Callback -= handler;
        }

        public void UnsubscribeAll()
        {
            Callback = null;
        }

        public void Invoke(TArg1 arg1, TArg2 arg2)
        {
            Callback?.Invoke(arg1, arg2);

            object[] arg = { arg1, arg2 };
            base.Invoke(arg);
        }
    }

    public class Signal<TArg1, TArg2, TArg3> : BaseSignal
    {
        private event Action<TArg1, TArg2, TArg3> Callback;

        public void Subscribe(Action<TArg1, TArg2, TArg3> handler)
        {
            Callback += handler;
        }

        public void Unsubscribe(Action<TArg1, TArg2, TArg3> handler)
        {
            Callback -= handler;
        }

        public void UnsubscribeAll()
        {
            Callback = null;
        }

        public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            Callback?.Invoke(arg1, arg2, arg3);

            object[] arg = { arg1, arg2, arg3 };
            base.Invoke(arg);
        }
    }
}