using System;

namespace Core.Signal
{
    public interface ISignal
    {
        string Hash { get; }
        void Subscribe(Action<ISignal> action);
        void Unsubscribe(Action<ISignal> action);
    }
}