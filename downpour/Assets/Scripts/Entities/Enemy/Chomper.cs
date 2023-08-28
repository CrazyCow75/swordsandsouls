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

        private void Start() {
            originalPosition = transform.position;
        }

        public override void OnUpdate()
        {
            if(!detectedPlayer) {
                return;
            } else {
                Collider2D c = Physics2D.OverlapCircle(transform.position, detectRad, Layers.PlayerLayer);
                if(c == null) {
                    detectedPlayer = false;
                    animator.SetBool("attacking", false);

                    transform.position = originalPosition;
                }
            }

            if(detectedPlayer) {
                animator.SetBool("attacking", true);
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
