using ECS.Components.Shared;
using Managers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Systems.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ViewSyncSystem : ISystem 
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<TransformComponent> _transform;
        
        private readonly EntityViewManager _entityViewManager;
        
        public ViewSyncSystem(EntityViewManager entityViewManager)
        {
            _entityViewManager = entityViewManager;
        }

        public void OnAwake()
        {
            _transform = World.GetStash<TransformComponent>();
            _filter = World.Filter.With<TransformComponent>().Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var entityTransform = _entityViewManager.GetEntityTransform(entity);
                if (entityTransform == null) continue;
                
                ref var transformComponent = ref _transform.Get(entity);
                entityTransform.position = transformComponent.position;
                entityTransform.rotation = transformComponent.rotation;
            }
        }

        public void Dispose()
        {
        }
    }
}