using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    private GameObject player;
    private GameManager gameManager;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        transform.position = player.transform.position + offset;
    }
}
