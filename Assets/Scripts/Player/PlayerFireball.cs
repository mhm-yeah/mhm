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

    // for the cards - will change later :<
    public bool unlocked = false;
    void Awake()
    {
        enabled = false;
    }
    public void Activate()
    {
        unlocked = true;
        enabled = true;

        Debug.Log("FireBall unlocked!");
    }


    public void OnFireball(InputValue value)
    {
        if (!enabled) return; // also for da cards

        if (!value.isPressed)
            return;

        //Debug.Log("FIREBALL");

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