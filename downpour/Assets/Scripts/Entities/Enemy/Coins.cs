using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Combat;
using System;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class Coins : MonoBehaviour, IHittable
    {
        public event Action<Coins> CoinDeathEvent;
        private HealthSystem _healthSystem;
         private void Awake() {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.DeathEvent += OnDeath;
        }
        public void OnHit(Player player, int damage, int knockbackDirection) {
            _healthSystem.TakeDamage(damage);

        }
        private void OnDeath() {
            CoinDeathEvent?.Invoke(this);
            Destroy(gameObject);
        }

        public Vector2 GetSlashEffectPosition() {
            return transform.position;
        }
    }
}
