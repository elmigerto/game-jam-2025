using UnityEngine;

public class CollisionSeedHandler : MonoBehaviour
{
    public string targetTag = "SphereWorld"; // Tag of the objects that will trigger score increment
    public int scoreValue = 10; // Points to add when collision happens

    private void OnCollisionEnter(Collision collision)
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
