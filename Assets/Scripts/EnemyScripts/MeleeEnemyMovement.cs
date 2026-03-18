using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    private EnemyStats enemyStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        GameObject playerObject;

        playerObject = GameObject.FindWithTag("Player");

        player = playerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyStats.damage);
        }
    }
}
