using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    public float healAmount = 20f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 0, -50 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
