using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float lifetime;

    public virtual void Init(Weapon weapon)
    {
        damage = weapon.damage;
        speed = weapon.projectileSpeed;
        lifetime = weapon.projectileLifetime;
    }

    public virtual void Init(Ability ability)
    {
        damage = ability.currentDamage;
        speed = ability.abilitySpeed;
        lifetime = ability.lifeTime;
    }

    void Start()
    {
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    void DestroyProjectile()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
