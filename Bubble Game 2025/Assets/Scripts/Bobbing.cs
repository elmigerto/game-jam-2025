using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mathf;

public class Bobbing : MonoBehaviour
{
    [SerializeField] float bobbingOffset = 0;
    [SerializeField] float bobbingAmount = 0.1f;
    [SerializeField] private bool doRandomOffset = true;
    private float bobbingInterval = .3f; //in seconds
    private float randomOffsetAmount = 0;
    private Vector3 initialScale;
    private float timeScaleFactor;
    void Start(){
        if (doRandomOffset)
        {
            randomOffsetAmount = Random.Range(0f,1f);
        }
        initialScale = transform.localScale;
    }
    void Update()
    {
        if (Time.timeScale != 0)
        {
            timeScaleFactor = 1/Time.timeScale;
        }
        float newScale = (Pow(Sin((Time.time*timeScaleFactor/bobbingInterval+randomOffsetAmount+bobbingOffset-0.5f)*PI)+1,1.6f))/2; //curve
        newScale = Lerp(newScale,1,1-bobbingAmount); //remap
        transform.localScale = Vector3.Scale(new Vector3(2-newScale,newScale,2-newScale),initialScale);
    }
}
