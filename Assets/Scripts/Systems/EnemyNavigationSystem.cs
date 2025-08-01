using Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemyNavigationSystem : ISystem 
    {
        public World World { get; set; }

        private Filter _playerFilter;
        private Filter _enemyFilter;

        private Stash<TransformComponent> _transformStash;
        private Stash<MovementComponent> _movementStash;
        
        public void OnAwake()
        {
            _playerFilter = World.Filter.With<PlayerTag>().With<TransformComponent>().Build();
            _enemyFilter = World.Filter.With<EnemyTag>().With<TransformComponent>().With<MovementComponent>().Build();

            _transformStash = World.GetStash<TransformComponent>();
            _movementStash = World.GetStash<MovementComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            var playerPosition = Vector3.zero;
            foreach (var playerEntity in _playerFilter)
            {
                ref var playerTransform = ref _transformStash.Get(playerEntity);
                playerPosition = playerTransform.position;
                break;
            }

            foreach (var enemyEntity in _enemyFilter)
            {
                ref var enemyTransform = ref _transformStash.Get(enemyEntity);
                ref var enemyMovement = ref _movementStash.Get(enemyEntity);

                var direction = (playerPosition - enemyTransform.position).normalized;
                enemyMovement.direction = direction;

                enemyTransform.position += direction * enemyMovement.speed * deltaTime;
            }
        }

        public void Dispose()
        {

        }
    }
}