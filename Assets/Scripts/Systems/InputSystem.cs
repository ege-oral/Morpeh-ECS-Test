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

        public void OnAwake()
        {
            _filter = World.Filter.With<InputComponent>().Build();
            _inputComponents = World.GetStash<InputComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
        
            foreach (var entity in _filter)
            {
                ref var input = ref _inputComponents.Get(entity);
        
                input.horizontalInput = horizontal;
                input.verticalInput = vertical;
            }
        }

        public void Dispose()
        {
            _filter = null;
            _inputComponents = null;
        }
    }
}