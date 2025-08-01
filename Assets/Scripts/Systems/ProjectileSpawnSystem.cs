using System.Collections.Generic;
using Components;
using Components.Shoot;
using Providers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Systems
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
        
        private readonly GameObject _projectilePrefab;
        private readonly Dictionary<Entity, Transform> _entityViews;
        
        public ProjectileSpawnSystem(GameObject projectilePrefab, Dictionary<Entity, Transform> entityViews)
        {
            _projectilePrefab = projectilePrefab;
            _entityViews = entityViews;
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
                var view = Object.Instantiate(_projectilePrefab, shooterTransform.position, Quaternion.LookRotation(fireRequest.direction));

                var transformProvider = view.GetComponent<TransformProvider>();
                ref var transformData = ref transformProvider.GetData();
                transformData = new TransformComponent
                {
                    position = shooterTransform.position,
                    rotation = Quaternion.LookRotation(fireRequest.direction)
                };
                
                var projectileProvider = view.GetComponent<ProjectileProvider>();
                ref var projectileData = ref projectileProvider.GetData();
                projectileData = new ProjectileComponent
                {
                    direction = fireRequest.direction,
                    speed = 10f
                };
                
                var projectile = World.CreateEntity();
                World.GetStash<TransformComponent>().Set(projectile, transformData);
                World.GetStash<ProjectileComponent>().Set(projectile, projectileData);
                
                _entityViews[projectile] = view.transform;
                _fireRequestStash.Remove(shooter);
            }
        }

        public void Dispose()
        {
        }
    }
}