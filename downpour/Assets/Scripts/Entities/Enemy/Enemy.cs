using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Combat;

namespace Downpour.Entity.Enemy
{
    using Downpour.Entity.Player;

    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IHittable
    {
        HealthSystem _healthSystem;
        [SerializeField] private float _knockbackMultiplier;
        [SerializeField] private float _knockbackCounter;
        [SerializeField] private float _knockbackTime;
        private int _knockbackDirection;

        protected Vector2 _velocity;

        private Rigidbody2D _rb;

        public event Action<Enemy> EnemyDeathEvent;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();

            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.DeathEvent += OnDeath;
        }
        public void OnHit(Player player, int damage, int knockbackDirection) {
            _healthSystem.TakeDamage(damage);
            _knockbackDirection = knockbackDirection;
            _knockbackCounter = _knockbackTime;
        }

        public Vector2 GetSlashEffectPosition() {
            return transform.position;
        }

        public virtual void OnDeath() {
            EnemyDeathEvent?.Invoke(this);
            Destroy(gameObject);
        }

        private void Update() {
            _velocity = _rb.velocity;

            if(_knockbackCounter <= 0) { // TODO: Move To State Machine
                _velocity.x = 0;
            }

            OnUpdate();

            if(_knockbackCounter > 0) {
                _knockbackCounter -= Time.deltaTime;
                _velocity.x = _knockbackDirection * _knockbackMultiplier;
            }

            

            

            _rb.velocity = _velocity;
        }

        public virtual void OnUpdate() {

        }
    }
}
