using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerDefense playerDefense;
    private GameManager gameManager;
    private float currentHealth;
    AudioManager audioManager;

    public Image healthBarBackground;
    private Image healthBar;
    private TextMeshProUGUI healthText;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerStats = GetComponent<PlayerStats>();
        currentHealth = playerStats.maxHealth;
        playerDefense = GetComponent<PlayerDefense>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        healthBar = healthBarBackground.transform.Find("Health bar").GetComponent<Image>();
        healthText = healthBarBackground.transform.Find("Healthtxt").GetComponent<TextMeshProUGUI>();

        UpdateHp();
    }

    private void UpdateHp()
    {
        healthBar.fillAmount = currentHealth / playerStats.maxHealth;
        healthText.text = Mathf.FloorToInt(currentHealth) + "/" + Mathf.FloorToInt(playerStats.maxHealth);
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
        audioManager.PlaySFX(audioManager.playerDamaged);

        UpdateHp();

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
        UpdateHp();
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