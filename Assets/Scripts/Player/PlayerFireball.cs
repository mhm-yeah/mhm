using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireball : MonoBehaviour
{
    private Transform hands;
    private GameObject projectilesFolder;
    [SerializeField] private GameObject fireballPrefab;

    void Start()
    {
        projectilesFolder = GameObject.Find("Projectiles");
        hands = transform.Find("Hands");
    }

    public void OnFireball(InputValue value)
    {
        if (!value.isPressed)
            return;

        Debug.Log("FIREBALL");

        GameObject fireball = Instantiate(
            fireballPrefab,
            hands.position,
            hands.rotation,
            projectilesFolder.transform
        );

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = hands.up * 10f;
        }
    }
}