using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Combat;
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class bomb : MonoBehaviour
    {
    Rigidbody2D rb;
 
    public float speed = 2f;

    public float explosionRad = 2f;
 
 
    public int damage;
   

 
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Invoke("Explode", 0.5f);
            //Destroy(gameObject, 5f);

        }

 
        // Update is called once per frame
        void Update()
        {
            
 
            rb.velocity = transform.right * speed;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 0.3f, Layers.HittableLayer);
            if(enemies.Length != 0) {
                Explode();
            }
        }

        void Explode() {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRad, Layers.HittableLayer);
                    if(hits.Length != 0) {
                            foreach(Collider2D hit in hits) {
                                if(hit) {
                                    if(hit.transform.TryGetComponent(out IHittable hittable)) {

                                        hittable.OnHit(Player.Instance, damage, this.transform.position.x > hit.transform.position.x ? 1 : -1);

                                         Player.Instance.PlayerCombatController._emitSlashParticle(hittable.GetSlashEffectPosition());

                                        Player.Instance.PlayerStatsController.heal(Player.Instance.PlayerStatsController.CurrentPlayerStats.regen);

                                        

                                        
                                    }
                                }
                            }

                            
                        }
                        Player.Instance.PlayerCombatController.emitExplosionParticle(transform.position);
                        CameraManager.Instance.CameraShaker.Shake(0.2f, 2f);

                        Destroy(gameObject);
        }
    }
}
