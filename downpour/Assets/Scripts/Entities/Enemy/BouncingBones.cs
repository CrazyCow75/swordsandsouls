using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class BouncingBones : Enemy
    {
        public Animator animator;
        public int attack1damage;
        public int attack2damage;

        public BoxCollider2D slashBox;
        bool trackingPlayer;
        private Vector3 originalPosition;

        public float attackCooldown;
        public float idleCounter;

        public float moveSpeed;
        public Transform slashPosition;

        private void Start() {
            originalPosition = transform.position;
        }

        public override void OnUpdate()
        {
            if(!detectedPlayer) {
                return;
            }

            if(trackingPlayer) {
                Vector2 pos = Player.Instance.transform.position;

                transform.position = new Vector2(pos.x, transform.position.y);
            }

            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
            } else {
                int attack = (int)(Random.Range(1, 4));
                Debug.Log(attack);
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
        }

        public override Vector2 GetSlashEffectPosition() {
            return slashPosition.position;
        }

        public void resetPos() {
            transform.position = originalPosition;
        }

        public void startTrack() {
            trackingPlayer = true;
        }

        public void stopTrack() {
            trackingPlayer = false;
        }

        public void attack1() {
            Collider2D c;
            if(transform.localScale.x < 0f) {
                c = Physics2D.OverlapBox(slashBox.offset + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);
            } else {
                c = Physics2D.OverlapBox(new Vector2(slashBox.offset.x * -1f, slashBox.offset.y) + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);
            }
            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack1damage);
                }
            }
        }

        public void attack2() {
            Collider2D c;
            if(transform.localScale.x < 0f) {
                c = Physics2D.OverlapBox(slashBox.offset + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);
            } else {
                c = Physics2D.OverlapBox(new Vector2(slashBox.offset.x * -1f, slashBox.offset.y) + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);
            }
            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack2damage);
                }
            }
        }
    }
}
