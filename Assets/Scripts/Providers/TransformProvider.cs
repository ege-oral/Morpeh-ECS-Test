using Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TransformProvider : MonoProvider<TransformComponent> 
    {
        private Stash<TransformComponent> _transformStash;

        private void Start()
        {
            _transformStash = World.Default.GetStash<TransformComponent>();
        }

        private void Update()
        {
            if (_transformStash.Has(Entity) == false) return;
            
            ref var transformComponent = ref _transformStash.Get(Entity);
            transform.position = transformComponent.position;
        }
    }
}