using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public InputActionReference fireAction;
    [HideInInspector] public GameObject projectilePrefab;

    private GameManager gameManager;
    private GameObject player;
    private PlayerStats playerStats;
    private GameObject hands;
    private GameObject projectilesFolder;
    
    public Utilities.Element element = Utilities.Element.Default;
    public int id = 0;
    public int damage = 0;
    public float attackSpeed = 0f;
    public float projectileSpeed = 0f;
    public float projectileLifetime = 0f;
    [HideInInspector] public float projectileXRotation = 0f;

    private bool isFiring = false;

    public void Init()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        projectilesFolder = GameObject.Find("Projectiles").gameObject;
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        hands = player.transform.Find("Hands").gameObject;
    }

    void Update()
    {
        if (gameManager.isGameOver == true || playerStats.GetWeaponActionStatus() == false)
        {
            return;
        }

        if (isFiring)
        {
            Fire();
        }
    }

    void OnEnable()
    {
        //fireAction.action.Enable();
        fireAction.action.started += OnFireStarted;
        fireAction.action.canceled += OnFireEnded;
    }

    void OnDisable()
    {
        //fireAction.action.Disable();
        fireAction.action.started -= OnFireStarted;
        fireAction.action.canceled -= OnFireEnded;
    }

    void OnFireStarted(InputAction.CallbackContext context)
    {
        isFiring = true;
    }

    void OnFireEnded(InputAction.CallbackContext context)
    {
        isFiring = false;
    }

    void Fire()
    {
        playerStats.ApplyWeaponCooldown();

        Weapon currentWeapon = playerStats.GetCurrentWeapon();

        GameObject bullet = Instantiate(projectilePrefab, hands.transform.position, hands.transform.rotation, projectilesFolder.transform);
        bullet.transform.Rotate(projectileXRotation, 0f, 0f);

        Projectile projectileScript = bullet.GetComponent<Projectile>();
        // currently not every projectile has this script (testing)
        if (projectileScript != null)
        {
            projectileScript.Init(currentWeapon);

            bool hasElementalSynergy = playerStats.HasElementalSynergy(currentWeapon.element);
            projectileScript.SetElementSynergy(hasElementalSynergy);
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * projectileSpeed, ForceMode2D.Impulse);
    }

    public abstract Dictionary<string, object> WeaponInfo();
}
