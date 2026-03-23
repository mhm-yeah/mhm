using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private float range = 50f;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        if (Vector2.Distance(transform.position, player.transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
