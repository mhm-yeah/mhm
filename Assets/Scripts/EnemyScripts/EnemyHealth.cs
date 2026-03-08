using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private GameManager gameManager;

    public float maxHealth = 100f;
    private float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death animation or effects here
        // drop xp and loot here
        Destroy(gameObject);
        Debug.Log("Enemy died!");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            float damage = 25f; // to change later
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
    }
}
