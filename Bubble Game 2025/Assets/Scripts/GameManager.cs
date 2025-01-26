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

 


    [Header("UI Settings")]
    public TextMeshProUGUI scoreTMPText; // For TextMeshPro
    public TextMeshProUGUI p1text; // For TextMeshPro
    public TextMeshProUGUI p2text; // For TextMeshPro
    public TextMeshProUGUI levelText;
    public GameObject StartingInfos;
    public float DisableInfosTime = 2f;

    [Header("Audio Settings")]
    public AudioClip backgroundMusic; // Assign your background music in the Inspector
    public float musicVolume = 0.5f; // Volume level for background music

    private AudioSource audioSource;
    private int maxLevel = 50;

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
        RestartGame();
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
        var playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        player.transform.position = spawnPoint.position;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        AssignModel(player);
        SoundManager.PlayPlayerSound(SoundManager.Instance.playerStartingVoice, playerMovement.playerSoundNumber);


        playerCount++;
    }

    private void AssignModel(PlayerInput player)
    {
        Transform player1Model = player.transform.Find("Player1");
        Transform player2Model = player.transform.Find("Player2");

        if (player1Model == null || player2Model == null)
        {
            Debug.LogWarning("Player models are missing! Ensure Player1 and Player2 are children of the prefab.");
            return;
        }

        var playerMovement = player.gameObject.GetComponent<PlayerMovement>();
            playerMovement.playerIndex = player.playerIndex;
            playerMovement.playerSoundNumber = UnityEngine.Random.Range(0, 3); // 1=dani, 2=tobi, 3=Anic, 4=Arthur
            Debug.Log(playerMovement.playerSoundNumber);
        // Use the player index to determine which model to activate
        if (player.playerIndex % 2 == 0) // Even player index: Show Player1
        {
            player1Model.gameObject.SetActive(true);
            player2Model.gameObject.SetActive(false);
            Instance.p1text.text = "o o o o o";

        }
        else // Odd player index: Show Player2
        {
            player1Model.gameObject.SetActive(false);
            player2Model.gameObject.SetActive(true);
            Instance.p2text.text = "o o o o o";
        }


        Debug.Log($"Assigned model for Player {player.playerIndex}");
    }

    // Resets all game variables
    public void ResetGame()
    {
        Score = 0;
        Level = 1;
        GameTime = 0f;
        IsGameRunning = true;
    }


    // respanws the player when it leaves the sphere
    public static void RespawnPlayer(GameObject player)
    {
        Transform spawnPoint = Instance.spawnPoints[Instance.playerCount % Instance.spawnPoints.Length]; // Cycle through spawn points
        player.transform.position = spawnPoint.position;
    }

    // Adds to the player's score
    public void AddScore(int points)
    {
        Score += points;
        UpdateScoreUI();
        SoundManager.PlayPlayerSound(SoundManager.Instance.playerScoreVoice, UnityEngine.Random.Range(0, 3));
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
        if (scoreTMPText != null)
        {
            scoreTMPText.text = $"Score: {Score}";
        }
    }

    internal static void OnPlayerTakeDamage(PlayerMovement player)
    {
        player.lifePoints = Mathf.Clamp(player.lifePoints, 0, 5); // Ensure input is between 0 and 5
        var displayString = string.Join(" ", new string('o', player.lifePoints).PadRight(5, 'x').ToCharArray());
        Instance.p1text.text = displayString;
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

    internal static void UpdateLevel(int v)
    {
        Instance.levelText.text = $"Level: {(Instance.maxLevel - v)}"; //.ToString();
    }

    void RestartGame()
    {
        ResetGame();
        EnableStartingInfos();
        Invoke("DisableStartingInfos", DisableInfosTime);
    }


    void EnableStartingInfos()
    {
        StartingInfos.SetActive(true);
    }


    void DisableStartingInfos()
    {
        StartingInfos.SetActive(false);
    }
}
