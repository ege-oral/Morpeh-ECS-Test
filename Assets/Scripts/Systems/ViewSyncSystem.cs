using System.Collections.Generic;
using Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ViewSyncSystem : ISystem 
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<TransformComponent> _transform;
        
        private readonly Dictionary<Entity, Transform> _views;
        
        public ViewSyncSystem(Dictionary<Entity, Transform> views)
        {
            _views = views;
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
                if (_views.TryGetValue(entity, out var view) == false) continue;
                
                ref var transformComponent = ref _transform.Get(entity);
                view.position = transformComponent.position;
                view.rotation = transformComponent.rotation;
            }
        }

        public void Dispose()
        {
        }
    }
}