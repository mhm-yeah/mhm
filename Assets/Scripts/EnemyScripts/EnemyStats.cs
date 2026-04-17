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

    void Start()
    {
        originalColor = transform.GetComponent<SpriteRenderer>().color;
        sprite = transform.GetComponent<SpriteRenderer>();
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
        if (isSlowed == false)
        {
            isSlowed = true;
            moveSpeed *= (1f - slowPercentage);
            sprite.color = Color.cyan;

            StartCoroutine(Unslow(duration, slowPercentage));
        }
    }

    // might need to change later
    IEnumerator Unslow(float duration, float slowPercentage)
    {
        yield return new WaitForSeconds(duration);

        if (transform != null) // it is possible for the enemy to die while slowed.
        {
            isSlowed = false;
            moveSpeed /= (1f - slowPercentage);
            sprite.color = originalColor;
        }
    }
}
