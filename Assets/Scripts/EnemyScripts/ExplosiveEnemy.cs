using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    public float explosionRadius = 2f;
    public float explosionDamage = 20f;
    public Animator animator;

    private bool isTriggered = false;

    private Collider2D enemyCollider;

    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
    }

    public void TriggerExplosion()
    {
        if (isTriggered) return;

        isTriggered = true;

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        transform.localScale = Vector3.one * 20f;
        animator.SetTrigger("isExplode");
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }

    public void Explode()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            GameObject obj = hit.gameObject;

            PlayerHealth playerHealth =
                obj.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage, gameObject);
            }

            EnemyHealth enemyHealth =
                obj.GetComponent<EnemyHealth>();

            if (enemyHealth != null && obj != gameObject)
            {
                enemyHealth.TakeDamage(explosionDamage);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}