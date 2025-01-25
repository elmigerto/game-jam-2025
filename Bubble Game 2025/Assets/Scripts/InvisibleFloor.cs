using UnityEngine;

public class InvisibleFloor : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the object that will trigger the event
    public string seedTag = "Seed";
    public int scoreSeed = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.gameObject.CompareTag(playerTag);
            Destroy(other.gameObject);
           }
    }
}
