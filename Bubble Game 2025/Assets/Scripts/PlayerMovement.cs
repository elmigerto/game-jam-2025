using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player
    private Animator[] animators; // Reference to the Animators of the models

    [Header("Jump Settings")]
    public float jumpForce = 10f; // Force applied when jumping
    public float fallMultiplier = 2.5f; // Makes falling slower
    public float lowJumpMultiplier = 2f; // Makes low jumps smoother
    public bool allowMultipleJumps = false; // Enable or disable multiple jumps

    [Header("Descent Settings")]
    public float descentForce = 10f; // Force applied when descending

    [Header("Bounce Settings")]
    public float bounceForce = 20f;
    public float bounceRadius = 5f;
    public float damageForce = 40f;
    public int lifePoints;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 movement;
    private bool isGrounded = true; // Check if the player is on the ground
    private float timer;
    private bool isRunning;

    [Header("Sound Settings")]
    public int playerSoundNumber = 0;
    public int playerIndex = 0;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        StartCoroutine(RepeatAction(4, CheckIdle));
        StartCoroutine(RepeatAction(0.4f, PlayMovementSound)); // TODO: adjust  to actual moving sound



        // Get the Animator components from the models inside the player prefab
        animators = GetComponentsInChildren<Animator>();
        if (animators.Length == 0)
        {
            Debug.LogWarning("No animators found in child objects.");
        }
    }

    void OnMove(InputValue value)
    {
        var vector = value.Get<Vector2>();
        movement = new Vector3(vector.x, 0, vector.y);

        // Orient the player to face the movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        UpdateAnimator();
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
            SoundManager.PlayPlayerSound(SoundManager.Instance.playerJumpSounds);
        }
    }

    public void OnCrouch(InputValue value)
    {
        if (value.isPressed && !isGrounded)
        {
            Thrust(-descentForce);
        }
        else
        {
            Debug.Log(value.ToString());
        }
    }

    public void OnInteract(InputValue value)
    {
        Debug.Log("suicide");
        SoundManager.PlayPlayerSound(SoundManager.Instance.playerDeadVoice);
        Destroy(this.gameObject);
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

            isMoving = movement.magnitude > 0.1f;
            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // Increment timer by the time elapsed since the last frame
            timer += Time.deltaTime;
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        foreach (var animator in animators)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", math.max(movement.magnitude * moveSpeed, 1));
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

    public void TakeDamage(int damage)
    {
        lifePoints -= damage;
        GameManager.OnPlayerTakeDamage(this);
        if (lifePoints <= 0)
        {
            SoundManager.PlayPlayerSound(SoundManager.Instance.playerDeadVoice);
            Destroy(this.gameObject);
        }
        else
        {
            SoundManager.PlayPlayerSound(SoundManager.Instance.playerDamageVoice);
        }
    }

    // Take damage and also repell away
    public void TakeDamage(Collision collision)
    {
        // Debug.Log($"Damage: {value}");
        rb.linearVelocity = Vector3.zero;
        var direction = (collision.contacts[0].impulse + Vector3.up) * damageForce; // opposit direction
        rb.AddForce(direction, ForceMode.Impulse);
        TakeDamage(1);
    }


    IEnumerator RepeatAction(float interval, System.Action action)
    {
        while (true)
        {
            action?.Invoke(); // Call the method
            yield return new WaitForSeconds(interval); // Wait before next call
        }
    }

    void CheckIdle()
    {
        if (Random.Range(1, 10) > 2)
        {
            SoundManager.PlayPlayerSound(SoundManager.Instance.playerIdleVoice, playerSoundNumber);
        }
    }

    void PlayMovementSound()
    {
        if (isMoving)
        {

            SoundManager.PlaySound(SoundManager.Instance.playerMovementSound);
        }

    }
}
