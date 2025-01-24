using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player

    [Header("Input Settings")]
    public string horizontalInput = "Horizontal"; // Input axis for horizontal movement
    public string verticalInput = "Vertical";     // Input axis for vertical movement
    public KeyCode jumpKey = KeyCode.Space;        // Key for jumping

    [Header("Jump Settings")]
    public float jumpForce = 5f; // Force applied when jumping

    private Rigidbody rb;
    private Vector3 movement;
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
        // Get input from the keyboard
        float moveX = Input.GetAxis(horizontalInput); // Customizable horizontal input
        float moveZ = Input.GetAxis(verticalInput);   // Customizable vertical input

        // Store movement vector
        movement = new Vector3(moveX, 0f, moveZ).normalized; // Movement on XZ plane

        // Jump input
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
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

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded when colliding with a surface
        if (collision.contacts[0].normal.y > 0.5f) // Ensure collision is with a surface beneath
        {
            isGrounded = true;
        }
    }
}
