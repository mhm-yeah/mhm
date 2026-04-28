using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerDefense playerDefense;
    private GameManager gameManager;
    private float currentHealth;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerStats = GetComponent<PlayerStats>();
        currentHealth = playerStats.maxHealth;
        playerDefense = GetComponent<PlayerDefense>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (TryDodge())
        {
            Debug.Log("Attack dodged!");
            return;
        }

        if (playerDefense != null)
        {
            damage = playerDefense.ApplyDefense(damage);
        }
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    //This is for enemies that do not collide, thornmail ability:)
    public void TakeDamage(float damage)
    {
        TakeDamage(damage, null);
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, playerStats.maxHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private bool TryDodge()
    {
        return Random.value < playerStats.dodgeChance;
    }

    private void Die()
    {
        Debug.Log("Player died.");
        gameManager.GameOver();
        Destroy(gameObject);
    }
}