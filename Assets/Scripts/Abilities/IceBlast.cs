using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class IceBlast : MonoBehaviour
{
    public GameObject ringPrefab;
    public GameObject iceBlastPrefab;
    private GameManager gameManager;

    public float chargeTime = 2f;
    public float stunLength = 3f;
    public float damage = 10f;
    public float cooldownTime = 5f;
    public float blastRadius = 5f; // adjusted because player sprite has weird measurements
    private bool isOnCooldown = false;

    void Start()
    {
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

        Debug.Log("Ice Blast unlocked!");
    }


    void Update()
    {
        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.T) && !isOnCooldown)
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

        GameObject ring = Instantiate(ringPrefab, transform.position, transform.rotation, transform);
        GameObject iceBlast = Instantiate(iceBlastPrefab, transform.position, transform.rotation, ring.gameObject.transform);
        StartCoroutine(BlastCharge(ring, iceBlast));
    }

    IEnumerator BlastCharge(GameObject ring, GameObject iceBlast)
    {
        // charging animation
        for (float i = 0.01f; i <= chargeTime; i += Time.deltaTime)
        {
            iceBlast.transform.localScale = new Vector3(1f / (chargeTime / i), 1f / (chargeTime / i), 1f);
            yield return new WaitForEndOfFrame();
        }

        ActivateBlast(ring);

        Destroy(ring);
        Destroy(iceBlast);

        Invoke(nameof(DeactivateCooldown), cooldownTime);
    }

    private void ActivateBlast(GameObject ring)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ring.transform.position, blastRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    StartCoroutine(StunEnemy(enemy));
                }
            }
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
            enemySprite.color = Color.cyan;

            yield return new WaitForSeconds(stunLength);
            
            if (enemyStats != null)
            {
                enemyStats.isStunned = false;
                enemySprite.color = originalColor;

                Debug.Log("Enemy was stunned for " + (Time.time - now) + " seconds");
            }
        }
    }
}
