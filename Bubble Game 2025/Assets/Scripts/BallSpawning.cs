using System;
using UnityEngine;

public class BallSpawning : MonoBehaviour
{
    public GameObject BallPrefab;
    private System.Random rnd = new System.Random();
    public float interval = 2f; // Time interval in seconds
    public int forceHoricontal = 4;
    public int forceVertical = 2;
    private float timer = 0f;  // Tracks elapsed time

    // Update is called once per frame
    void Update()
    {
        // Increment timer by the time elapsed since the last frame
        timer += Time.deltaTime;

        // Check if the timer has reached or exceeded the interval
        if (timer >= interval)
        {
            timer -= interval; // Reset timer (keep remainder for accuracy)
            InstantiateSeed();        // Call the desired method
        }
    }

    public void InstantiateSeed()
    {
        var currentBall = Instantiate(BallPrefab, this.transform.position + new Vector3(0, 2, 0), new Quaternion());
        var vX = rnd.Next(-forceHoricontal, forceHoricontal);
        var vY = forceVertical;
        var vZ = rnd.Next(-forceHoricontal, forceHoricontal);
        currentBall.GetComponentInChildren<Rigidbody>().linearVelocity = new Vector3(vX, vY, vZ);

        SoundManager.PlaySound(SoundManager.Instance.spawnSound);
    }

}
