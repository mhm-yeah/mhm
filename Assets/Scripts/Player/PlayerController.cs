using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    private GameManager gameManager;
    private GameObject hands;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        }
    }
}
