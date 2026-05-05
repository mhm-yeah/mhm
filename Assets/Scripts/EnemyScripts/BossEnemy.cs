using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private GameManager gameManager;
    private EnemyStats enemyStats;
    private GameObject player;
    private PlayerHealth playerHealth;
    private GameObject projectilesFolder;

    [Header("Ability references")]
    public GameObject basicAttackRingPrefab;
    public GameObject basicAttackCirclePrefab;

    [Header("Boss settings")]
    public float basicAttackChargeTime = 1f;
    public float basicCooldownDuration = 0.5f;
    public float abilityCooldownDuration = 5f;
    public float secondPhaseHasteMultiplier = 2f;
    public int attacksBeforeAbility = 100;
    private int attackCounter = 0;

    [Header("Boss status")]
    public bool isOnCooldown = false;
    public bool isOnSecondPhase = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        projectilesFolder = GameObject.Find("Projectiles");
    }

    void Update()
    {
        if (gameManager.isGameOver || isOnCooldown || enemyStats.isStunned)
        {
            return;
        }

        isOnCooldown = true;

        if (attackCounter < attacksBeforeAbility)
        {
            BasicAttack();
            attackCounter++;
        }
        else
        {
            attackCounter = 0;

            // choose random ability
            int abilityIndex = Random.Range(0, 3);
            switch (abilityIndex)
            {
                case 0:
                    GroundSlam();
                    break;
                case 1:
                    BulletRain();
                    break;
                case 2:
                    SpinningSun();
                    break;
            }

            Invoke(nameof(RemoveCooldown), abilityCooldownDuration);
        }
    }

    private void RemoveCooldown()
    {
        isOnCooldown = false;
    }

    private void BasicAttack()
    {
        GameObject ring = Instantiate(basicAttackRingPrefab, player.transform.position, player.transform.rotation, projectilesFolder.transform);
        GameObject blast = Instantiate(basicAttackCirclePrefab, player.transform.position, player.transform.rotation, ring.transform);

        StartCoroutine(BasicAttackCharge(ring, blast));
    }

    IEnumerator BasicAttackCharge(GameObject ring, GameObject blast)
    {
        // charging animation
        for (float i = 0.01f; i <= basicAttackChargeTime; i += Time.deltaTime)
        {
            blast.transform.localScale = new Vector3(i / basicAttackChargeTime, i / basicAttackChargeTime, 1f);
            yield return new WaitForEndOfFrame();
        }

        ActivateBlast(ring);

        Destroy(ring);
        Destroy(blast);

        Invoke(nameof(RemoveCooldown), basicCooldownDuration);
    }

    private void ActivateBlast(GameObject ring)
    {
        // damage player if in radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(ring.transform.position, ring.transform.localScale.x / 2f);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerHealth.TakeDamage(enemyStats.damage);
            }
        }
    }

    private void GroundSlam()
    {
        
    }

    private void BulletRain()
    {
        
    }

    private void SpinningSun()
    {
        
    }
}
