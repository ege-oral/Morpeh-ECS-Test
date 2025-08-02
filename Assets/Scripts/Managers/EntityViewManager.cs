using System;
using System.Collections.Generic;
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
                Debug.LogWarning($"Pool {poolName} already exists!");
                return;
            }

            var pool = new ObjectPool(prefab, size, configureEntity, _world);
            _pools[poolName] = pool;
        }

        public Entity GetPooledEntity(string poolName)
        {
            if (_pools.TryGetValue(poolName, out var pool) == false)
            {
                Debug.LogError($"Pool {poolName} not found!");
                return default;
            }

            var (entity, transform) = pool.Get();
            _entityToTransform[entity] = transform;
            return entity;
        }

        public void ReturnEntity(Entity entity, string poolName)
        {
            if (_pools.TryGetValue(poolName, out var pool) == false)
            {
                Debug.LogError($"Pool {poolName} not found!");
                return;
            }

            if (_entityToTransform.TryGetValue(entity, out var transform))
            {
                pool.Return(entity, transform);
                _entityToTransform.Remove(entity);
            }
        }

        public void RegisterEntityView(Entity entity, Transform transform)
        {
            _entityToTransform[entity] = transform;
        }

        public void UnregisterEntityView(Entity entity)
        {
            _entityToTransform.Remove(entity);
        }

        public Transform GetEntityTransform(Entity entity)
        {
            return _entityToTransform.GetValueOrDefault(entity);
        }

        public Dictionary<Entity, Transform> GetEntityTransformMap()
        {
            return _entityToTransform;
        }

        public void Dispose()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Dispose();
            }
            _pools.Clear();
            _entityToTransform.Clear();
        }

        private class ObjectPool : IDisposable
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

                var poolParentGo = new GameObject($"{prefab.name}_Pool");
                _poolParent = poolParentGo.transform;

                for (var i = 0; i < size; i++)
                {
                    var go = UnityEngine.Object.Instantiate(prefab, _poolParent);
                    var entityProvider = go.GetComponent<EntityProvider>();
                
                    _configureEntity?.Invoke(entityProvider.Entity, go);
                
                    go.SetActive(false);
                    _pool.Enqueue((entityProvider.Entity, go.transform));
                }
            }

            public (Entity entity, Transform transform) Get()
            {
                if (_pool.Count > 0)
                {
                    var (entity, transform) = _pool.Dequeue();
                    transform.gameObject.SetActive(true);
                    return (entity, transform);
                }

                var newGo = UnityEngine.Object.Instantiate(_prefab);
                var newEntity = _world.CreateEntity();
                _configureEntity?.Invoke(newEntity, newGo);
            
                return (newEntity, newGo.transform);
            }

            public void Return(Entity entity, Transform transform)
            {
                transform.gameObject.SetActive(false);
                transform.SetParent(_poolParent);
                _pool.Enqueue((entity, transform));
            }

            public void Dispose()
            {
                while (_pool.Count > 0)
                {
                    var (entity, transform) = _pool.Dequeue();
                    if (transform != null)
                    {
                        UnityEngine.Object.Destroy(transform.gameObject);
                    }
                    _world.RemoveEntity(entity);
                }

                if (_poolParent != null)
                {
                    UnityEngine.Object.Destroy(_poolParent.gameObject);
                }
            }
        }
    }
}