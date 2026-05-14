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
    //to animate
    public GameObject basicAttackBlastPrefab;

    public GameObject groundSlamPrefab;
    public GameObject spinningSunPrefab;
    public GameObject bossBulletPrefab;
    //to animate
    public GameObject spinningSunRayPrefab;


    [Header("Boss settings")]
    public float secondPhaseHasteModifier = 2f;
    public int attacksBeforeAbility = 100;
    private int attackCounter = 0;
    private float modifier = 1f;
    private Color abilityColor;
    private string specialTag = "EnemyProjectile";

    [Header("Basic attack settings")]
    public float basicAttackChargeTime = 1f;
    public float basicCooldownDuration = 0.5f;
    public float abilityCooldownDuration = 2f;

    [Header("Ground slam settings")]
    public float groundSlamDamage = 20f;
    public float groundSlamChargeTime = 3f;
    public float groundSlamStunDuration = 2f;

    [Header("Spinning sun settings")]
    public int spinningSunRays = 4;
    public float spinningSunRayChargeTime = 2f;
    public float spinningSunRayMovingDuration = 10f;
    public float spinningSunRayMoveDegrees = 360f;

    [Header("Bullet circle settings")]
    public int bulletCircleCount = 4;
    public int bulletCount = 8;
    public float bulletDamage = 10f;
    public float bulletSpeed = 5f;
    public float bulletLifetime = 10f;
    public float bulletWaitTime = 1f;

    [Header("Boss status")]
    public bool isOnCooldown = false;
    public bool isOnSecondPhase = false;
    private bool cannotGetSpinningSun = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerHealth = player.GetComponent<PlayerHealth>();
        projectilesFolder = GameObject.Find("Projectiles");

        abilityColor = groundSlamPrefab.GetComponent<SpriteRenderer>().color;
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

            //choose random ability
            int abilityIndex = GetRandomIndex();

            while (cannotGetSpinningSun && abilityIndex == 1)
            {
                abilityIndex = GetRandomIndex();
            }

            switch (abilityIndex)
            {
                case 0:
                    Debug.Log("Ground slam chosen");
                    StartCoroutine(GroundSlam());
                    break;
                case 1:
                    Debug.Log("Spinning sun chosen");
                    SpinningSun();
                    break;
                case 2:
                    Debug.Log("Bullet circle chosen");
                    StartCoroutine(BulletCircle());
                    break;
            }
        }
    }

    public void ObjectCleanUp()
    {
        Debug.Log("|Should be deleted");
        foreach (Transform child in projectilesFolder.transform)
        {
            if (child.CompareTag(specialTag))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private int GetRandomIndex()
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        return randomIndex;
    }

    private void RemoveCooldown()
    {
        isOnCooldown = false;
    }

    private void BasicAttack()
    {
        GameObject ring = Instantiate(basicAttackRingPrefab, player.transform.position, player.transform.rotation, projectilesFolder.transform);
        GameObject blast = Instantiate(basicAttackCirclePrefab, player.transform.position, player.transform.rotation, ring.transform);
        
        ring.tag = specialTag;
        blast.tag = specialTag;

        SpriteRenderer ringRenderer = ring.GetComponent<SpriteRenderer>();
        SpriteRenderer blastRenderer = blast.GetComponent<SpriteRenderer>();

        ringRenderer.color = abilityColor;
        blastRenderer.color = abilityColor;

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
        GameObject visualBlast = Instantiate(basicAttackBlastPrefab, ring.transform.position, Quaternion.identity, projectilesFolder.transform);
        visualBlast.tag = specialTag;
        BA_Animation blastScript = visualBlast.GetComponent<BA_Animation>();
        blastScript.Setup(this, ring.transform.localScale.x / 2f);
    }
    public PlayerHealth GetPlayerHealth()
    {
        return playerHealth;
    }

    public float GetDamage()
    {
        return enemyStats.damage;
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
        GameObject slam = Instantiate(groundSlamPrefab, transform.position, transform.rotation, projectilesFolder.transform);
        slam.tag = specialTag;

        GameObject innerRing = slam.transform.Find("InnerRing").gameObject;

        slam = ResizeSlam(slam);

        Vector3 previousSlamScale = slam.transform.localScale;
        Vector3 previousInnerRingScale = innerRing.transform.localScale;

        StartCoroutine(GroundSlamCharge(slam, innerRing));

        yield return new WaitForSeconds(groundSlamChargeTime / modifier);

        for (int i = 0; i < 2; i++)
        {
            GameObject newSlam = Instantiate(groundSlamPrefab, transform.position, transform.rotation, projectilesFolder.transform);
            newSlam.tag = specialTag;
            GameObject newInnerRing = newSlam.transform.Find("InnerRing").gameObject;

            newSlam = ResizeSlam(newSlam, previousSlamScale, previousInnerRingScale);
            newSlam.tag = specialTag;

            previousSlamScale = newSlam.transform.localScale;
            previousInnerRingScale = newInnerRing.transform.localScale;

            StartCoroutine(GroundSlamCharge(newSlam, newInnerRing));

            yield return new WaitForSeconds(groundSlamChargeTime / modifier);
        }

        Invoke(nameof(RemoveCooldown), abilityCooldownDuration / modifier);
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
                //Debug.Log("Slam radius: " + outerRadius + ", InnerRing radius: " + innerRadius);

                if (hitColliders2.Contains(collider))
                {
                    inCollider = false;
                    //Debug.Log("In the inner collider, no damage");
                }
                else
                {
                    //Debug.Log("In the outer collider, deal damage");
                }
            }
        }

        if (hitPlayer && inCollider)
        {
            playerHealth.TakeDamage(groundSlamDamage);
            playerStats.Stun(groundSlamStunDuration);
        }
    }

    private void SpinningSun()
    {
        cannotGetSpinningSun = true;

        int numberOfRays = (int)(spinningSunRays * 1f * modifier);

        GameObject[] movingRays = new GameObject[numberOfRays];
        GameObject[] rayAnimations = new GameObject[numberOfRays];

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * (180f / numberOfRays);
            GameObject ray = Instantiate(spinningSunPrefab, transform.position, transform.rotation, projectilesFolder.transform);
            ray.tag = specialTag;
            GameObject rayFilling = ray.transform.Find("Fill").gameObject;
            SpriteRenderer rayRenderer = ray.GetComponent<SpriteRenderer>();
            SpriteRenderer rayFillingRenderer = rayFilling.GetComponent<SpriteRenderer>();

            rayRenderer.color = abilityColor;
            rayFillingRenderer.color = abilityColor;
            ray.transform.Rotate(0f, 0f, angle);

            //vfx
            GameObject rayAnim = Instantiate(spinningSunRayPrefab, ray.transform.position, ray.transform.rotation, projectilesFolder.transform);
            rayAnim.tag = specialTag;
            SpriteRenderer fillRenderer = rayFilling.GetComponent<SpriteRenderer>();
            SpriteRenderer animRenderer = rayAnim.GetComponent<SpriteRenderer>();

            if (animRenderer != null)
            {
                //animRenderer.color = abilityColor;
                animRenderer.transform.localScale = new Vector3(4f, 4f, 1f);
            }
            rayAnim.SetActive(false);
            movingRays[i] = ray;
            rayAnimations[i] = rayAnim;
        }
        StartCoroutine(SpinningSunCharge(movingRays, rayAnimations));
    }

    IEnumerator SpinningSunCharge(GameObject[] rays, GameObject[] rayAnimations)
    {
        GameObject[] rayFillings = new GameObject[rays.Length];

        for (int i = 0; i < rays.Length; i++)
        {
            rayFillings[i] = rays[i].transform.Find("Fill").gameObject;
        }

        float chargeTime = spinningSunRayChargeTime / modifier;

        for (float i = 0; i <= chargeTime; i += Time.deltaTime)
        {
            float newScale = i / chargeTime;
            foreach (GameObject ray in rayFillings)
            {
                ray.transform.localScale = new Vector3(1f, newScale, 1f);
            }
            yield return new WaitForEndOfFrame();
        }

        // activate damage + animations
        for (int i = 0; i < rays.Length; i++)
        {
            EnemyDamageObject script = rayFillings[i].GetComponent<EnemyDamageObject>();
            script.enabled = true;
            //rayFillings[i].SetActive(false); ///?
            rayFillings[i].GetComponent<SpriteRenderer>().enabled = false;
            rayAnimations[i].SetActive(true);
            Animator animator = rayAnimations[i].GetComponent<Animator>();
            animator.speed = modifier;
        }
        StartCoroutine(SpinningSunMove(rays, rayAnimations));
        // start casting other abilities after spinning rays start moving
        RemoveCooldown();
    }

    IEnumerator SpinningSunMove(GameObject[] rays, GameObject[] rayAnimations)
    {
        float elapsedTime = 0f;
        float moveDuration = spinningSunRayMovingDuration / modifier;
        float moveDegreesPerSecond = spinningSunRayMoveDegrees / moveDuration;

        while (elapsedTime < moveDuration)
        {
            float degressToRotate = moveDegreesPerSecond * Time.deltaTime;
            //Debug.Log("Rotating rays by " + degressToRotate + " degrees");

            for (int i = 0; i < rays.Length; i++)
            {
                rays[i].transform.Rotate(0f, 0f, degressToRotate);
                rayAnimations[i].transform.RotateAround(transform.position, Vector3.forward, degressToRotate);
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < rays.Length; i++)
        {
            Destroy(rays[i]);
            Destroy(rayAnimations[i]);
        }
        cannotGetSpinningSun = false;
    }

    IEnumerator BulletCircle()
    {
        int numberOfBullets = (int)(bulletCount * modifier);
        int numberOfCircles = (int)(bulletCircleCount * modifier);

        float customAngle = 360f / numberOfBullets / 2f;

        for (int i = 0; i < numberOfCircles; i++)
        {
            GameObject[] bullets = new GameObject[numberOfBullets];

            for (int j = 0; j < numberOfBullets; j++)
            {
                float angle = j * (360f / numberOfBullets) + customAngle * i;
                GameObject bullet = Instantiate(bossBulletPrefab, transform.position, transform.rotation, projectilesFolder.transform);
                bullet.tag = specialTag;

                bullet.transform.Rotate(0f, 0f, angle);

                EnemyProjectile bulletScript = bullet.GetComponent<EnemyProjectile>();
                bulletScript.Init(bulletDamage, bulletLifetime);

                bullets[j] = bullet;
            }

            ActivateBulletCircle(bullets);

            yield return new WaitForSeconds(bulletWaitTime / modifier);
        }

        Invoke(nameof(RemoveCooldown), abilityCooldownDuration / modifier);
    }

    private void ActivateBulletCircle(GameObject[] bullets)
    {
        foreach (GameObject bullet in bullets)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed * modifier, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerHealth.TakeDamage(enemyStats.damage);
        }
    }
}
