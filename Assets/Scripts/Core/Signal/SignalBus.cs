namespace Core.Signal
{
    public static class SignalBus
    {
        private static readonly Bus Bus = new();
        
        public static TSignal Get<TSignal>() where TSignal : ISignal, new()
        {
            return Bus.Get<TSignal>();
        }
    }
}