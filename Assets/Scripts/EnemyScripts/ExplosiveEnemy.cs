using System.Collections;
using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    public float explosionDelay = 1f;
    public float explosionRadius = 2f;
    public float explosionDamage = 20f;

    private bool isTriggered = false;

    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void TriggerExplosion()
    {
        if (isTriggered) return;

        isTriggered = true;

        StartCoroutine(ExplosionCountdown());
    }

    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject obj = hits[i].gameObject;
            PlayerHealth playerHealth = obj.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage, gameObject);
            }
            EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
            if (enemyHealth != null && obj != gameObject)
            {
                enemyHealth.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }
}