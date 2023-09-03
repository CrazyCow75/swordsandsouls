using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class MegaChomper : Enemy
    {
        public Animator animator;
        public int attack1damage;
        public int attack2damage;
        public int attack3damage;
        public BoxCollider2D slashBox;
         public BoxCollider2D slashBox2;

        bool trackingPlayer;
        public float slashDistance;

        private Vector3 originalPosition;
        public AnimationCurve bounceCurve;

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

                float distance = Vector2.Distance (this.transform.position, pos);

                if(distance > slashDistance) { // track player
                    _velocity = new Vector2((pos.x > transform.position.x ? 1f : -1f) * moveSpeed * Time.deltaTime * 20f, _velocity.y);
                }
            }

            if(idleCounter > 0f) {
                idleCounter -= Time.deltaTime;
            } else {
                if(getHealth() < 250f) {
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
                } else {
                    int attack = (int)(Random.Range(1, 3));
                     Debug.Log(attack);
                    if(attack == 1) {
                        animator.SetTrigger("attack1");
                    }
                    if(attack == 2) {
                        animator.SetTrigger("attack2");
                    }
                }

                idleCounter = attackCooldown;
            }
        }

        public override Vector2 GetSlashEffectPosition() {
            return slashPosition.position;
        }

        public void checkFlip() {
            if(Player.Instance.transform.position.x > originalPosition.x) {
                transform.localScale = new Vector3(1f, 1f, 0f);
            } else {
                transform.localScale = new Vector3(-1f, 1f, 0f);
            }
        }

        public void resetFlip() {
            transform.localScale = new Vector3(1f, 1f, 0f);
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

        public void bite1() {
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
        public void bite2() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

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

            c = Physics2D.OverlapBox(slashBox2.offset * transform.localScale.x + (Vector2)(slashBox2.transform.position), slashBox2.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(attack3damage);
                }
            }
        }
    }
}
