using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public int damagevalue = 1;

    void OnCollisionEnter(Collision collision)
    {
        // collision with a other player
        var touchedplayer = collision.gameObject.GetComponent<PlayerMovement>();
        if (touchedplayer != null)
        {
            touchedplayer.SendMessage("TakeDamage", damagevalue);//.gameObject.GetComponent<Rigidbody>().AddExplosionForce(bounceForce, collision.contacts[0].point, bounceRadius);
            Debug.Log("TakeDamage sent");
        }
    }
}
