using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player

    [Header("Input Settings")]
    public string horizontalInput = "Horizontal"; // Input axis for horizontal movement
    public string verticalInput = "Vertical";     // Input axis for vertical movement

    private Rigidbody rb;
    private Vector3 movement;

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
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
    }
}
