using UnityEngine;

public class Gun : MonoBehaviour
{
    private GameManager gameManager;
    private ItemManager itemManager;
    private GameObject hands;
    private GameObject bulletsFolder;
    private GameObject bulletPrefab;

    public float bulletSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        bulletsFolder = GameObject.Find("Bullets");
        bulletPrefab = itemManager.bulletPrefabs[0];

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, hands.transform.position, hands.transform.rotation, bulletsFolder.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
