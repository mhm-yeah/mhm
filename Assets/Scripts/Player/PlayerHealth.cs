using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth;
    private PlayerDefense playerDefense;
    private PlayerStats playerStats;
    public GameManager gameManager;
    bool isDead;
    private void Awake()
    {
        playerDefense = GetComponent<PlayerDefense>();
        playerStats = GetComponent<PlayerStats>();
        currentHealth = playerStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (playerDefense != null)
        {
            damage = playerDefense.ApplyDefense(damage);
        }

        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health left: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            gameManager.GameOver();
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        Destroy(gameObject);
    }
}