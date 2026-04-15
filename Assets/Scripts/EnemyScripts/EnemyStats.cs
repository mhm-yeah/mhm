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
    public bool isInvulnerable = false;

    [Header("Drops table")]
    public float healthDropChance = 0.1f;
    public float experienceDropChance = 0.9f;

    private Color originalColor;

    void Start()
    {
        originalColor = transform.GetComponent<SpriteRenderer>().color;
    }

    public Color GetOriginalColor()
    {
        return originalColor;
    }
}
