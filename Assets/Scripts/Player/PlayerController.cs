using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public InputActionReference mousePositionAction;
    private GameManager gameManager;
    private GameObject hands;
    private Vector2 mousePosition;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
    }

    void Update()
    {
        if (!gameManager.gameIsOver)
        {
            mousePosition = mousePositionAction.action.ReadValue<Vector2>();
            Vector2 mousePos = cam.ScreenToWorldPoint(mousePosition);
            Vector2 lookDirection = mousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            hands.transform.rotation = Quaternion.Euler(0, 0, angle);       
        }
    }
}
