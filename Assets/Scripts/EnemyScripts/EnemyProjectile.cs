using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;
    public float lifetime = 4f;
    float lifetimer = 0f;

    public void Launch(float dirX, float dirY, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(dirX, dirY) * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        lifetimer += Time.deltaTime;
        if (lifetimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}