using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
     using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class MegaGoblin : Enemy
    {
       public Animator animator;
        public int attack1damage;
        public int attack2damage;
        public int attack3damage;
        public BoxCollider2D slashBox;
         public BoxCollider2D slashBox2;

         public float attackCooldown;
        public float idleCounter;

        public Transform slashPosition;

        public GameObject phase2spikes;
        public GameObject platforms;

        public override void OnDeath() {
            platforms.SetActive(true);
            base.OnDeath();
        }

        public void Awake() {
            base.Awake();
            platforms.SetActive(true);
        }

        public override void OnUpdate()
        {
            platforms.SetActive(false);
            if(!detectedPlayer) {
                return;
            }

            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
            } else {
                int attack = (int)(Random.Range(1, 4));
                    
                    if(attack == 1) {
                        animator.SetTrigger("attack1");
                    }
                    if(attack == 2) {
                        animator.SetTrigger("attack2");
                    }
                    if(attack == 3) {
                        animator.SetTrigger("attack3");
                    }
                    


                idleCounter = attackCooldown;
            }

            if(getHealth() < 400) {
                phase2spikes.SetActive(true);
                phase2spikes.GetComponent<Animator>().SetTrigger("spike");
            }
        }

        public override Vector2 GetSlashEffectPosition() {
            return slashPosition.position;
        }

        public void bite1() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack1damage);
                }
            }
        }
        public void bite2() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack2damage);
                }
            }

            c = Physics2D.OverlapBox(slashBox2.offset * transform.localScale.x + (Vector2)(slashBox2.transform.position), slashBox2.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack2damage);
                }
            }
        }
        public void bite3() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack3damage);
                }
            }
        }
    }
}
