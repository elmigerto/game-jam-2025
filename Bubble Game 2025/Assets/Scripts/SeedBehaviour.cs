using System;
using UnityEngine;

public class SeedBehaviour : MonoBehaviour
{
    public int despawnCount = 3;
    public GameObject arena;
    public GameObject plantBody;
    public float bounceForce = 30;

    public string targetTag = "SphereWorld"; // Tag of the objects that will trigger score increment
    public int scoreValue = 10; // Points to add when collision happens

    public GameObject BallPrefab;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == arena.name)
        {
            // Debug.LogWarning("here");
            despawnCount -= 1;
            if (despawnCount == 0)
            {
                BecomePlant();
            }
            else
            {
                BounceFromGround();
            }
        }
        else if (collision.gameObject.CompareTag(targetTag))
        {
            // Increase the score using the GameManager
            GameManager.Instance?.AddScore(scoreValue);

            Debug.Log($"Collision with {targetTag}! Score increased by {scoreValue}.");
        }
        else
        {
        SoundManager.PlaySound(SoundManager.Instance.glasSounds);
        }
        // tODO: also sund for seed on seed action
    }

    private void BounceFromGround()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * bounceForce);
        SoundManager.PlaySound(SoundManager.Instance.floorSounds);
    }

    private void BecomePlant()
    {
        gameObject.SetActive(false);
        plantBody.transform.position = gameObject.transform.position;
        plantBody.SetActive(true);
        Debug.Log("Im a plant now");
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the collided object has the target tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Increase the score using the GameManager
            GameManager.Instance?.AddScore(scoreValue);

            // Optional: Destroy the object after collision
            // Destroy(collision.gameObject);

            Debug.Log($"Collision with {targetTag}! Score increased by {scoreValue}.");
        }
    }
}

