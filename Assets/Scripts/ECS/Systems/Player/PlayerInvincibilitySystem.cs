using ECS.Components.Player;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Systems.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerInvincibilitySystem : ISystem 
    {
        public World World { get; set;}
        private Filter _invincibilityFilter;
        private Stash<InvincibilityComponent> _invincibilityStash;

        public void OnAwake()
        {
            _invincibilityFilter = World.Filter.With<PlayerTag>().With<InvincibilityComponent>().Build();
            _invincibilityStash = World.GetStash<InvincibilityComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _invincibilityFilter)
            {
                ref var invincibilityComponent = ref _invincibilityStash.Get(entity);
                invincibilityComponent.remainingTime -= deltaTime;

                if (invincibilityComponent.remainingTime <= 0)
                {
                    _invincibilityStash.Remove(entity);
                }
            }
        }

        public void Dispose()
        {
            _invincibilityFilter = null;
            _invincibilityStash = null;
        }
    }
}