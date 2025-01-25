using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Range(0,1)]
    public float timeScaleFactor = 0.3f;
    public Transform[] spawnPoints;  // Assign spawn points in Inspector
    public Material[] playerMaterials;  // Assign spawn points in Inspector
    private int playerCount = 0;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

     Time.timeScale = timeScaleFactor;
    }
    
    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} joined!");
        
        Transform spawnPoint = spawnPoints[playerCount % spawnPoints.Length]; // Cycle through spawn points
        player.transform.position = spawnPoint.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        AssignMaterial(player);

        playerCount++;
    }

     private void AssignMaterial(PlayerInput player)
    {
        Renderer playerRenderer = player.GetComponentInChildren<Renderer>(); // Find player’s renderer
        if (playerRenderer != null && playerMaterials.Length > 0)
        {
            int materialIndex = playerCount % playerMaterials.Length; // Cycle through materials
            playerRenderer.material = playerMaterials[materialIndex];

            Debug.Log($"Assigned Material {playerMaterials[materialIndex].name} to Player {player.playerIndex}");
        }
        else
        {
            Debug.LogWarning("No Renderer found on player or materials list is empty!");
        }
    }
}
