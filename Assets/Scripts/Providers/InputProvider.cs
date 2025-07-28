using Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputProvider : MonoProvider<InputComponent>
    {
        [SerializeField] private float movementSpeed = 5f;
        
        private Stash<MovementComponent> _stash;

        private void Awake()
        {
            _stash = World.Default.GetStash<MovementComponent>();
        }

        private void Update()
        {
            if (_stash.Has(Entity) == false) return;
            
            ref var direction = ref _stash.Get(Entity).direction;

            transform.position += direction * (movementSpeed * Time.deltaTime);
        }
    }
}