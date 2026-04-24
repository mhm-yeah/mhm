using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    private EnemyStats enemyStats;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer; // ?? pridëta

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyStats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ?? pridëta

        GameObject playerObject;
        playerObject = GameObject.FindWithTag("Player");

        player = playerObject.transform;
    }

    void Update()
    {
        if (gameManager.isGameOver || enemyStats.isStunned) return;

        Vector3 direction;
        direction.x = player.position.x - transform.position.x;
        direction.y = player.position.y - transform.position.y;
        direction.z = 0;

        float distance;
        distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        if (distance < 0.01f)
        {
            return;
        }
        direction.x = direction.x / distance;
        direction.y = direction.y / distance;

        transform.position = transform.position + direction * enemyStats.moveSpeed * Time.deltaTime;

        spriteRenderer.flipX = direction.x < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ExplosiveEnemy explosive = GetComponent<ExplosiveEnemy>();

            if (explosive != null && !explosive.IsTriggered())
            {
                explosive.TriggerExplosion();
                Debug.Log("explodes");
                return;
            }
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerHealth>().TakeDamage(enemyStats.damage, gameObject);
        }
    }
}