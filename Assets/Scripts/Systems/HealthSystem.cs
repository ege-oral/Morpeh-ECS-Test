using Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

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

        public void OnAwake() {
            _filter = World.Filter.With<HealthComponent>().Build();
            _healthStash = World.GetStash<HealthComponent>();
        }

        public void OnUpdate(float deltaTime) {
            foreach (var entity in _filter) 
            {
                ref var healthComponent = ref _healthStash.Get(entity);
            }
        }

        public void Dispose() {
        }
    }
}