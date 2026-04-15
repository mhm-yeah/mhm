using UnityEngine;
using UnityEngine.InputSystem;

public class FireStaff : Weapon
{
    private GameManager gameManager;
    private PlayerStats playerStats;
    private ItemManager itemManager;
    private GameObject hands;
    private GameObject projectilesFolder;
    private GameObject bulletPrefab;
    public InputActionReference fireAction;

    public float bulletSpeed = 10f;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        projectilesFolder = GameObject.Find("Projectiles");
        bulletPrefab = itemManager.bulletPrefabs[1]; // currently the prefab is the same as fireball (ability)

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
        playerStats = GetComponent<PlayerStats>();

    }

    void OnEnable()
    {
        //fireAction.action.Enable();
        fireAction.action.started += Fire;
    }

    void OnDisable()
    {
        //fireAction.action.Disable();
        fireAction.action.started -= Fire;
    }

    void Fire(InputAction.CallbackContext context)
    {
        if (gameManager.isGameOver == true || playerStats.getAttackActionStatus() == false)
        {
            return;
        }
        
        playerStats.applyAttackCooldown();
        GameObject bullet = Instantiate(bulletPrefab, hands.transform.position, hands.transform.rotation, projectilesFolder.transform);
        bullet.transform.Rotate(new Vector3(180, 0, 0));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
