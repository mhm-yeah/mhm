using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireball : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private Transform hands;

    void Start()
    {
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
            hands.rotation
        );

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = hands.up * 10f;
        }
    }
}