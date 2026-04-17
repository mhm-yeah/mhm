using UnityEngine;

public class LightningProjectile : Projectile
{
    [HideInInspector] public float stunDuration = 0.5f;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);

        LightningStaff staff = weapon as LightningStaff;
        stunDuration = staff.stunDuration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            
            if (enemyHealth != null && enemyStats != null)
            {
                enemyHealth.TakeDamage(damage);
                enemyStats.Stun(stunDuration);
            }

            Destroy(gameObject);
        }
    }
    
}
