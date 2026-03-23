using UnityEngine;

public class RangedEnemyShot : MonoBehaviour
{
    Transform player;
    private EnemyStats enemyStats;
    GameManager gameManager;
    public GameObject projectilePrefab;
    public float shootCooldown = 2f;
    float shootTimer = 0f;
    public float shootRange = 6f;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindWithTag("Player").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        float dx = player.position.x - transform.position.x;
        float dy = player.position.y - transform.position.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        shootTimer += Time.deltaTime;

        if (distance <= shootRange && shootTimer >= shootCooldown)
        {
            shootTimer = 0f;

            float dirX = dx / distance;
            float dirY = dy / distance;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();
            ep.damage = enemyStats.damage;
            ep.Launch(dirX, dirY, 8f);
        }
    }
}
