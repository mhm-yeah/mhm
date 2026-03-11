using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;

    private int currentHealth;
    private PlayerDefense playerDefense;
    public GameManager gameManager;
    bool isDead;
    private void Awake()
    {
        currentHealth = maxHealth;
        playerDefense = GetComponent<PlayerDefense>();
    }

    public void TakeDamage(int damage)
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