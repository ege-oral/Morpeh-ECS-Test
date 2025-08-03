using Core;
using Core.Signal;
using ECS.Components.Events;
using ECS.Components.Health;
using ECS.Components.Player;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Systems.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DamageSystem : ISystem 
    {
        public World World { get; set;}

        private Filter _player;
        private Filter _damageEventFilter;
        private Stash<DamageEvent> _damageEventStash;
        private Stash<HealthComponent> _healthComponentStash;
        private Stash<InvincibilityComponent> _invincibilityStash;
        private Stash<DeadTag> _deadTagStash;
        
        public void OnAwake() 
        {
            _player = World.Filter.With<PlayerTag>().With<HealthComponent>().Build();
            _damageEventFilter = World.Filter.With<DamageEvent>().Build();
            
            _damageEventStash = World.GetStash<DamageEvent>();
            _healthComponentStash = World.GetStash<HealthComponent>();
            _invincibilityStash = World.GetStash<InvincibilityComponent>();
            _deadTagStash = World.GetStash<DeadTag>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var damageEventEntity in _damageEventFilter)
            {
                ref var damageEvent = ref _damageEventStash.Get(damageEventEntity);
                var sourceEntity = damageEvent.sourceEntity;
                var targetEntity = damageEvent.targetEntity;

                if (_invincibilityStash.Has(sourceEntity))
                {
                    continue;
                }
               
                if (damageEvent.instantKill)
                {
                    _deadTagStash.Set(targetEntity, new DeadTag());
                    continue;
                }
                
                ref var healthComponent = ref _healthComponentStash.Get(targetEntity);
                healthComponent.currentHealth -= damageEvent.damageAmount;
                
                if (healthComponent.currentHealth <= 0)
                {
                    _deadTagStash.Set(targetEntity, new DeadTag());
                }
                
                foreach (var player in _player)
                {
                    if (sourceEntity.Id == player.Id)
                    {
                        _invincibilityStash.Set(sourceEntity, new InvincibilityComponent
                        {
                            remainingTime = 5f
                        });
                    }
                }
                
                _damageEventStash.Remove(damageEventEntity);
                SignalBus.Get<DamageSignal>().Invoke(targetEntity, healthComponent.currentHealth, healthComponent.maxHealth);
            }
        }

        public void Dispose()
        {
            _player = null;
            _damageEventFilter = null;
            _damageEventStash = null;
            _healthComponentStash = null;
            _invincibilityStash = null;
            _deadTagStash = null;
        }
    }
}