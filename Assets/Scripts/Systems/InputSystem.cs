using Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputSystem : ISystem 
    {
        public World World { get; set;}
        private Filter _filter;
        private Stash<InputComponent> _inputComponents;
        private Stash<MovementComponent> _movementComponents;
        private Stash<TransformComponent>  _transformComponents;

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InputComponent>()
                .With<MovementComponent>()
                .With<TransformComponent>()
                .Build();

            _inputComponents = World.GetStash<InputComponent>();
            _movementComponents = World.GetStash<MovementComponent>();
            _transformComponents = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var direction = new Vector3(horizontal, 0f, vertical).normalized;

            foreach (var entity in _filter)
            {
                ref var input = ref _inputComponents.Get(entity);
                ref var verticalInput = ref input.verticalInput;
                verticalInput = vertical;
                
                ref var horizontalInput = ref input.horizontalInput;
                horizontalInput = horizontal;
                
                ref var move = ref _movementComponents.Get(entity);

                input.horizontalInput = horizontal;
                input.verticalInput = vertical;

                move.direction = direction;
            }

        }

        public void Dispose()
        {

        }
    }
}