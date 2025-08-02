using Components;
using Components.Shoot;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Providers
{
    public class PlayerEntityProvider : EntityProvider
    {
        [Header("Player Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float fireCooldown = 1f;
        [SerializeField] private float fireRange = 20f;

        private readonly World _world = World.Default;
    
        protected override void Initialize()
        {
            _world.GetStash<InputComponent>().Set(Entity, new InputComponent());

            _world.GetStash<MovementComponent>().Set(Entity, new MovementComponent { speed = speed });
        
            _world.GetStash<TransformComponent>().Set(Entity, new TransformComponent
            {
                position = transform.position,
                rotation = transform.rotation
            });
        
            _world.GetStash<ShooterComponent>().Set(Entity, new ShooterComponent
            {
                fireCooldown = fireCooldown,
                fireRange = fireRange,
                fireTimer = fireCooldown
            });
        
            _world.GetStash<PlayerTag>().Set(Entity, new PlayerTag());
        }
    }
}