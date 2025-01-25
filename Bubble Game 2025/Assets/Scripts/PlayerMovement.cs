using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player

    [Header("Jump Settings")]
    public float jumpForce = 10f; // Force applied when jumping
    public float fallMultiplier = 0.5f; // Makes falling slower
    public float lowJumpMultiplier = 2f; // Makes low jumps smoother
    public bool allowMultipleJumps = true; // Enable or disable multiple jumps

    [Header("Descent Settings")]
    public float descentForce = 10f; // Force applied when descending

    [Header("Bounce Settings")]
    public float bounceForce = 20f;
    public float bounceRadius = 5f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 movement;
    private bool isGrounded = true; // Check if the player is on the ground

    private int lifePoints;
    private float originalMass;

    void Awake()
    {
        lifePoints = 3;
        rb = GetComponent<Rigidbody>();
        originalMass = rb.mass;
        playerInput = GetComponent<PlayerInput>();
    }

    void OnMove(InputValue value)
    {
        var vector = value.Get<Vector2>();
        movement = new Vector3(vector.x, 0, vector.y);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded || allowMultipleJumps)
            {
                isGrounded = false;
                Thrust(jumpForce);
            }
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed && !isGrounded)
        {
            Thrust(-descentForce);
        }
    }

    private void Thrust(float force)
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, force, rb.linearVelocity.z);
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

            // Adjust falling and jumping behavior
            if (rb.linearVelocity.y < 0) // Falling
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space)) // Low jump
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) // Ensure collision is with a surface beneath
        {
            isGrounded = true;
        }

        var touchedSeed = collision.gameObject.GetComponent<SeedBehaviour>();
        if (touchedSeed != null)
        {
            touchedSeed.gameObject.GetComponent<Rigidbody>()
                .AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
        }

        var touchedPlayer = collision.gameObject.GetComponent<PlayerMovement>();
        if (touchedPlayer != null)
        {
            touchedPlayer.gameObject.GetComponent<Rigidbody>()
                .AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void TakeDamage(int value)
    {
        Debug.Log($"Damage: {value}");
        lifePoints -= value;
        if (lifePoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void onDrestroy()
    {
        isGrounded = false;
        Debug.Log("Destroying");
        GameManager.OnPlayerDestroyed(this.gameObject);
    }

    // sound, low volume pop1

}
