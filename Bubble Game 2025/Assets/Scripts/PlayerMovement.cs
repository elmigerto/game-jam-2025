using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player
    public float speedMultiplier = 1f; // Multiplier to adjust speed dynamically

    [Header("Jump Settings")]
    public float jumpForce = 50f; // Force applied when jumping
    
    [Header("Bounce Settings")]
    public float bounceForce = 20;

    public float bounceRadius = 20;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 movement;
    private bool isGrounded = true; // Check if the player is on the ground

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }


    void OnMove(InputValue value)
    {
        var vector = value.Get<Vector2>();
        movement = new Vector3(vector.x, 0, vector.y);
        // Debug.Log($"Move Input: {movement}");
    }

    public void OnJump(InputValue value)
    {
        Debug.Log("Jump");
        if (value.isPressed)
        {
            Debug.Log($"Player {playerInput.playerIndex} Jumped!");
            AdjustHeight(jumpForce);

        }
        else
        {
            Debug.Log(value.ToString());
        }
    }

        public void OnCrouch(InputValue value)
    {
        Debug.Log("Crouch");
        if (value.isPressed)
        {
            Debug.Log($"Player {playerInput.playerIndex} Crouched!");
            AdjustHeight(-jumpForce);
        }
        else
        {
            Debug.Log(value.ToString());
        }
    }

    private void AdjustHeight(float force)
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.down * force, ForceMode.VelocityChange);
        }
    }

    // void Jump()
    // {
    //     if (rb != null)
    //     {
    //         rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    //         isGrounded = false; // Set grounded to false when jumping
    //     }
    // }


    void FixedUpdate()
    {
        // Apply movement to the Rigidbody
        if (rb != null)
        {
            Vector3 velocity = movement * moveSpeed * speedMultiplier;
            velocity.y = rb.linearVelocity.y; // Maintain current vertical velocity
            rb.linearVelocity = velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded when colliding with a surface
        if (collision.contacts[0].normal.y > 0.5f) // Ensure collision is with a surface beneath
        {
            isGrounded = true;
        }

        // collision with a seed
        var touchedSeed = collision.gameObject.GetComponent<SeedBehaviour>();
        if (touchedSeed != null)
        {
            touchedSeed.gameObject.GetComponent<Rigidbody>().AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
            Debug.Log("You dead");

        }
    }
}
