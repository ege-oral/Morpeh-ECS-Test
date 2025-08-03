using ECS.Components.Projectile;
using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ECS.Systems.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ShooterSystem : ISystem 
    {
        public World World { get; set;}
        private Filter _shooterFilter;
        private Filter _enemyFilter;
        
        private Stash<TransformComponent> _transformStash;
        private Stash<ShooterComponent> _shooterStash;
        private Stash<FireProjectileRequest> _fireRequestStash;
        private Stash<TransformComponent> _enemyTransformStash;

        public void OnAwake() 
        {
            _shooterFilter = World.Filter.With<TransformComponent>().With<ShooterComponent>().Build();
            _enemyFilter = World.Filter.With<EnemyTag>().With<TransformComponent>().Without<DeadTag>().Build();
            
            _transformStash = World.GetStash<TransformComponent>();
            _shooterStash = World.GetStash<ShooterComponent>();
            _fireRequestStash = World.GetStash<FireProjectileRequest>();
            _enemyTransformStash = World.GetStash<TransformComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var shooter in _shooterFilter)
            {
                ref var shooterTransform = ref _transformStash.Get(shooter);
                ref var shooterData = ref _shooterStash.Get(shooter);

                shooterData.fireTimer -= deltaTime;
                if (shooterData.fireTimer > 0f) continue;

                Entity? target = null;
                var closestDistSqr = float.MaxValue;

                foreach (var enemy in _enemyFilter)
                {
                    ref var enemyTransform = ref _enemyTransformStash.Get(enemy);
                    var distSqr = (enemyTransform.position - shooterTransform.position).sqrMagnitude;
                    if (distSqr < shooterData.fireRange * shooterData.fireRange && distSqr < closestDistSqr)
                    {
                        closestDistSqr = distSqr;
                        target = enemy;
                    }
                }

                if (target.HasValue)
                {
                    ref var enemyPos = ref _enemyTransformStash.Get(target.Value);
                    var direction = (enemyPos.position - shooterTransform.position).normalized;

                    _fireRequestStash.Set(shooter, new FireProjectileRequest { direction = direction });

                    shooterData.fireTimer = shooterData.fireCooldown;
                }
            }
        }

        public void Dispose()
        {

        }
    }
}