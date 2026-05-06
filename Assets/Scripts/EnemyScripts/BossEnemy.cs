using System;
using System.Collections;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private GameManager gameManager;
    private EnemyStats enemyStats;
    private GameObject player;
    private PlayerStats playerStats;
    private PlayerHealth playerHealth;
    private GameObject projectilesFolder;

    [Header("Ability references")]
    public GameObject basicAttackRingPrefab;
    public GameObject basicAttackCirclePrefab;
    public GameObject groundSlamPrefab;

    [Header("Boss settings")]
    public float basicAttackChargeTime = 1f;
    public float basicCooldownDuration = 0.5f;
    public float abilityCooldownDuration = 5f;
    public float groundSlamDamage = 20f;
    public float groundSlamChargeTime = 3f;
    public float groundSlamStunDuration = 2f;
    public float secondPhaseHasteModifier = 2f;
    public int attacksBeforeAbility = 100;
    private int attackCounter = 0;
    private float modifier = 1f;

    [Header("Boss status")]
    public bool isOnCooldown = false;
    public bool isOnSecondPhase = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerHealth = player.GetComponent<PlayerHealth>();
        projectilesFolder = GameObject.Find("Projectiles");
    }

    void Update()
    {
        if (gameManager.isGameOver || isOnCooldown || enemyStats.isStunned)
        {
            return;
        }

        if (enemyStats.isBelowHalfHealth && !isOnSecondPhase)
        {
            isOnSecondPhase = true;
            modifier = secondPhaseHasteModifier;
            Debug.Log("second phase");
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
            // int abilityIndex = Random.Range(0, 3);
            // switch (abilityIndex)
            // {
            //     case 0:
            //         GroundSlam();
            //         break;
            //     case 1:
            //         BulletRain();
            //         break;
            //     case 2:
            //         SpinningSun();
            //         break;
            // }

            StartCoroutine(GroundSlam());

            //Invoke(nameof(RemoveCooldown), abilityCooldownDuration / modifier);
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
        float chargeTime = basicAttackChargeTime / modifier;
        for (float i = 0.01f; i <= chargeTime; i += Time.deltaTime)
        {
            blast.transform.localScale = new Vector3(i / chargeTime, i / chargeTime, 1f);
            yield return new WaitForEndOfFrame();
        }

        ActivateBlast(ring);

        Destroy(ring);
        Destroy(blast);

        Invoke(nameof(RemoveCooldown), basicCooldownDuration / modifier);
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

    private GameObject ResizeSlam(GameObject slam)
    {
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        slam.transform.localScale = new Vector3(scaleX * scaleX, scaleY * scaleY, 1f);
        return slam;
    }

    private GameObject ResizeSlam(GameObject currentSlam, Vector3 previousSlamScale, Vector3 previousInnerRingScale)
    {
        currentSlam.transform.localScale = new Vector3(previousSlamScale.x / previousInnerRingScale.x,
            previousSlamScale.y / previousInnerRingScale.y, 1f);
        return currentSlam;
    }

    IEnumerator GroundSlam()
    {
        Debug.Log("ground slam");

        GameObject slam = Instantiate(groundSlamPrefab, transform.position, transform.rotation, projectilesFolder.transform);
        GameObject innerRing = slam.transform.Find("InnerRing").gameObject;
        
        slam = ResizeSlam(slam);

        Vector3 previousSlamScale = slam.transform.localScale;
        Vector3 previousInnerRingScale = innerRing.transform.localScale;

        StartCoroutine(GroundSlamCharge(slam, innerRing));

        yield return new WaitForSeconds(groundSlamChargeTime / modifier);

        for (int i = 0; i < 2; i++)
        {
            GameObject newSlam = Instantiate(groundSlamPrefab, transform.position, transform.rotation, projectilesFolder.transform);
            GameObject newInnerRing = newSlam.transform.Find("InnerRing").gameObject;

            newSlam = ResizeSlam(newSlam, previousSlamScale, previousInnerRingScale);

            previousSlamScale = newSlam.transform.localScale;
            previousInnerRingScale = newInnerRing.transform.localScale;

            StartCoroutine(GroundSlamCharge(newSlam, newInnerRing));

            yield return new WaitForSeconds(groundSlamChargeTime / modifier);
        }

        RemoveCooldown();
    }

    IEnumerator GroundSlamCharge(GameObject slam, GameObject innerRing)
    {
        GameObject filling = slam.transform.Find("Filling").gameObject;
        float initialScale = filling.transform.localScale.x;

        float chargeTime = groundSlamChargeTime / modifier;

        for (float i = 0; i <= chargeTime; i += Time.deltaTime)
        {
            float newScale = initialScale + ((1 - initialScale) * (i / chargeTime));
            filling.transform.localScale = new Vector3(newScale, newScale, 1f);
            yield return new WaitForEndOfFrame();
        }

        ActivateGroundSlam(slam, innerRing);

        Destroy(slam);
    }

    private void ActivateGroundSlam(GameObject slam, GameObject innerRing)
    {
        float outerRadius = slam.transform.localScale.x / 2f;
        float innerRadius = outerRadius * innerRing.transform.localScale.x;

        Collider2D[] hitColliders1 = Physics2D.OverlapCircleAll(slam.transform.position, outerRadius);
        Collider2D[] hitColliders2 = Physics2D.OverlapCircleAll(innerRing.transform.position, innerRadius);
        
        bool hitPlayer = false;
        bool inCollider = false;

        foreach (Collider2D collider in hitColliders1)
        {
            if (collider.CompareTag("Player"))
            {
                hitPlayer = true;
                inCollider = true;
                Debug.Log("Slam radius: " + outerRadius + ", InnerRing radius: " + innerRadius);
                
                if (hitColliders2.Contains(collider))
                {
                    inCollider = false;
                    Debug.Log("In the inner collider, no damage");
                }
                else
                {
                    Debug.Log("In the outer collider, deal damage");
                }
            }
        }

        if (hitPlayer && inCollider)
        {
            playerHealth.TakeDamage(groundSlamDamage);
            playerStats.Stun(groundSlamStunDuration);
        }
    }

    private void BulletRain()
    {
        
    }

    private void SpinningSun()
    {
        
    }
}
