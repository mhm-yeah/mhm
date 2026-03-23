using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    private EnemyStats enemyStats;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyStats = GetComponent<EnemyStats>();
        GameObject playerObject;

        playerObject = GameObject.FindWithTag("Player");

        player = playerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver) return;
        
        Vector3 direction;
        direction.x = player.position.x - transform.position.x;
        direction.y = player.position.y - transform.position.y;
        direction.z = 0;

        float distance;
        distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);

        direction.x = direction.x / distance;
        direction.y = direction.y / distance;

        transform.position = transform.position + direction * enemyStats.moveSpeed * Time.deltaTime;
    }

    // perhaps change later?
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerHealth>().TakeDamage(enemyStats.damage);
        }
    }
}
