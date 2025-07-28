using System.Collections.Generic;
using Components;
using Providers;
using Scellecs.Morpeh;
using Systems;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    
    private World _world;
    private SystemsGroup _systems;
    private readonly Dictionary<Entity, Transform> _entityToTransform = new();
    
    private void Start()
    {
        _world = World.Default;

        _systems = _world.CreateSystemsGroup();
        _systems.AddSystem(new HealthSystem());
        _systems.AddSystem(new InputSystem());
        _systems.AddSystem(new PlayerMovementSystem());
        _systems.AddSystem(new EnemyNavigationSystem());
        _systems.Initialize();
        _world.AddSystemsGroup(order: 0, _systems);

        CreatePlayerEntity();
        CreateEnemyEntity();
    }
    
    private void Update() 
    {
        _systems.Update(Time.deltaTime);
    }

    private void CreatePlayerEntity() 
    {
        var playerGo = Instantiate(playerPrefab);
        var entity = _world.CreateEntity();

        var inputProvider = playerGo.GetComponent<InputProvider>();
        ref var inputData = ref inputProvider.GetData();
        inputData = new InputComponent { horizontalInput = 0f, verticalInput = 0f };
        
        var movementProvider = playerGo.GetComponent<MovementProvider>();
        ref var movementData = ref movementProvider.GetData();
        movementData = new MovementComponent { speed = 5f };
        
        var transformProvider = playerGo.GetComponent<TransformProvider>();
        ref var transformData = ref transformProvider.GetData();
        transformData = new TransformComponent();
        
        var playerTagProvider = playerGo.GetComponent<PlayerTagProvider>();
        ref var playerTagData = ref playerTagProvider.GetData();
        playerTagData = new PlayerTag();
        
        _world.GetStash<InputComponent>().Set(entity, inputData);
        _world.GetStash<MovementComponent>().Set(entity, movementData);
        _world.GetStash<TransformComponent>().Set(entity, transformData);
        _world.GetStash<PlayerTag>().Set(entity, playerTagData);

        _entityToTransform[entity] = playerGo.transform;
    }

    private void CreateEnemyEntity()
    {
        var enemyGo = Instantiate(enemyPrefab, Vector3.one, Quaternion.identity);
        var entity = _world.CreateEntity();
        
        var movementProvider = enemyGo.GetComponent<MovementProvider>();
        ref var movementData = ref movementProvider.GetData();
        movementData = new MovementComponent { speed = 3f };
        
        var transformProvider = enemyGo.GetComponent<TransformProvider>();
        ref var transformData = ref transformProvider.GetData();
        transformData = new TransformComponent();
        
        var enemyTagProvider = enemyGo.GetComponent<EnemyTagProvider>();
        ref var enemyTagData = ref enemyTagProvider.GetData();
        enemyTagData = new EnemyTag();
        
        _world.GetStash<MovementComponent>().Set(entity, movementData);
        _world.GetStash<TransformComponent>().Set(entity, transformData);
        _world.GetStash<EnemyTag>().Set(entity, enemyTagData);

        _entityToTransform[entity] = enemyGo.transform;
    }

    private void OnDestroy() 
    {
        _systems.Dispose();
        _world.Dispose();
    }
}
