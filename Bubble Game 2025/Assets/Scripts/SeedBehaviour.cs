using UnityEngine;

public class SeedBehaviour : MonoBehaviour
{
    public int despawnCount = 3;
    public GameObject arena;

    void OnCollisionEnter(Collision collision)
    {
        // collision with a other player
        if (collision.gameObject == arena)
        {
            despawnCount -= 1;
            if (despawnCount == 0)
            {
                BecomePlant();
            }
        }
    }

    private void BecomePlant()
    {
        Debug.Log("Im a plant now");
    }
}

