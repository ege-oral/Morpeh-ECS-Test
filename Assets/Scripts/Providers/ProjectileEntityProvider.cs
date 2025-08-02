using Components;
using Components.Projectile;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Providers
{
    public class ProjectileEntityProvider : EntityProvider
    {
        [SerializeField] private float projectileSpeed;
    
        private readonly World _world = World.Default;
    
        protected override void Initialize()
        {
            _world.GetStash<TransformComponent>().Set(Entity, new TransformComponent());
            _world.GetStash<PoolableEntity>().Set(Entity, new PoolableEntity { poolName = "Projectile" });
            _world.GetStash<ProjectileComponent>().Set(Entity, new ProjectileComponent { speed = projectileSpeed });
        }

        public void InitializeProjectile(Vector3 position, Quaternion rotation, Vector3 direction)
        {
            ref var transformComponent = ref _world.GetStash<TransformComponent>().Get(Entity);
            transformComponent.position = position;
            transformComponent.rotation = rotation;
        
            ref var projectileComponent = ref _world.GetStash<ProjectileComponent>().Get(Entity);
            projectileComponent.direction = direction;
        }
    }
}