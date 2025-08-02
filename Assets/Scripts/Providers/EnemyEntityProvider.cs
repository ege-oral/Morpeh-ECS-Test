using Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Providers
{
    public class EnemyEntityProvider : EntityProvider
    {
        [Header("Enemy Settings")] 
        [SerializeField] private float speed = 1f;

        private readonly World _world = World.Default;

        protected override void Initialize()
        {
            _world.GetStash<MovementComponent>().Set(Entity, new MovementComponent { speed = speed });
            _world.GetStash<TransformComponent>().Set(Entity, new TransformComponent());
            _world.GetStash<EnemyTag>().Set(Entity, new EnemyTag());
        }

        public void InitializePosition(Vector3 position, Quaternion rotation)
        {
            ref var transformComponent = ref _world.GetStash<TransformComponent>().Get(Entity);
            transformComponent.position = position;
            transformComponent.rotation = rotation;
        }
    }
}