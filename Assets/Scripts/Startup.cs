using Managers;
using Providers;
using Scellecs.Morpeh;
using Systems;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject projectilePrefab;
    
    private World _world;
    private SystemsGroup _systems;
    private EntityViewManager _entityViewManager;
    
    private void Start()
    {
        _world = World.Default;
        _entityViewManager = new EntityViewManager();

        _systems = _world.CreateSystemsGroup();
        _systems.AddSystem(new HealthSystem());
        _systems.AddSystem(new InputSystem());
        _systems.AddSystem(new PlayerMovementSystem());
        _systems.AddSystem(new EnemyNavigationSystem());
        _systems.AddSystem(new ShooterSystem());
        _systems.AddSystem(new ProjectileSpawnSystem(_entityViewManager));
        _systems.AddSystem(new ProjectileMovementSystem());
        _systems.AddSystem(new ViewSyncSystem(_entityViewManager));
        _systems.Initialize();
        _world.AddSystemsGroup(order: 0, _systems);

        InitializePools();
        CreateInitialEntities();
    }
    
    private void Update() 
    {
        _systems.Update(Time.deltaTime);
    }

    private void CreateInitialEntities()
    {
        CreatePlayerEntity();
        CreateEnemyEntity();
    }
    
    private void CreatePlayerEntity()
    {
        var playerGo = Instantiate(playerPrefab);
        var playerProvider = playerGo.GetComponent<PlayerEntityProvider>();
        
        if (playerProvider != null)
        {
            _entityViewManager.RegisterEntityView(playerProvider.Entity, playerGo.transform);
        }
    }

    private void CreateEnemyEntity()
    {
        var entity = _entityViewManager.GetPooledEntity("Enemy");
        var enemyTransform = _entityViewManager.GetEntityTransform(entity);
        var enemyEntityProvider = enemyTransform.GetComponent<EnemyEntityProvider>();
        enemyEntityProvider.InitializePosition(new Vector3(5f, 5f, 0), Quaternion.identity);
    }
    
    private void InitializePools()
    {
        _entityViewManager.InitializePool("Enemy", enemyPrefab, 1, null);
        _entityViewManager.InitializePool("Projectile", projectilePrefab, 100, null);
    }
    
    
    private void OnDestroy() 
    {
        _systems.Dispose();
        _world.Dispose();
    }
}
