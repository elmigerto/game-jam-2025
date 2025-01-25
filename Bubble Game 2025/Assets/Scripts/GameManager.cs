using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(0, 1)]
    public float timeScaleFactor = 0.3f;
    public Transform[] spawnPoints;  // Assign spawn points in Inspector
    public Material[] playerMaterials;  // Assign spawn points in Inspector
    private int playerCount = 0;

    // Global game variables
    public int Score { get; private set; }
    public int Level { get; private set; }
    public float GameTime { get; private set; } // Tracks the total game time in seconds

    public bool IsGameRunning { get; private set; }

    public Text scoreText; // Legacy UI
    public TextMeshProUGUI scoreTMPText; // For TextMeshPro

    [Header("Audio Settings")]
    public AudioClip backgroundMusic; // Assign your background music in the Inspector
    public float musicVolume = 0.5f; // Volume level for background music

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = timeScaleFactor;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = musicVolume;

        PlayBackgroundMusic();
    }

    void Update()
    {
        if (IsGameRunning)
        {
            GameTime += Time.deltaTime;
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} joined!");

        Transform spawnPoint = spawnPoints[playerCount % spawnPoints.Length]; // Cycle through spawn points
        player.transform.position = spawnPoint.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        //AssignMaterial(player);

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

    // Resets all game variables
    public void ResetGame()
    {
        Score = 0;
        Level = 1;
        GameTime = 0f;
        IsGameRunning = true;
    }

    // Adds to the player's score
    public void AddScore(int points)
    {
        Score += points;
        UpdateScoreUI();
        SoundManager.PlaySound(SoundManager.Instance.pointSound);
    }

    // Advances to the next level
    public void NextLevel()
    {
        Level++;
    }

    // Stops the game
    public void StopGame()
    {
        IsGameRunning = false;
        StopBackgroundMusic();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Score}";
        }

        if (scoreTMPText != null)
        {
            scoreTMPText.text = $"Score: {Score}";
        }
    }

    internal static void OnPlayerDestroyed(GameObject gameObject)
    {

        SoundManager.PlaySound(SoundManager.Instance.playerDeadSound);
        Debug.Log("A player has died!!");
    }


    public void PlayBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
