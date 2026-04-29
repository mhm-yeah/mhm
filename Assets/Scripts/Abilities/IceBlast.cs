using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class IceBlast : Ability
{
    public GameObject ringPrefab;
    public GameObject iceBlastPrefab;
    private GameManager gameManager;
   [SerializeField] private GameObject iceBlastObject;
    public float stunLength = 3f;
    public float slowPercentage = 1f;
    public float blastRadius = 5f; // adjusted because player sprite has weird measurements

    [SerializeField] private GameObject synergyBlastParticles;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected override void Awake()
    {
        base.Awake();
        unlocked = false;
        element = Utilities.Element.Ice;
    }
    
    public override void Activate()
    {
        iceBlastObject.SetActive(true);
        base.Activate();
    }


    protected override void Update()
    {
        base.Update();
        if (gameManager.isGameOver || isOnCooldown) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Cast();
        }
    }
    
    public void Cast()
    {
        StartCooldown();
        GameObject ring = Instantiate(ringPrefab, transform.position, transform.rotation, transform);
        GameObject iceBlast = Instantiate(iceBlastPrefab, transform.position, transform.rotation, ring.gameObject.transform);
        StartCoroutine(BlastCharge(ring, iceBlast));
    }

    IEnumerator BlastCharge(GameObject ring, GameObject iceBlast)
    {
        // charging animation
        for (float i = 0.01f; i <= castTime; i += Time.deltaTime)
        {
            iceBlast.transform.localScale = new Vector3(1f / (castTime / i), 1f / (castTime / i), 1f);
            yield return new WaitForEndOfFrame();
        }

        ActivateBlast(ring);

        Destroy(ring);
        Destroy(iceBlast);
    }

    private void ActivateBlast(GameObject ring)
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        bool hasSynergy = playerStats.HasElementalSynergy(element);
        //a bit redundant
        if (hasSynergy && synergyBlastParticles != null)
        {
            GameObject vfx = Instantiate(synergyBlastParticles, ring.transform.position, Quaternion.identity);
            Destroy(vfx, 3f); // match particle duration
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ring.transform.position, blastRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

                if (enemyHealth != null && enemyStats != null)
                {
                    enemyHealth.TakeDamage(currentDamage);
                    enemyStats.Slow(stunLength, slowPercentage);
                }
            }
        }
    }

    public override Dictionary<string, object> AbilityInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", 1 },
            { "Damage", currentDamage },
            { "Cooldown", currentCooldownTime },
            { "Stun duration", stunLength }
        };
    }

    public override Dictionary<string, object> LevelUpInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", $"{level} -> {level + 1}" },
            { "Damage", $"{currentDamage} -> {currentDamage + perLevelDamageIncrease}" },
            { "Cooldown", $"{currentCooldownTime} -> {currentCooldownTime - perLevelCooldownReduction}" },
            { "Stun duration", stunLength }
        };
    }
}
