using Managers;
using Providers;
using Scellecs.Morpeh;
using Systems;
using Systems.Enemy;
using Systems.Player;
using Systems.Projectile;
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
        _systems.AddSystem(new EnemySpawnSystem(_entityViewManager));
        _systems.AddSystem(new ProjectileMovementSystem());
        _systems.AddSystem(new ViewSyncSystem(_entityViewManager));
        _systems.AddSystem(new ProjectileHitSystem());
        _systems.AddSystem(new ReturnEntityToPoolSystem(_entityViewManager));
        
        _systems.Initialize();
        _world.AddSystemsGroup(order: 0, _systems);

        InitializePools();
        CreateInitialEntities();
    }
    
    private void Update() 
    {
        _systems.Update(Time.deltaTime);
    }
    
    private void InitializePools()
    {
        _entityViewManager.InitializePool("Enemy", enemyPrefab, 5, null);
        _entityViewManager.InitializePool("Projectile", projectilePrefab, 10, null);
    }
    
    private void CreateInitialEntities()
    {
        CreatePlayerEntity(new Vector3(0f, 0f, 0f), Quaternion.identity);
        CreateEnemyEntity();
    }
    
    private void CreatePlayerEntity(Vector3 position, Quaternion rotation)
    {
        var playerGo = Instantiate(playerPrefab);
        var playerProvider = playerGo.GetComponent<PlayerEntityProvider>();
        playerProvider.InitializePosition(position, rotation);
        _entityViewManager.RegisterEntityView(playerProvider.Entity, playerGo.transform);
    }

    private void CreateEnemyEntity()
    {
        var entity = _entityViewManager.GetPooledEntity("Enemy");
        var enemyTransform = _entityViewManager.GetEntityTransform(entity);
        var enemyEntityProvider = enemyTransform.GetComponent<EnemyEntityProvider>();
        enemyEntityProvider.InitializePosition(new Vector3(5f, 5f, 0), Quaternion.identity);
    }
    
    private void OnDestroy() 
    {
        _systems.Dispose();
        _world.Dispose();
    }
}
