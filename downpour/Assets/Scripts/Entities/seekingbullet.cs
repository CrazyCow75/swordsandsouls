using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Combat;
    using Downpour.Entity.Player;
    using Downpour.Entity.Enemy;
    public class seekingbullet : MonoBehaviour
    {
    Rigidbody2D rb;
 
    public float speed = 0.02f;
 
    public Vector2 target;
 
    private float moveSpeedFollow = 1f;
    public float rotateSpeed = 200f;
 
    public int damage;
 
    public float MoveSpeed5 = 1f;
 
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            
            Destroy(gameObject, 4f);

        }

 
        // Update is called once per frame
        void Update()
        {
            
            
 
            rb.velocity = transform.right * speed;

            target = GetClosestEnemy();

            if(!(target.x == 0 && target.y == 0)) {
                transform.LookAt(target);
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                //rb.velocity = transform.up * speed;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.3f, Layers.HittableLayer);
            
            if(hits.Length != 0) {
                    foreach(Collider2D hit in hits) {
                        if(hit) {
                            if(hit.transform.TryGetComponent(out IHittable hittable)) {

                                hittable.OnHit(Player.Instance, damage, this.transform.position.x > hit.transform.position.x ? 1 : -1);

                                Player.Instance.PlayerCombatController._emitSlashParticle(hittable.GetSlashEffectPosition());

                                Player.Instance.PlayerStatsController.heal(Player.Instance.PlayerStatsController.CurrentPlayerStats.regen);

                                CameraManager.Instance.CameraShaker.Shake(0.1f, 1f);

                                Destroy(gameObject);
                            }
                        }
                    }

                    
                }
        }
     
        Vector2 GetClosestEnemy()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 10f, Layers.HittableLayer);
            //Debug.Log(enemies + " " + enemies.Length);
            Vector2 tMin = new Vector2(0f, 0f);
            float minDist = Mathf.Infinity;
            Vector2 currentPos = transform.position;

            if(enemies.Length == 0) {
                return new Vector2(0f, 0f);
            }

            foreach (Collider2D t in enemies)
            {
                if(t.gameObject.GetComponent<IHittable>() != null) {
                    float dist = Vector2.Distance(t.gameObject.GetComponent<IHittable>().GetSlashEffectPosition(), currentPos);
                    if (dist < minDist)
                    {
                        tMin = t.gameObject.GetComponent<IHittable>().GetSlashEffectPosition();
                        //Debug.Log(t.gameObject.GetComponent<IHittable>().GetSlashEffectPosition() + "hitpos");
                        minDist = dist;
                    }
                }
            }
            
            return tMin;
        }
    
    }
}
