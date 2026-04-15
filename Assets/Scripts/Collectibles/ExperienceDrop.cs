using UnityEngine;

public class ExperienceDrop : MonoBehaviour
{
    private LevelsManager levelsManager;
    public float expAmount = 20f;
    void Start()
    {
        levelsManager = GameObject.Find("GameManager").GetComponent<LevelsManager>();
    }

    void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 0, -50 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelsManager.IncreaseXP(expAmount);
            Destroy(gameObject);
        }
    }
}
