using UnityEngine;

public class SeedBehaviour : MonoBehaviour
{
    public int despawnCount = 3;
    public GameObject arena;
    public GameObject plantBody;

    void OnCollisionEnter(Collision collision)
    {
        // collision with a other player
        // Debug.Log("Collision:  " + collision.gameObject.name);
        if (collision.gameObject.name == arena.name)
        {
            Debug.LogWarning("here");
            despawnCount -= 1;
            if (despawnCount == 0)
            {
                BecomePlant();
            }
        }
        else{
            Debug.Log("hit that" + collision.gameObject.name);
        }
    }

    private void BecomePlant()
    {
        gameObject.SetActive(false);
        plantBody.transform.position = gameObject.transform.position;
        plantBody.SetActive(true);
        Debug.Log("Im a plant now");
    }
}

