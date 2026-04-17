using UnityEngine;

public class FireProjectile : Projectile
{
    [HideInInspector] public float burnDamagePerSecond = 5f;
    [HideInInspector] public float burnDuration = 3f;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);

        FireStaff staff = weapon as FireStaff;
        burnDamagePerSecond = staff.burnDamagePerSecond;
        burnDuration = staff.burnDuration;
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
                enemyStats.DamageOverTime(burnDuration, burnDamagePerSecond);
            }

            Destroy(gameObject);
        }
    }
}
