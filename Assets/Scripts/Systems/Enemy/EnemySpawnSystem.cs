using Managers;
using Providers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Systems.Enemy
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemySpawnSystem : ISystem 
    {
        public World World { get; set;}
        private readonly EntityViewManager _entityViewManager;

        private float _spawnTimer = 5f;

        public EnemySpawnSystem(EntityViewManager entityViewManager)
        {
            _entityViewManager = entityViewManager;
        }

        public void OnAwake() 
        {

        }

        public void OnUpdate(float deltaTime) 
        {
            _spawnTimer -= deltaTime;
            if(_spawnTimer > 0f) return;

            _spawnTimer = 5f;
            
            var enemyEntity = _entityViewManager.GetPooledEntity("Enemy");
            var enemyEntityTransform = _entityViewManager.GetEntityTransform(enemyEntity);

            var enemyEntityProvider = enemyEntityTransform.GetComponent<EnemyEntityProvider>();
            if(enemyEntityProvider  == null) return;
            enemyEntityProvider.InitializePosition(GetRandomPosition(Vector3.zero, new Vector3(20f, 20f, 0f)),
                Quaternion.identity);
        }
        
        private static Vector3 GetRandomPosition(Vector3 center, Vector3 size)
        {
            return new Vector3(
                Random.Range(center.x - size.x / 2f, center.x + size.x / 2f),
                Random.Range(center.y - size.y / 2f, center.y + size.y / 2f),
                Random.Range(center.z - size.z / 2f, center.z + size.z / 2f)
            );
        }

        public void Dispose()
        {

        }
    }
}