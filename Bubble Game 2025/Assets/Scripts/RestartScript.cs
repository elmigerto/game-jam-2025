using UnityEngine;

public class RestartScript : MonoBehaviour
{

    public GameObject GameManager;
    public void OnButtonPress()
    {
        gameObject.SetActive(false);
        var gameManagerScript = GameManager.GetComponent<GameManager>();
        gameManagerScript.RestartGame();
        Debug.Log("Button Pressed!"); // Replace this with your actual functionality
    }

    
}
