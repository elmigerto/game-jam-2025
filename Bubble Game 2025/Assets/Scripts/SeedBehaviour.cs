using System;
using UnityEngine;

public class SeedBehaviour : MonoBehaviour
{
    public int despawnCount = 3;
    public GameObject arena;
    public GameObject plantBody;
    public float bounceForce = 30;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == arena.name)
        {
            // Debug.LogWarning("here");
            despawnCount -= 1;
            if (despawnCount == 0)
            {
                BecomePlant();
            }else{
                BounceFromGround();
            }
        }
        else{
            // Debug.Log("hit that" + collision.gameObject.name);
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
}

