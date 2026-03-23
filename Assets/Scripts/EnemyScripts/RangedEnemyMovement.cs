using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform player;
    private EnemyStats enemyStats;
    private GameManager gameManager;

    public float preferredDistance = 5f;
    public float retreatDistance = 3f;
    public float retreatDelay = 0.3f;

    float retreatTimer = 0f;
    float strafeDirection;
    float strafeTimer = 0f;
    public float strafeChangeTime = 2f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindWithTag("Player").transform;
        strafeDirection = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        float dx = player.position.x - transform.position.x;
        float dy = player.position.y - transform.position.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        float dirX = dx / distance;
        float dirY = dy / distance;

        
        float perpX = -dirY * strafeDirection;
        float perpY = dirX * strafeDirection;

        strafeTimer += Time.deltaTime;
        if (strafeTimer >= strafeChangeTime)
        {
            strafeDirection *= -1f;
            strafeTimer = 0f;
        }

        if (distance > preferredDistance)
        {
            retreatTimer = 0f;
            float moveX = (dirX + perpX) * enemyStats.moveSpeed * Time.deltaTime;
            float moveY = (dirY + perpY) * enemyStats.moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, moveY, 0);
        }
        else if (distance < retreatDistance)
        {
            retreatTimer += Time.deltaTime;
            if (retreatTimer >= retreatDelay)
            {
                float moveX = (-dirX + perpX * 0.3f) * enemyStats.moveSpeed * Time.deltaTime;
                float moveY = (-dirY + perpY * 0.3f) * enemyStats.moveSpeed * Time.deltaTime;
                transform.position = transform.position + new Vector3(moveX, moveY, 0);
            }
        }
        else
        {
            retreatTimer = 0f;
            float moveX = perpX * enemyStats.moveSpeed * Time.deltaTime;
            float moveY = perpY * enemyStats.moveSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, moveY, 0);
        }
    }
}
