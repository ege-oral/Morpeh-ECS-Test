using Components;
using Managers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ReturnEntityToPoolSystem : ISystem 
    {
        public World World { get; set;}
        private Filter _filter;
        private Stash<PoolableEntity> _poolStash;
        
        private readonly EntityViewManager _entityViewManager;
        public ReturnEntityToPoolSystem(EntityViewManager entityViewManager)
        {
            _entityViewManager = entityViewManager;
        }
        
        public void OnAwake()
        {
            _filter = World.Filter.With<PoolableEntity>().With<DeadTag>().Build();
            _poolStash = World.GetStash<PoolableEntity>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter)
            {
                ref var poolName = ref _poolStash.Get(entity).poolName;
                _entityViewManager.ReturnEntity(poolName, entity);
            }
        }

        public void Dispose()
        {

        }
    }
}