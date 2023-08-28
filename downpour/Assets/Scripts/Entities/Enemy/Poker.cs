using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class Poker : Enemy
    {
        public float idleTime;
        public float attackTime;

        public float windupTime;
        public Animator animator;
        public int damage;
        public BoxCollider2D hitArea;

        [SerializeField] private float idleCounter;
        [SerializeField] private float attackCounter;
        [SerializeField] private float windupCounter;
        public bool canDamagePlayer;
        public float speed;

        void Start() {
            idleCounter = idleTime;
            attackCounter = 0f;
        }

        public override void OnUpdate() {
            if(windupCounter > 0f) {
                windupCounter -= Time.deltaTime;

                transform.LookAt(Player.Instance.transform.position);
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
            }
            if(canDamagePlayer) {
                Collider2D c = Physics2D.OverlapBox(hitArea.offset * transform.localScale.x + (Vector2)(hitArea.transform.position), hitArea.size, 0f, Layers.PlayerLayer);

                if(c!=null) {
                    if(c.transform.TryGetComponent(out Player player)) {
                        player.PlayerStatsController.TakeDamage(damage);
                    }
                }

                
                if(!(windupCounter > 0f))
                    transform.Translate(new Vector3(speed*Time.deltaTime, 0, 0));
            }
            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
                
                _velocity = new Vector3(0f, 0f, 0f);
                transform.localScale = new Vector2((Player.Instance.transform.position.x > transform.position.x ? -1f : 1f), 1f);
                return;
            }
            if(attackCounter > 0f) {
                attackCounter -= Time.deltaTime;
                return;
            }
            if(canDamagePlayer) {
                idleCounter = idleTime;
                canDamagePlayer = false;
                transform.rotation = Quaternion.identity;
            } else {
                attackCounter = attackTime;
                windupCounter = windupTime;
                canDamagePlayer = true;
                animator.SetTrigger("attack");
                transform.localScale = new Vector2(-1f, 1f);
                
            }
        }
    }
}
