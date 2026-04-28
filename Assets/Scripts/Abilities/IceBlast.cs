using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class IceBlast : Ability
{
    public GameObject ringPrefab;
    public GameObject iceBlastPrefab;
    private GameManager gameManager;

    public float stunLength = 3f;
    public float slowPercentage = 1f;
    public float blastRadius = 5f; // adjusted because player sprite has weird measurements

    [SerializeField] private GameObject synergyBlastParticles;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Awake()
    {
        unlocked = false;
        element = Utilities.Element.Ice;
    }
    public override void Activate()
    {
        base.Activate();
        Debug.Log("Ice Blast unlocked!");
    }


    protected override void Update()
    {
        if (!unlocked) return;
        base.Update();
        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.T) && !isOnCooldown)
        {
            Cast();
        }
    }
    
    public void Cast()
    {
        isOnCooldown = true;

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

        StartCooldown();
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
                    enemyHealth.TakeDamage(damage);
                    enemyStats.Slow(stunLength, slowPercentage);
                }
            }
        }
    }
}
