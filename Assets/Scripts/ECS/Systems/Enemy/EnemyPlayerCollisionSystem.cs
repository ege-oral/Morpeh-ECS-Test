using ECS.Components.Events;
using ECS.Components.Player;
using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems.Enemy
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemyPlayerCollisionSystem : ISystem 
    {
        public World World { get; set;}
        private Filter _playerFilter;
        private Filter _enemyFilter;

        private Stash<TransformComponent> _transformStash;
        private Stash<DamageEvent> _damageEventStash;

        public void OnAwake()
        {
            _playerFilter = World.Filter.With<PlayerTag>().With<TransformComponent>().Without<InvincibilityComponent>().Build();
            _enemyFilter = World.Filter.With<EnemyTag>().With<TransformComponent>().Without<DeadTag>().Without<InactiveTag>().Build();
            _transformStash = World.GetStash<TransformComponent>();
            _damageEventStash = World.GetStash<DamageEvent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var player in _playerFilter)
            {
                ref var playerTransform = ref _transformStash.Get(player);
            
                foreach (var enemy in _enemyFilter)
                {
                    ref var enemyTransform = ref _transformStash.Get(enemy);

                    var distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
                    var collisionDistance = 1f;

                    if (distance <= collisionDistance)
                    {
                        var damageEventEntity = World.CreateEntity();
                        _damageEventStash.Set(damageEventEntity, new DamageEvent
                        {
                            sourceEntity = enemy,
                            targetEntity = player,
                            damageAmount = 1,
                            instantKill = false
                        });
                    }
                }
            }
        }

        public void Dispose()
        {
            _playerFilter = null;
            _enemyFilter = null;
            _transformStash = null;
            _damageEventStash = null;
        }
    }
}