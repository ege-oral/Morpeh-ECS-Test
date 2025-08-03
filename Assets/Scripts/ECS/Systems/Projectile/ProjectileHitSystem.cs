using ECS.Components.Projectile;
using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;

namespace ECS.Systems.Projectile
{
    public sealed class ProjectileHitSystem : ISystem
    {
        private Filter _projectileFilter;
        private Filter _enemyFilter;

        private Stash<TransformComponent> _transformStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<DeadTag> _deadStash;

        public World World { get; set; }

        public void OnAwake()
        {
            _projectileFilter = World.Filter.With<TransformComponent>().With<ProjectileComponent>().Build();
            _enemyFilter = World.Filter.With<TransformComponent>().With<HealthComponent>().With<EnemyTag>().Without<DeadTag>().Build();

            _transformStash = World.GetStash<TransformComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _deadStash = World.GetStash<DeadTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var projectile in _projectileFilter)
            {
                if(_deadStash.Has(projectile)) continue;
                
                ref var projTransform = ref _transformStash.Get(projectile);
                foreach (var enemy in _enemyFilter)
                {
                    ref var enemyTransform = ref _transformStash.Get(enemy);
                    ref var enemyHealth = ref _healthStash.Get(enemy);

                    var sqrDistance = (enemyTransform.position - projTransform.position).sqrMagnitude;
                    var hitRadius = 1f;

                    if (sqrDistance <= hitRadius * hitRadius == false) continue;
                    
                    enemyHealth.healthPoints -= 1;
                    _deadStash.Set(projectile, new DeadTag());
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}