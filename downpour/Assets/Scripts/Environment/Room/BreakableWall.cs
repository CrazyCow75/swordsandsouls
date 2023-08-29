using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Combat;
using Downpour.Entity.Player;
using System;

namespace Downpour
{
    [RequireComponent(typeof(HealthSystem))]
    public class BreakableWall : MonoBehaviour, IHittable
    {
        public event Action<BreakableWall> BreakableWallBreakEvent;
        [field: SerializeField] private GameObject _breakableWall;
        [field: SerializeField] private GameObject _falseArea;
        private HealthSystem _healthSystem;

        private void Awake() {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.DeathEvent += OnDeath;
        }

        public void OnHit(Player player, int damage, int direction) {
            _healthSystem.TakeDamage(damage);
            return; // TODO: implement taking damage
        }

        public Vector2 GetSlashEffectPosition() {
            return transform.position; // TODO: implement taking damage
        }

        public virtual void OnDeath() {
            BreakableWallBreakEvent?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
