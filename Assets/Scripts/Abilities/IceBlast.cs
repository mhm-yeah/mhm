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

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected override void Awake()
    {
        cooldownTime += castTime;
        base.Awake();
        //enabled = true;
        //unlocked = true;
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
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, object> LevelUpInfo()
    {
        throw new System.NotImplementedException();
    }
}
