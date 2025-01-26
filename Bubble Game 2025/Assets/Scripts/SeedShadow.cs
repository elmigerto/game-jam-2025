using UnityEngine;

public class SeedShadow : MonoBehaviour
{
    //[SerializeField] Transform seedTarget;
    void LateUpdate()
    {
        transform.position =  new Vector3(transform.parent.transform.position.x, 0, transform.parent.transform.position.z);
        transform.rotation = Quaternion.identity;
    }
}
