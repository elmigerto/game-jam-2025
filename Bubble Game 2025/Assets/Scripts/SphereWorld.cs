using UnityEngine;

public class SphereWorld : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the object that will trigger the event
    public string seedTag = "Seed";
    public int scoreSeed = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"{other.gameObject.name} passed through the trigger!");

            // Add your custom logic here
            PerformAction();
        }
    }

    private void PerformAction()
    {
        // Example: Log or trigger something
        Debug.Log("Action triggered!");
    }



    private void OnTriggerExit(Collider collision)
    {
        Debug.Log($"ExitTrigger.");
        // Check if the collided object has the target tag
        if (collision.gameObject.CompareTag(seedTag))
        {
            // Increase the score using the GameManager
            GameManager.Instance?.AddScore(scoreSeed);

            Debug.Log($"Collision with {seedTag}! Score increased by {scoreSeed}.");
        }
    }
}
