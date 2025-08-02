using Components;
using Components.Projectile;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Systems.Projectile
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileMovementSystem : ISystem 
    {
        public World World { get; set;}
        private Filter _filter;

        private Stash<TransformComponent> _transformStash;
        private Stash<ProjectileComponent> _projectileStash;

        public void OnAwake()
        {
            _filter = World.Filter.With<ProjectileComponent>().With<TransformComponent>().Build();
            _transformStash = World.GetStash<TransformComponent>();
            _projectileStash = World.GetStash<ProjectileComponent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var projectile in _filter)
            {
                ref var projectileTransform = ref _transformStash.Get(projectile);
                ref var projectileComponent = ref _projectileStash.Get(projectile);
                
                projectileTransform.position +=  projectileComponent.direction * projectileComponent.speed * deltaTime;
            }
        }

        public void Dispose()
        {

        }
    }
}