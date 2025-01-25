using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioClip> floorSounds = new List<AudioClip>();
    public List<AudioClip> glasSounds = new List<AudioClip>();
    public List<AudioClip> bounceSounds = new List<AudioClip>();
    public List<AudioClip> growSounds = new List<AudioClip>();
    public AudioClip spawnSound;
    public List<AudioClip> playerJumpSounds = new List<AudioClip>();
    public AudioClip playerMovementSound;
    public List<AudioClip> playerStartingVoice = new List<AudioClip>();
    public List<AudioClip> playerDeadVoice = new List<AudioClip>();
    public List<AudioClip> playerScoreVoice = new List<AudioClip>();
    public List<AudioClip> playerDamageVoice = new List<AudioClip>();
    public List<AudioClip> playerIdleVoice = new List<AudioClip>();
    public float soundVolume = 1f; // Public variable to adjust volume


    private AudioSource audioSource;

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

    public static void PlaySound(IList<AudioClip> sounds)
    {
        if (sounds != null && sounds.Count > 0)
        {
            int randomIndex = Random.Range(0, sounds.Count-1); // Random index
            var randomSound = sounds[randomIndex]; // Get the value
            PlaySound(randomSound);
        }
    }
}
