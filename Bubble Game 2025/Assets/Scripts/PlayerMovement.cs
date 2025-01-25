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
    public float jumpForce = 50f; // Force applied when jumping

    [Header("Bounce Settings")]
    public float bounceForce = 20;

    public float bounceRadius = 5;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 movement;
    private bool isGrounded = true; // Check if the player is on the ground

    private int lifePoints;
    private float orginialMass;
    private float massMuliplier = 5;

    void Awake()
    {
        lifePoints = 3;
        rb = GetComponent<Rigidbody>();
        orginialMass = rb.mass;
        playerInput = GetComponent<PlayerInput>();
    }


    void OnMove(InputValue value)
    {
        var vector = value.Get<Vector2>();
        movement = new Vector3(vector.x, 0, vector.y);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            isGrounded = false;
            Thrust(jumpForce);
            Task.Run(() => {
                Debug.Log("task ran");
                isGrounded = true;
            });
        }
        else
        {
            Debug.Log(value.ToString());
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed && !isGrounded)
        {
            Thrust(-jumpForce);
        }
        else
        {
            Debug.Log(value.ToString());
        }
    }

    public void OnInteract(InputValue value)
    {
        Debug.Log("suicide");
        Destroy(this.gameObject);
    }

    private void Thrust(float force)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.up * force;
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
            rb.mass = !isGrounded ? orginialMass : orginialMass * massMuliplier;
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
            // var direction = collision.contacts[0].point + position
            touchedSeed.gameObject.GetComponent<Rigidbody>().AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
        }

        // collision with a other player
        var touchedplayer = collision.gameObject.GetComponent<PlayerMovement>();
        if (touchedplayer != null)
        {
            touchedplayer.gameObject.GetComponent<Rigidbody>().AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
            // Debug.Log("You touched player");

        }
    }

    void OnCollisionExit(Collision collision)
    {
        this.isGrounded = false;
        Debug.Log("Exit");
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
    }
}
