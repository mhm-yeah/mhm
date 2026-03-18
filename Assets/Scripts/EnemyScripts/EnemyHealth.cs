using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private GameManager gameManager;
    private LevelsManager levelsManager;
    private EnemyStats enemyStats;
    private PlayerStats playerStats;
    private float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelsManager = GameObject.Find("GameManager").GetComponent<LevelsManager>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
        currentHealth = enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death animation or effects here
        // drop xp and loot here

        levelsManager.IncreaseXP(10); // Example XP increase, adjust as needed
        Destroy(gameObject);
        Debug.Log("Enemy died!");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(playerStats.damage);
            Destroy(collision.gameObject);
        }
    }
}

