using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    //public InputActionReference sprintAction;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // void OnEnable()
    // {
    //     sprintAction.action.Enable();
    //     sprintAction.action.performed += OnSprint;
    // }

    // void OnDisable()
    // {
    //     sprintAction.action.Disable();
    //     sprintAction.action.performed -= OnSprint;
    // }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // void OnSprint(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         Debug.Log("Sprint performed");
    //         currentSpeed = sprintSpeed;
    //     }
    //     else if (context.canceled)
    //     {
    //         Debug.Log("Sprint canceled");
    //         currentSpeed = moveSpeed;
    //     }
    // }
}