using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private float sprintSpeed;
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        moveSpeed = playerStats.moveSpeed;
        sprintSpeed = playerStats.sprintSpeed;
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * currentSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}