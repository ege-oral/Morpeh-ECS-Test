using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerMovementSystem : ISystem 
    {
        public World World { get; set;}

        private Filter _filter;
        private Stash<InputComponent> _inputStash;
        private Stash<MovementComponent> _movementStash;
        private Stash<TransformComponent> _transformStash;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<MovementComponent>()
                .With<TransformComponent>()
                .With<PlayerTag>()
                .Build();
            
            _inputStash = World.GetStash<InputComponent>();
            _movementStash = World.GetStash<MovementComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter) 
            {
                ref var input = ref _inputStash.Get(entity);
                ref var move = ref _movementStash.Get(entity);
                ref var transform = ref _transformStash.Get(entity);

                var moveDir = new Vector3(input.horizontalInput, input.verticalInput, 0f).normalized;
                move.direction = moveDir;
                transform.position += moveDir * move.speed * deltaTime;
            }
        }

        public void Dispose()
        {
            _filter = null;
            _inputStash = null;
            _movementStash = null;
            _transformStash = null;
        }
    }
}