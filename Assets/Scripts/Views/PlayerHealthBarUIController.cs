using Core;
using Core.Signal;
using ECS.Components.Tags;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class PlayerHealthBarUIController : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;
        
        private int _playerEntityId;
        
        private void Awake()
        {
            _playerEntityId = FindPlayerEntityId();
            healthBar.value = 1f;
            SignalBus.Get<DamageSignal>().Subscribe(OnDamageSignal);
        }

        private void OnDamageSignal(Entity entity, int currentHealth, int maxHealth)
        {
            if (entity.Id != _playerEntityId) return;
            
            healthBar.value = (float)currentHealth / maxHealth;
        }

        private void OnDestroy()
        {
            SignalBus.Get<DamageSignal>().Unsubscribe(OnDamageSignal);
        }

        private int FindPlayerEntityId()
        {
            var world = World.Default;
            if (world != null)
            {
                var playerFilter = world.Filter.With<PlayerTag>().Build();
                foreach (var entity in playerFilter)
                {
                    return entity.Id;
                }
            }
            return -1;
        }
    }
}