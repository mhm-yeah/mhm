using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats enemyStats;
    private ItemManager itemManager;
    private GameObject collectiblesFolder;
    private BossEnemy bossEnemyScript;
    private float currentHealth;
    public GameObject damageNumberPrefab;
    public HealthBarBehaviour healthBar;
    AudioManager audioManager;

    private GameManager gameManager;
    private bool isBoss = false;
    private bool isDead = false;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        collectiblesFolder = GameObject.Find("Collectibles");
        enemyStats = GetComponent<EnemyStats>();
        currentHealth = enemyStats.maxHealth;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (GetComponent<BossEnemy>() != null)
        {
            healthBar.SetHealth(currentHealth, enemyStats.maxHealth);
            bossEnemyScript = GetComponent<BossEnemy>();
            isBoss = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (enemyStats.isInvulnerable || isDead)
        {
            return;
        }
        
        currentHealth -= damage;

        if (healthBar != null)
        {
            currentHealth = Mathf.Max(0f, currentHealth);
            healthBar.SetHealth(currentHealth, enemyStats.maxHealth);
        }

        audioManager.PlaySFX(audioManager.enemyDamaged);
        if (currentHealth <= enemyStats.maxHealth / 2)
        {
            enemyStats.isBelowHalfHealth = true;
        }

        GameObject dmgNum = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity);
        dmgNum.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(Mathf.RoundToInt(damage).ToString());

        if (currentHealth <= 0)
        {
            isDead = true;
            if (isBoss == true)
            {
                Debug.Log("boss died");
                bossEnemyScript.ObjectCleanUp();
            }

            Die();
        }
    }

    void Die()
    {
        // Add death animation or effects here
        // drop xp and loot here

        // make a function to check all the possible drops and their chances.
        int healthDropRoll = Random.Range(0, 100);
        if (healthDropRoll < enemyStats.healthDropChance * 100)
        {
            GameObject healthDrop = itemManager.healthDrop;
            Instantiate(healthDrop, transform.position, transform.rotation, collectiblesFolder.transform);
        }
        else
        {
            float expAmount = enemyStats.xpValue;
            GameObject expDrop = itemManager.smallExperienceDrop;
            if (expAmount >= itemManager.bigExperienceDrop.GetComponent<ExperienceDrop>().expAmount)
            {
                expDrop = itemManager.bigExperienceDrop;
            }
            else if (expAmount >= itemManager.mediumExperienceDrop.GetComponent<ExperienceDrop>().expAmount)
            {
                expDrop = itemManager.mediumExperienceDrop;
            }

            ExperienceDrop expScript = expDrop.GetComponent<ExperienceDrop>();
            expScript.expAmount = enemyStats.xpValue;
            Instantiate(expDrop, transform.position, transform.rotation, collectiblesFolder.transform);
        }

        ExplosiveEnemy explosive = GetComponent<ExplosiveEnemy>();
        if (explosive != null && !explosive.IsTriggered())
        {
            explosive.TriggerExplosion();
            return;
        }

        if (isBoss == true)
        {
            Debug.Log("boss died");
            gameManager.VictoryDelay();
        }
        Destroy(gameObject);
    }
}

