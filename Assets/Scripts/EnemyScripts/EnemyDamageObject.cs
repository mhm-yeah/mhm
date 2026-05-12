using System.Collections;
using UnityEngine;

public class EnemyDamageObject : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    public float damage;
    public float lifetime;

    private float damageInterval = 0.5f;
    private bool isPlayerInCollider = false;

    private Coroutine damageInsideCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        Destroy(gameObject, lifetime);
    }

    IEnumerator DamageInside()
    {
        while (isPlayerInCollider)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = true;

            if (damageInsideCoroutine != null)
            {
                StopCoroutine(damageInsideCoroutine);
            }
            damageInsideCoroutine = StartCoroutine(DamageInside());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInCollider = false;
        }
    }
}
