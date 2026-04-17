using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base stats")]
    public float baseMoveSpeed = 2f;
    public float baseMaxHealth = 100f;
    public float baseDamage = 10f;
    public float baseDefense = 0f;

    [Header("Live stats")]
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float damage = 10f;
    public float defense = 0f;
    public float xpValue = 25f;

    [Header("Enemy status")]
    public bool isStunned = false;
    public bool isSlowed = false;
    public bool isInvulnerable = false;

    [Header("Drops table")]
    public float healthDropChance = 0.1f;
    public float experienceDropChance = 0.9f;

    private Color originalColor;
    private SpriteRenderer sprite;
    private EnemyHealth enemyHealth;

    private Coroutine slowCoroutine;
    private Coroutine damageOverTimeCoroutine;

    void Start()
    {
        originalColor = transform.GetComponent<SpriteRenderer>().color;
        sprite = transform.GetComponent<SpriteRenderer>();
        enemyHealth = transform.GetComponent<EnemyHealth>();
    }

    public Color GetOriginalColor()
    {
        return originalColor;
    }

    public void Stun(float duration)
    {
        if (isStunned == false)
        {
            isStunned = true;
            sprite.color = Color.softYellow;

            Invoke(nameof(Unstun), duration);
        }
    }

    private void Unstun()
    {
        if (transform != null) // it is possible for the enemy to die while stunned.
        {
            isStunned = false;
            sprite.color = originalColor;
        }
    }

    public void Slow(float duration, float slowPercentage)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine); // if the enemy is already slowed, reset the timer and slow percentage.
            // need to think of a proper solution
            moveSpeed = baseMoveSpeed; // moveSpeed could be affected by other slows with different percentages, so just reset
        }

        isSlowed = true;
        moveSpeed *= (1f - slowPercentage);
        sprite.color = Color.cyan;

        slowCoroutine = StartCoroutine(Unslow(duration, slowPercentage));
    }

    IEnumerator Unslow(float duration, float slowPercentage)
    {
        yield return new WaitForSeconds(duration);

        if (transform != null) // it is possible for the enemy to die while slowed.
        {
            isSlowed = false;
            moveSpeed /= (1f - slowPercentage);
            sprite.color = originalColor;
        }

        slowCoroutine = null;
    }

    public void DamageOverTime(float duration, float damagePerSecond)
    {
        if (damageOverTimeCoroutine != null)
        {
            StopCoroutine(damageOverTimeCoroutine); // reset effect if already burning.
        }
        damageOverTimeCoroutine = StartCoroutine(ApplyDamageOverTime(duration, damagePerSecond));
    }

    IEnumerator ApplyDamageOverTime(float duration, float damagePerSecond)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);

            if (enemyHealth != null) // it is possible for the enemy to die while burning.
            {
                enemyHealth.TakeDamage(damagePerSecond);
            }

            elapsed += 1f;
        }

        damageOverTimeCoroutine = null;
    }
}
