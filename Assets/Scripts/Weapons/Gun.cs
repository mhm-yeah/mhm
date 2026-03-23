using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerStats playerStats;
    private ItemManager itemManager;
    private GameObject hands;
    private GameObject bulletsFolder;
    private GameObject bulletPrefab;
    public InputActionReference fireAction;

    public float bulletSpeed = 10f;

    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        bulletsFolder = GameObject.Find("Bullets");
        bulletPrefab = itemManager.bulletPrefabs[0];

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

        GameObject bullet = Instantiate(bulletPrefab, hands.transform.position, hands.transform.rotation, bulletsFolder.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
