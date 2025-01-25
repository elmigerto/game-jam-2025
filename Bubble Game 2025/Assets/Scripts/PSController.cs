using UnityEngine;
using UnityEngine.InputSystem;

public class PS5ControllerTest : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player
    public float rotationalSpeed = 5f; // Speed of the player

    [Header("Input Settings")]
    public string horizontalInput = "Horizontal"; // Input axis for horizontal movement
    public string verticalInput = "Vertical";     // Input axis for vertical movement
    public KeyCode jumpKey = KeyCode.Space;        // Key for jumping

    [Header("Jump Settings")]
    public float jumpForce = 5f; // Force applied when jumping

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 rotation;
    private bool isGrounded = true; // Check if the player is on the ground

    void Start()
    {
        // Get the Rigidbody component attached to the sphere
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the player! Make sure to add one.");
        }
    }

    void Update()
    {
        if (Gamepad.current != null)
        {
            float moveX = Gamepad.current.leftStick.x.ReadValue();
            float moveY = Gamepad.current.leftStick.y.ReadValue();
            float rotateZ = Gamepad.current.rightStick.x.ReadValue() ;
            bool xButton = Gamepad.current.buttonSouth.isPressed;

            Debug.Log($"right Stick: ({rotateZ})");

            // Store movement vector
            movement = new Vector3(moveX, 0f, moveY).normalized; // Movement on XZ plane
            rotation = new Vector3(0f, rotateZ, 0).normalized * rotationalSpeed;
            // Jump input
            if (xButton)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody
        if (rb != null)
        {
            Vector3 velocity = movement * moveSpeed;
            velocity.y = rb.linearVelocity.y; // Maintain current vertical velocity
            rb.linearVelocity = velocity;
            rb.AddTorque(rotation);
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Set grounded to false when jumping
        }
    }
}
