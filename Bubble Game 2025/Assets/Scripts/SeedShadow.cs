using UnityEngine;

public class SeedShadow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    // Update is called once per frame
    void Update()
    {
        this.transform.position =  new Vector3(transform.position.x, 0, transform.position.z);
    }
}
