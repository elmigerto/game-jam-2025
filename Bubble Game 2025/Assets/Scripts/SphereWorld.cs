using UnityEngine;

public class SphereWorld : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the object that will trigger the event
    public string seedTag = "Seed";
    public int scoreSeed = 10;

    private void OnTriggerEnter(Collider collision)
    {
        // if (other.CompareTag(playerTag))
        // {
        //     other.gameObject.GetComponent<PlayerMovement>().TakeDamage(1);
        //     GameManager.RespawnPlayer(other.gameObject);
        // }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(1);
            GameManager.RespawnPlayer(collision.gameObject);
        }
        // Check if the collided object has the target tag
        if (collision.gameObject.CompareTag(seedTag))
        {
            // Increase the score using the GameManager
            GameManager.Instance?.AddScore(scoreSeed);
            Destroy(collision.gameObject, 3f);
        }
    }
}
