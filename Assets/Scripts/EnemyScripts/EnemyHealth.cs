using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private GameManager gameManager;
    private LevelsManager levelsManager;
    private EnemyStats enemyStats;
    private PlayerStats playerStats;
    private ItemManager itemManager;
    private GameObject collectiblesFolder;
    private LightningChain lightningChainAttack; // to change later
    private float currentHealth;
    public GameObject damageNumberPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        levelsManager = GameObject.Find("GameManager").GetComponent<LevelsManager>();
        collectiblesFolder = GameObject.Find("Collectibles");
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        lightningChainAttack = GameObject.Find("Player").GetComponent<LightningChain>(); // to change later
        enemyStats = GetComponent<EnemyStats>();
        currentHealth = enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        GameObject dmgNum = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity);
        dmgNum.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(Mathf.RoundToInt(damage).ToString());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add death animation or effects here
        // drop xp and loot here

        levelsManager.IncreaseXP(enemyStats.xpValue); // Example XP increase, adjust as needed

        // make a function to check all the possible drops and their chances.
        int healthDropRoll = Random.Range(0, 100);
        if (healthDropRoll < enemyStats.healthDropChance * 100)
        {
            GameObject healthDrop = itemManager.healthDrop;
            Instantiate(healthDrop, transform.position, transform.rotation, collectiblesFolder.transform);
        }

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
        else if (collision.CompareTag("LightningChain")) // to change later
        {
            Destroy(collision.gameObject);
            lightningChainAttack.StartImpact(transform.GetComponent<Collider2D>()); // to change later
        }
    }
}

