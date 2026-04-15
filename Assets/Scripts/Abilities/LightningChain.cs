using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class LightningChain : MonoBehaviour
{
    private Transform hands;
    private GameObject projectilesFolder;
    private GameManager gameManager;
    public GameObject lightningChainPrefab;
    
    public float lightningChainSpeed = 10f;
    public float damage = 20f;
    public int maxChainCount = 3;
    public float chainRange = 2f;
    public float stunLength = 1f;
    public float cooldownTime = 5f;
    private bool isOnCooldown = false;

    void Start()
    {
        hands = transform.Find("Hands"); // Making the ability for the player for now.
        projectilesFolder = GameObject.Find("Projectiles");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    // for the cards - will change later :<
    public bool unlocked = false;
    void Awake()
    {
        enabled = false;
    }
    public void Activate()
    {
        unlocked = true;
        enabled = true;

        Debug.Log("Lightning Chain unlocked!");
    }


    void Update()
    {
        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Q) && !isOnCooldown)
        {
            Cast();
        }
    }

    private void DeactivateCooldown()
    {
        isOnCooldown = false;
    }

    public void Cast()
    {
        isOnCooldown = true;

        GameObject lightningChain = Instantiate(lightningChainPrefab, hands.position, hands.rotation, projectilesFolder.transform);
        Rigidbody2D rb = lightningChain.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * lightningChainSpeed, ForceMode2D.Impulse);

        Invoke(nameof(DeactivateCooldown), cooldownTime);
    }

    public void StartImpact(Collider2D enemy)
    {
        GameObject[] markedEnemies = new GameObject[maxChainCount + 1];
        markedEnemies[0] = enemy.gameObject;

        ImpactEnemy(enemy);

        StartChain(maxChainCount, enemy.gameObject, markedEnemies);
    }

    private void StartChain(int chainCount, GameObject currentTarget, GameObject[] markedEnemies)
    {
        if (chainCount <= 0) return;

        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(currentTarget.transform.position, chainRange);
        GameObject nextTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in potentialTargets)
        {
            if (collider.gameObject.CompareTag("Enemy") && !Array.Exists(markedEnemies, enemy => enemy == collider.gameObject))
            {
                float distance = Vector2.Distance(currentTarget.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nextTarget = collider.gameObject;
                }
            }
        }

        if (nextTarget != null)
        {
            markedEnemies[maxChainCount - chainCount + 1] = nextTarget;
            // Add the logic to visually connect the currentTarget and nextTarget with a lightning effect.
            
            ImpactEnemy(nextTarget.GetComponent<Collider2D>());

            StartChain(chainCount - 1, nextTarget, markedEnemies);
        }
    }

    private void ImpactEnemy(Collider2D enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            StartCoroutine(StunEnemy(enemy));
        }
    }

    IEnumerator StunEnemy(Collider2D enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>(); 
        if (enemyStats != null)
        {
            float now = Time.time;
            enemyStats.isStunned = true;
            Color originalColor = enemySprite.color;
            enemySprite.color = Color.softYellow;

            yield return new WaitForSeconds(stunLength);
            
            if (enemyStats != null) // it is possible for the enemy to die while stunned.
            {
                enemyStats.isStunned = false;
                enemySprite.color = originalColor;

                Debug.Log("Enemy was stunned for " + (Time.time - now) + " seconds");
            }
        }
    }
}
