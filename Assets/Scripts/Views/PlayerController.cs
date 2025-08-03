using Core;
using Core.Signal;
using UnityEngine;

namespace Views
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Renderer playerRenderer;

        private void Awake()
        {
            SignalBus.Get<InvincibilitySignal>().Subscribe(OnInvincibilitySignal);
        }

        private void OnInvincibilitySignal(bool isInvincible)
        {
            playerRenderer.material.color = isInvincible ? Color.grey : Color.green;
        }

        private void OnDestroy()
        {
            SignalBus.Get<InvincibilitySignal>().Unsubscribe(OnInvincibilitySignal);
        }
    }
}
