using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class Chomper : Enemy
    {
        public Animator animator;
        public int damage;
        public BoxCollider2D slashBox;

        private Vector3 originalPosition;
        public AnimationCurve bounceCurve;

        [SerializeField] private float attackCounter;
        [SerializeField] private float idleCounter;

        public bool toxic = false;

        public float idleTime;
        public float attackTime;
        public bool canDamagePlayer;

        private void Start() {
            originalPosition = transform.position;
        }

        public override void OnUpdate()
        {
            if(!detectedPlayer) {
                return;
            }

            // if(detectedPlayer) {
            //     animator.SetBool("attacking", true);
            // }

            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
                transform.position = originalPosition;
                animator.SetBool("attacking", false);
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
                canDamagePlayer = true;
                
                

                if(toxic) {
                    if(Random.Range(0, 2) == 1) {
                        animator.SetBool("attacking", false);
                        animator.SetTrigger("blast");
                    } else {
                        animator.SetBool("attacking", true);
                    }
                } else {
                    animator.SetBool("attacking", true);
                }
            }
        }

        public void bite() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(damage);
                }
            }
        }
    }
}
