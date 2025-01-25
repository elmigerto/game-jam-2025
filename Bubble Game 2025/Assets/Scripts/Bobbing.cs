using UnityEngine;
using static UnityEngine.Mathf;

public class Bobbing : MonoBehaviour
{
    private float bobbingInterval = .3f; //in seconds

    // Update is called once per frame
    void Update()
    {
        float newScale = (Pow(Sin((Time.time/bobbingInterval-0.5f)*PI)+1,1.6f))/2;
        newScale = Lerp(newScale,1,0.9f); //remap
        transform.localScale = new Vector3(2-newScale,newScale,2-newScale);
    }
}
