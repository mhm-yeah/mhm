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
        unlocked = false;
        element = Utilities.Element.Fire;
        //enabled = true;
        //unlocked = true;
    }
    public override void Activate()
    {
        base.Activate();
        Debug.Log("Fireball upgraded to level " + level);
    }

    public void OnFireball(InputValue value)
    {
        if (!unlocked) return;

        if (!value.isPressed)
            return;

        GameObject fireball = Instantiate(
            fireballPrefab,
            hands.position,
            hands.rotation,
            projectilesFolder.transform
        );

        FireProjectile projectileScript = fireball.GetComponent<FireProjectile>();
        //for matching particles
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
        bool hasSynergy = playerStats.HasElementalSynergy(element);
        projectileScript.SetElementSynergy(hasSynergy);

        projectileScript.Init(this);

        

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = hands.up * abilitySpeed;
        }
    }
}