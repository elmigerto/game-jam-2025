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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == arena.name)
        {
            Debug.LogWarning("here");
            despawnCount -= 1;
            if (despawnCount == 0)
            {
                BecomePlant();
            }else{
                BounceFromGround();
            }
        }
        else{
            Debug.Log("hit that" + collision.gameObject.name);
        }

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

    private void BounceFromGround()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * bounceForce);
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

