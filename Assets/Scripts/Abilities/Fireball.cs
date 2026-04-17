using UnityEngine;
using UnityEngine.InputSystem;

public class Fireball : Ability
{
    private Transform hands;
    private GameObject projectilesFolder;

    [Header("Fireball prefab")]
    [SerializeField] private GameObject fireballPrefab;

    void Start()
    {
        projectilesFolder = GameObject.Find("Projectiles");
        hands = transform.Find("Hands");
    }

    void Awake()
    {
        //enabled = false;
        enabled = true;
        unlocked = true;
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

        FireProjectile projectileScript = fireball.GetComponent<FireProjectile>();
        projectileScript.Init(this);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = hands.up * abilitySpeed;
        }
    }
}