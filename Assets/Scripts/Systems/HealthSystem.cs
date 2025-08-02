using Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthSystem : ISystem 
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<HealthComponent> _healthStash;
        private Stash<DeadTag> _deadTagStash;

        public void OnAwake() 
        {
            _filter = World.Filter.With<HealthComponent>().Build();
            _healthStash = World.GetStash<HealthComponent>();
            _deadTagStash  = World.GetStash<DeadTag>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter) 
            {
                ref var healthComponent = ref _healthStash.Get(entity);
                if (healthComponent.healthPoints <= 0)
                {
                    _deadTagStash.Set(entity);
                }
            }
        }

        public void Dispose() 
        {
            
        }
    }
}