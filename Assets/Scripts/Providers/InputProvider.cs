using Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputProvider : MonoProvider<InputComponent>
    {
        private Stash<InputComponent> _stash;

        private void Awake()
        {
            _stash = World.Default.GetStash<InputComponent>();
        }
        

        /// <summary>
        /// Read-only access to live ECS component.
        /// </summary>
        public bool TryGetReadOnly(out InputComponent data)
        {
            if (_stash.Has(Entity))
            {
                data = _stash.Get(Entity);
                return true;
            }

            data = default;
            return false;
        }

        /// <summary>
        /// Sync ECS component into serialized MonoBehaviour field for inspector.
        /// </summary>
        [ContextMenu("Sync ECS → component (for Inspector)")]
        public void SyncFromEcs()
        {
            if (_stash.Has(this.Entity))
            {
                Debug.Log($"[InputProvider] Synced from ECS → component.");
            }
            else
            {
                Debug.LogWarning("[InputProvider] Entity does not have InputComponent.");
            }
        }

        /// <summary>
        /// Print live ECS input values.
        /// </summary>
        [ContextMenu("Print ECS Input Values")]
        private void PrintInput()
        {
            if (_stash.Has(this.Entity))
            {
                var input = _stash.Get(this.Entity);
            }
            else
            {
                Debug.LogWarning("[InputProvider] InputComponent not found.");
            }
        }
    }
}