using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class Spike : MonoBehaviour
    {
        public BoxCollider2D slashBox;
        public int damage;
        
        private void Update() {
            Collider2D c = Physics2D.OverlapBox(slashBox.offset * transform.localScale.x + (Vector2)(slashBox.transform.position), slashBox.size, 0f, Layers.PlayerLayer);

            if(c!=null) {
                if(c.transform.TryGetComponent(out Player player)) {
                    player.PlayerStatsController.TakeDamage(damage);
                }
            }
        }
    }
}
