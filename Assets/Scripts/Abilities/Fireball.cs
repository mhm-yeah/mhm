using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fireball : Ability
{
    private Transform hands;
    private GameObject projectilesFolder;
    private AudioManager audioManager;

    [Header("Fireball prefab")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject FireballObject;

    void Start()
    {
        projectilesFolder = GameObject.Find("Projectiles");
        hands = transform.Find("Hands");
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    protected override void Awake()
    {
        base.Awake();
        unlocked = false;
        element = Utilities.Element.Fire;
    }

    public override void Activate()
    {
        FireballObject.SetActive(true);
        base.Activate();
        Debug.Log("Fireball upgraded to level " + level);
    }

    public void OnFireball(InputValue value)
    {
        if (!enabled || isOnCooldown) return;

        if (!value.isPressed)
            return;

        GameObject fireball = Instantiate(
            fireballPrefab,
            hands.position,
            hands.rotation,
            projectilesFolder.transform
        );

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.fireballSound);
        }

        FireProjectile projectileScript = fireball.GetComponent<FireProjectile>();

        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
        bool hasSynergy = playerStats.HasElementalSynergy(element);
        Debug.Log($"[Fireball] Has synergy? {hasSynergy}");

        projectileScript.SetElementSynergy(hasSynergy);
        projectileScript.Init(this);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = hands.up * abilitySpeed;
        }

        StartCooldown();
    }

    public override Dictionary<string, object> AbilityInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", 1 },
            { "Damage", currentDamage },
            { "Cooldown", currentCooldownTime }
        };
    }

    public override Dictionary<string, object> LevelUpInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", $"{level} -> {level + 1}" },
            { "Damage", $"{currentDamage} -> {currentDamage + perLevelDamageIncrease}" },
            { "Cooldown", $"{currentCooldownTime} -> {currentCooldownTime - perLevelCooldownReduction}" }
        };
    }
}