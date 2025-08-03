using System;
using System.Collections.Generic;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Managers
{
    public class EntityViewManager : IDisposable
    {
        private readonly World _world = World.Default;
        private readonly Dictionary<Entity, Transform> _entityToTransform = new();
        private readonly Dictionary<string, ObjectPool> _pools = new();
        
        public void InitializePool(string poolName, GameObject prefab, int size, Action<Entity, GameObject> configureEntity)
        {
            if (_pools.ContainsKey(poolName))
            {
                Debug.LogWarning($"Pool '{poolName}' already exists.");
                return;
            }

            _pools[poolName] = new ObjectPool(prefab, size, configureEntity, _world);
        }

        public Entity GetPooledEntity(string poolName)
        {
            if (!_pools.TryGetValue(poolName, out var pool))
            {
                Debug.LogError($"Pool '{poolName}' not found.");
                return default;
            }

            var (entity, transform) = pool.Get();
            _entityToTransform[entity] = transform;
            return entity;
        }

        public void ReturnEntity(string poolName, Entity entity)
        {
            if (!_pools.TryGetValue(poolName, out var pool))
            {
                Debug.LogError($"Pool '{poolName}' not found.");
                return;
            }

            if (_entityToTransform.Remove(entity, out var transform))
            {
                pool.Return(entity, transform);
            }
        }

        public void RegisterEntityView(Entity entity, Transform transform) => _entityToTransform[entity] = transform;
        public void UnregisterEntityView(Entity entity) => _entityToTransform.Remove(entity);
        public Transform GetEntityTransform(Entity entity) => _entityToTransform.GetValueOrDefault(entity);
        public IReadOnlyDictionary<Entity, Transform> GetEntityTransformMap() => _entityToTransform;

        public void Dispose()
        {
            foreach (var pool in _pools.Values)
                pool.Dispose();

            _pools.Clear();
            _entityToTransform.Clear();
        }
        
        private sealed class ObjectPool : IDisposable
        {
            private readonly Queue<(Entity entity, Transform transform)> _pool = new();
            private readonly GameObject _prefab;
            private readonly Action<Entity, GameObject> _configureEntity;
            private readonly World _world;
            private readonly Transform _poolParent;

            public ObjectPool(GameObject prefab, int size, Action<Entity, GameObject> configureEntity, World world)
            {
                _prefab = prefab;
                _configureEntity = configureEntity;
                _world = world;

                _poolParent = new GameObject($"{prefab.name}_Pool").transform;

                for (var i = 0; i < size; i++)
                {
                    _pool.Enqueue(CreatePooledInstance());
                }
            }

            public (Entity entity, Transform transform) Get()
            {
                if (_pool.Count > 0)
                {
                    var (entity, transform) = _pool.Dequeue();
                    transform.gameObject.SetActive(true);
                    _world.GetStash<DeadTag>().Remove(entity);
                    return (entity, transform);
                }

                return CreatePooledInstance();
            }

            public void Return(Entity entity, Transform transform)
            {
                transform.SetParent(_poolParent);
                transform.gameObject.SetActive(false);
                _pool.Enqueue((entity, transform));
            }

            public void Dispose()
            {
                while (_pool.Count > 0)
                {
                    var (entity, transform) = _pool.Dequeue();
                    UnityEngine.Object.Destroy(transform.gameObject);
                    _world.RemoveEntity(entity);
                }

                UnityEngine.Object.Destroy(_poolParent.gameObject);
            }

            private (Entity entity, Transform transform) CreatePooledInstance()
            {
                var go = UnityEngine.Object.Instantiate(_prefab, _poolParent);
                var entityProvider = go.GetComponent<EntityProvider>();
                var entity = entityProvider.Entity;

                _world.GetStash<DeadTag>().Set(entity, new DeadTag());
                _configureEntity?.Invoke(entity, go);

                go.SetActive(false);
                return (entity, go.transform);
            }
        }
    }
}