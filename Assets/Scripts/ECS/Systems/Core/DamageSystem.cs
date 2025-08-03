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
                var targetEntity = damageEvent.targetEntity;

                if (_invincibilityStash.Has(targetEntity))
                {
                    RemoveDamageEvent(damageEventEntity);
                    continue;
                }

                if (damageEvent.instantKill)
                {
                    KillEntity(targetEntity);
                    RemoveDamageEvent(damageEventEntity);
                    continue;
                }

                ref var health = ref _healthComponentStash.Get(targetEntity);
                health.currentHealth -= damageEvent.damageAmount;

                if (health.currentHealth <= 0f)
                {
                    KillEntity(targetEntity);
                    RemoveDamageEvent(damageEventEntity);
                    continue;
                }

                ApplyInvincibilityIfPlayer(targetEntity);
                SignalBus.Get<DamageSignal>().Invoke(targetEntity, health.currentHealth, health.maxHealth);

                RemoveDamageEvent(damageEventEntity);
            }
        }
        
        private void RemoveDamageEvent(Entity damageEventEntity)
        {
            _damageEventStash.Remove(damageEventEntity);
        }

        private void KillEntity(Entity entity)
        {
            _deadTagStash.Set(entity, new DeadTag());
        }

        private void ApplyInvincibilityIfPlayer(Entity entity)
        {
            foreach (var playerEntity in _player)
            {
                if (playerEntity.Id == entity.Id)
                {
                    _invincibilityStash.Set(entity, new InvincibilityComponent
                    {
                        remainingTime = 2f
                    });
                    break;
                }
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