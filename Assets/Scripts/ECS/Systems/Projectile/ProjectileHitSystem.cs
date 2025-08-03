using ECS.Components.Events;
using ECS.Components.Health;
using ECS.Components.Projectile;
using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Systems.Projectile
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileHitSystem : ISystem
    {
        private Filter _projectileFilter;
        private Filter _enemyFilter;

        private Stash<ProjectileComponent> _projectileStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DamageEvent> _damageEventStash;
        
        private const float HitRadius = 1f;

        public World World { get; set; }

        public void OnAwake()
        {
            _projectileFilter = World.Filter.With<TransformComponent>().With<ProjectileComponent>().Build();
            _enemyFilter = World.Filter.With<TransformComponent>().With<HealthComponent>().With<EnemyTag>().Without<DeadTag>().Build();

            _projectileStash = World.GetStash<ProjectileComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _damageEventStash = World.GetStash<DamageEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var projectile in _projectileFilter)
            {
                ref var projTransform = ref _transformStash.Get(projectile);
                foreach (var enemy in _enemyFilter)
                {
                    ref var enemyTransform = ref _transformStash.Get(enemy);
                    var sqrDistance = (enemyTransform.position - projTransform.position).sqrMagnitude;

                    if (sqrDistance <= HitRadius == false) continue;

                    var damageEventForEnemy = World.CreateEntity();
                    _damageEventStash.Set(damageEventForEnemy, new DamageEvent
                    {
                        sourceEntity = projectile,
                        targetEntity = enemy,
                        damageAmount = 1,
                        instantKill = false
                    });
                    
                    var damageEventForProjectile = World.CreateEntity();
                    _damageEventStash.Set(damageEventForProjectile, new DamageEvent
                    {
                        sourceEntity = enemy,
                        targetEntity = projectile,
                        damageAmount = 999,
                        instantKill = true
                    });

                    _projectileStash.Remove(projectile);
                }
            }
        }

        public void Dispose()
        {
            _projectileFilter = null;
            _enemyFilter = null;
            _transformStash = null;
            _projectileStash = null;
        }
    }
}