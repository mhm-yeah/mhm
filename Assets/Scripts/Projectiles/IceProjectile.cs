using UnityEngine;

public class IceProjectile : Projectile
{
    [HideInInspector] public float slowDuration = 2f;
    [HideInInspector] public float slowPercentage = 0.5f;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);

        IceStaff staff = weapon as IceStaff;
        slowDuration = staff.slowDuration;
        slowPercentage = staff.slowPercentage;
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
                enemyStats.Slow(slowDuration, slowPercentage);
            }

            Destroy(gameObject);
        }
    }
}
