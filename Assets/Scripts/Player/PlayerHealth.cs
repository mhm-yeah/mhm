using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    [Header("Dodge Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float dodgeChance = 0.2f;

    private PlayerDefense playerDefense;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerDefense = GetComponent<PlayerDefense>();
    }

    public void TakeDamage(float damage)
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
        Debug.Log("Player took " + damage + " damage. Health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private bool TryDodge()
    {
        return Random.value < dodgeChance;
    }

    private void Die()
    {
        Debug.Log("Player died.");
        Destroy(gameObject);
    }
}