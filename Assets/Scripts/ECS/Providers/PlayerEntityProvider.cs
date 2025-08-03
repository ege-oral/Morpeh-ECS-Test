using ECS.Components.Health;
using ECS.Components.Projectile;
using ECS.Components.Shared;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ECS.Providers
{
    public class PlayerEntityProvider : EntityProvider
    {
        [Header("Player Settings")] 
        [SerializeField] private int playerHealth = 10;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float fireCooldown = 1f;
        [SerializeField] private float fireRange = 20f;

        private readonly World _world = World.Default;
    
        protected override void Initialize()
        {
            _world.GetStash<InputComponent>().Set(Entity, new InputComponent());
            _world.GetStash<MovementComponent>().Set(Entity, new MovementComponent { speed = speed });
            _world.GetStash<TransformComponent>().Set(Entity, new TransformComponent());
            _world.GetStash<HealthComponent>().Set(Entity, new HealthComponent{currentHealth = playerHealth, maxHealth = playerHealth});
        
            _world.GetStash<ShooterComponent>().Set(Entity, new ShooterComponent
            {
                fireCooldown = fireCooldown,
                fireRange = fireRange,
                fireTimer = fireCooldown
            });
        
            _world.GetStash<PlayerTag>().Set(Entity, new PlayerTag());
        }

        public void InitializePosition(Vector3 position, Quaternion rotation)
        {
            ref var transformComponent = ref _world.GetStash<TransformComponent>().Get(Entity);
            transformComponent.position = position;
            transformComponent.rotation = rotation;
        }
    }
}