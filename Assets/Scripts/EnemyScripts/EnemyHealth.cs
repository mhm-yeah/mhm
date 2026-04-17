using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats enemyStats;
    private ItemManager itemManager;
    private GameObject collectiblesFolder;
    private float currentHealth;
    public GameObject damageNumberPrefab;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        collectiblesFolder = GameObject.Find("Collectibles");
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
        ExplosiveEnemy explosive = GetComponent<ExplosiveEnemy>();
        if (explosive != null && !explosive.IsTriggered())
        {
            explosive.TriggerExplosion();
            Debug.Log("explodes");
            return;
        }
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
            GameObject expDrop = itemManager.experienceDrop;
            ExperienceDrop expScript = expDrop.GetComponent<ExperienceDrop>();
            expScript.expAmount = enemyStats.xpValue;
            Instantiate(expDrop, transform.position, transform.rotation, collectiblesFolder.transform);
        }

        Destroy(gameObject);
    }
}

