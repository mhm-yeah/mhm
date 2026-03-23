using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float baseMoveSpeed = 2f;
    public float baseMaxHealth = 100f;
    public float maxHealth = 100f;
    public float baseDamage = 10f;
    public float damage = 10f;
    public float baseDefense = 0f;
    public float defense = 0f;
    public float xpValue = 25f;
    public float healthDropChance = 0.1f;
}
