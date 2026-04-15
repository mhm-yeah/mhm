using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : Weapon
{
    private GameManager gameManager;
    private PlayerStats playerStats;
    private ItemManager itemManager;
    private GameObject hands;
    private GameObject projectilesFolder;
    private GameObject bulletPrefab;
    public InputActionReference fireAction;

    public float bulletSpeed = 10f;
    private bool isFiring = false;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        projectilesFolder = GameObject.Find("Projectiles");
        bulletPrefab = itemManager.bulletPrefabs[0];

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (gameManager.isGameOver == true || playerStats.getAttackActionStatus() == false)
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
        playerStats.applyAttackCooldown();

        GameObject bullet = Instantiate(bulletPrefab, hands.transform.position, hands.transform.rotation, projectilesFolder.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
