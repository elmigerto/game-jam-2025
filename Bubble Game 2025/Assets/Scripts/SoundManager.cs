using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AudioClipPlayer
{
    public string playerNumber; // Optional: Add a name for this group
    public List<AudioClip> clips = new List<AudioClip>();
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioClip> floorSounds = new List<AudioClip>();
    public List<AudioClip> glasSounds = new List<AudioClip>();
    public List<AudioClip> bounceSounds = new List<AudioClip>();
    public List<AudioClip> growSounds = new List<AudioClip>();
    public AudioClip spawnSound;


    [Header("Player Audio Clips")]
    public List<AudioClipPlayer> playerMovementSounds = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerJumpSounds = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerStartingVoice = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerDeadVoice = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerScoreVoice = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerDamageVoice = new List<AudioClipPlayer>();
    public List<AudioClipPlayer> playerIdleVoice = new List<AudioClipPlayer>();


    //  public List<AudioClip> playerJumpSounds = new List<AudioClip>();
    //  public List<AudioClip> playerStartingVoice = new List<AudioClip>();
    //  public List<AudioClip> playerDeadVoice = new List<AudioClip>();
    //  public List<AudioClip> playerScoreVoice = new List<AudioClip>();
    //   public List<AudioClip> playerDamageVoice = new List<AudioClip>();
    //    public List<AudioClip> playerIdleVoice = new List<AudioClip>();
    public float soundVolume = 1f; // Public variable to adjust volume


    private AudioSource audioSource;
    private static bool isTalking;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Instance.audioSource = gameObject.AddComponent<AudioSource>();

    }
    public static void PlaySound(AudioClip sound)
    {
        if (sound != null && Instance.audioSource != null)
        {

            Instance.audioSource.PlayOneShot(sound);
        }
    }

    public static void PlaySound(List<AudioClip> sounds)
    {
        if (sounds != null && sounds.Count > 0)
        {
            int randomIndex = Random.Range(0, sounds.Count - 1); // Random index
            var randomSound = sounds[randomIndex]; // Get the value
            PlaySound(randomSound);
        }
    }

    public static async Task PlayPlayerSound(IList<AudioClipPlayer> playerSounds, int player = 0)
    {
        if(isTalking) return;
        isTalking = true;
        if (playerSounds != null && playerSounds.Count > 0)
        {
            var playerNumber = Mathf.Min(player, playerSounds.Count - 1);
            var sounds = playerSounds[playerNumber].clips;
            sounds = sounds.Count > 0 ? sounds : playerSounds[0].clips;
            PlaySound(sounds);
        }
        await Task.Delay(500);
        isTalking = false;
    }
}
