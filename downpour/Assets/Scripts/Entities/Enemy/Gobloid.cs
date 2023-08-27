using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class Gobloid : Enemy
    {
        public float slashTime;
        private float slashCounter;
        [SerializeField]private bool isSlashing;

        public float slashDistance;
        public float moveSpeed;

        public Animator animator;
        public int damage;
        public BoxCollider2D slashBox;
        public override void OnUpdate() {
            Vector2 pos = Player.Instance.transform.position;
            
            if(!isSlashing)
                transform.localScale = new Vector2((pos.x > transform.position.x ? -1f : 1f), 1f);

            if(isSlashing) {
                if(slashCounter > 0) {
                    slashCounter -= Time.deltaTime;
                } else {
                    isSlashing = false;
                }
                return;
            }

            float distance = Vector2.Distance (this.transform.position, pos);

            if(distance > slashDistance) {
               _velocity = new Vector2((pos.x > transform.position.x ? 1f : -1f) * moveSpeed * Time.deltaTime * 20f, _velocity.y);
            } else {
                isSlashing = true;
                slashCounter = slashTime;
                animator.SetTrigger("slash");
            }

        }

        public void slash() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset + (Vector2)(this.transform.position), slashBox.size, 0f, Layers.PlayerLayer);
            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(damage);
                }
            }
        }
    }
}
