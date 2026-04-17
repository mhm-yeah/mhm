using UnityEngine;

public class LightningProjectile : Projectile
{
    [HideInInspector] public float stunDuration = 0.5f;
    [HideInInspector] public int chainCount = 0;

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);

        LightningStaff staff = weapon as LightningStaff;
        stunDuration = staff.stunDuration;
    }

    public override void Init(Ability ability)
    {
        base.Init(ability);

        if (ability is LightningChain)
        {
            LightningChain chainAbility = ability as LightningChain;
            stunDuration = chainAbility.stunLength;
            chainCount = chainAbility.maxChainCount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            
            // weird solution... definitely a bad one.
            if (chainCount > 0)
            {
                LightningChain chainAbility = GameObject.Find("Player").GetComponent<LightningChain>();
                chainAbility.StartImpact(other);
            }

            if (enemyHealth != null && enemyStats != null)
            {
                enemyHealth.TakeDamage(damage);
                enemyStats.Stun(stunDuration);
            }

            Destroy(gameObject);
        }
    }
    
}
