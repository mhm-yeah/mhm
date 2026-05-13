using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : Ability
{
    private Transform hands;
    private GameObject projectilesFolder;
    private GameManager gameManager;
    private AudioManager audioManager;

    public GameObject lightningChainPrefab;
    [SerializeField] private GameObject lightningChainObject;

    public int maxChainCount = 3;
    public float chainRange = 2f;
    public float stunLength = 1f;
    public float delayBetweenChains = 0.1f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        hands = transform.Find("Hands");
        projectilesFolder = GameObject.Find("Projectiles");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public override void Activate()
    {
        lightningChainObject.SetActive(true);
        base.Activate();
        Debug.Log("Lightning Chain upgraded to level " + level);
    }

    protected override void Update()
    {
        base.Update();

        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Q) && !isOnCooldown)
        {
            Cast();
        }
    }

    public void Cast()
    {
        StartCooldown();

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.lightningSound);
        }

        GameObject lightningChain = Instantiate(
            lightningChainPrefab,
            hands.position,
            hands.rotation,
            projectilesFolder.transform
        );

        LightningProjectile projectileScript = lightningChain.GetComponent<LightningProjectile>();
        projectileScript.Init(this);

        PlayerStats playerStats = GetComponent<PlayerStats>();
        bool hasElementalSynergy = playerStats.HasElementalSynergy(element);
        projectileScript.SetElementSynergy(hasElementalSynergy);

        Rigidbody2D rb = lightningChain.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * abilitySpeed, ForceMode2D.Impulse);
    }

    public void StartImpact(Collider2D enemy)
    {
        GameObject[] markedEnemies = new GameObject[maxChainCount + 1];
        markedEnemies[0] = enemy.gameObject;

        ImpactEnemy(enemy);

        StartCoroutine(StartChain(maxChainCount, enemy.transform.position, markedEnemies));
    }

    private IEnumerator StartChain(int chainCount, Vector3 currentTarget, GameObject[] markedEnemies)
    {
        if (chainCount <= 0) yield break;

        yield return new WaitForSeconds(delayBetweenChains);

        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(currentTarget, chainRange);
        GameObject nextTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in potentialTargets)
        {
            if (collider.gameObject.CompareTag("Enemy") &&
                !Array.Exists(markedEnemies, enemy => enemy == collider.gameObject))
            {
                float distance = Vector2.Distance(currentTarget, collider.transform.position);

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

            ImpactEnemy(nextTarget.GetComponent<Collider2D>());

            yield return StartCoroutine(StartChain(chainCount - 1, nextTarget.transform.position, markedEnemies));
        }
    }

    private void ImpactEnemy(Collider2D enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        if (enemyHealth != null && enemyStats != null)
        {
            enemyHealth.TakeDamage(currentDamage);
            enemyStats.Stun(stunLength);
        }
    }

    public override Dictionary<string, object> AbilityInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", 1 },
            { "Damage", currentDamage },
            { "Cooldown", currentCooldownTime },
            { "Stun duration", stunLength },
            { "Chain count", maxChainCount }
        };
    }

    public override Dictionary<string, object> LevelUpInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", $"{level} -> {level + 1}" },
            { "Damage", $"{currentDamage} -> {currentDamage + perLevelDamageIncrease}" },
            { "Cooldown", $"{currentCooldownTime} -> {currentCooldownTime - perLevelCooldownReduction}" },
            { "Stun duration", stunLength },
            { "Chain count", maxChainCount }
        };
    }
}