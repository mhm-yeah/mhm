using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    private GameManager gameManager;
    private GameObject hands;

    // variables below will be removed later
    private ItemManager itemManager;
    private GameObject bulletsFolder;
    private GameObject bulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // remove later
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        bulletsFolder = GameObject.Find("Bullets");
        bulletPrefab = itemManager.bulletPrefabs[0];

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameIsOver)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDirection = mousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            hands.transform.rotation = Quaternion.Euler(0, 0, angle);

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    /// <summary>
    /// This function does not belong here, keeping it here for testing purposes.
    /// </summary>
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, hands.transform.position, hands.transform.rotation, bulletsFolder.transform);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(hands.transform.up * 10f, ForceMode2D.Impulse);
    }
}
