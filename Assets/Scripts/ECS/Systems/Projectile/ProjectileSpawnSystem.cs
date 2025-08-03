using ECS.Components.Projectile;
using ECS.Components.Shared;
using ECS.Providers;
using Managers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems.Projectile
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileSpawnSystem : ISystem
    {
        public World World { get; set; }
        private Filter _shooterFilter;
        private Stash<FireProjectileRequest> _fireRequestStash;
        private Stash<TransformComponent> _transformStash;
        
        private readonly EntityViewManager _entityViewManager;
        
        public ProjectileSpawnSystem(EntityViewManager entityViewManager)
        {
            _entityViewManager = entityViewManager;
        }

        public void OnAwake()
        {
            _shooterFilter = World.Filter.With<FireProjectileRequest>().With<TransformComponent>().Build();
            _fireRequestStash = World.GetStash<FireProjectileRequest>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var shooter in _shooterFilter)
            {
                ref var fireRequest = ref _fireRequestStash.Get(shooter);
                ref var shooterTransform = ref _transformStash.Get(shooter);
                var projectTileEntity = _entityViewManager.GetPooledEntity("Projectile");
                var projectTileEntityTransform = _entityViewManager.GetEntityTransform(projectTileEntity);
                if(projectTileEntityTransform == null) continue;
                
                var projectileEntityProvider = projectTileEntityTransform.GetComponent<ProjectileEntityProvider>();
                if(projectileEntityProvider == null) continue;

                projectileEntityProvider.InitializeProjectile(shooterTransform.position,
                    Quaternion.LookRotation(fireRequest.direction), fireRequest.direction);
                
                _fireRequestStash.Remove(shooter);
            }
        }

        public void Dispose()
        {
            _shooterFilter = null;
            _fireRequestStash = null;
            _transformStash = null;
        }
    }
}