using UnityEngine;

public class BA_Animation : MonoBehaviour
{
    private BossEnemy bossEnemy;
    private float radius;

    public void Setup(BossEnemy boss, float blastRadius)
    {
        bossEnemy = boss;
        radius = blastRadius;
    }

    // called by animation event
    public void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                bossEnemy.GetPlayerHealth().TakeDamage(bossEnemy.GetDamage());
            }
        }
    }

    // called by animation event
    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}