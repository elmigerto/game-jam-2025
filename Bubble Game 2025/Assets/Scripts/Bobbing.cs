using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mathf;

public class Bobbing : MonoBehaviour
{
    [SerializeField] float bobbingOffset = 0;
    [SerializeField] float bobbingAmount = 0.1f;
    private float bobbingInterval = .3f; //in seconds
    void Update()
    {
        float newScale = (Pow(Sin((Time.time/bobbingInterval+bobbingOffset-0.5f)*PI)+1,1.6f))/2; //curve
        newScale = Lerp(newScale,1,1-bobbingAmount); //remap
        transform.localScale = new Vector3(2-newScale,newScale,2-newScale);
    }
}
