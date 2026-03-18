using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float baseMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float baseMaxHealth = 100f;
    public float maxHealth = 100f;
    public float baseDamage = 25f;
    public float damage = 25f;
    public float fireRate = 3f;
    public float baseDefense = 0f;
    public float defense = 0f;
    public float dodgeChance = 0f;
    private bool canAttack = true;

    public bool getAttackActionStatus()
    {
        return canAttack;
    }

    public void applyAttackCooldown()
    {
        canAttack = false;
        Invoke(nameof(resetAttack), 1f / fireRate);
    }

    private void resetAttack()
    {
        canAttack = true;
    }
}
