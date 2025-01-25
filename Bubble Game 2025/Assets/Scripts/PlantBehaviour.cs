using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public int damagevalue = 1;

    public AudioClip growSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        PlaySound(growSound);
    }
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

    private void PlaySound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
