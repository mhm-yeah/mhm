using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private float sprintSpeed;
    private float currentSpeed;
    private PlayerStats playerStats;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        moveSpeed = playerStats.moveSpeed;
        sprintSpeed = playerStats.sprintSpeed;
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (gameManager.isGameOver || playerStats.GetStunStatus()) return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (gameManager.isGameOver) return;

        if (playerStats.GetStunStatus())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * currentSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}