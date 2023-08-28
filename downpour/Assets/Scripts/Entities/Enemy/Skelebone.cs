using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class Skelebone : Enemy
    {
        public float idleTime;
        public float attackTime;
        public Animator animator;
        public int damage;
        public BoxCollider2D hitArea;

        [SerializeField] private float idleCounter;
        [SerializeField] private float attackCounter;
        public bool canDamagePlayer;

        void Start() {
            idleCounter = idleTime;
            attackCounter = 0f;
        }

        public override void OnUpdate() {
            if(canDamagePlayer) {
                Collider2D c = Physics2D.OverlapBox(hitArea.offset * transform.localScale.x + (Vector2)(hitArea.transform.position), hitArea.size, 0f, Layers.PlayerLayer);

                if(c!=null) {
                    if(c.transform.TryGetComponent(out Player player)) {
                        player.PlayerStatsController.TakeDamage(damage);
                    }
                }
            }
            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
                return;
            }
            if(attackCounter > 0f) {
                attackCounter -= Time.deltaTime;
                return;
            }
            if(canDamagePlayer) {
                idleCounter = idleTime;
                canDamagePlayer = false;
            } else {
                attackCounter = attackTime;
                canDamagePlayer = true;
                animator.SetTrigger("attack");
            }
        }
    }
}
