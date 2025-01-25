using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioClip> floorSounds;
    public List<AudioClip> glasSounds;
    public List<AudioClip> bounceSounds;
    public List<AudioClip> growSounds;
    public AudioClip spawnSound;
    public AudioClip playerJumpSound;
    public AudioClip playerDeadSound;
    public AudioClip pointSound;
    public AudioClip player1HitSounds;
    public AudioClip player1MovementSounds;
    public List<AudioClip> player1IdleSounds;

    // public AudioClip player2HitSounds;
    // public AudioClip player2MovementSounds;
    // public List<AudioClip> player2IdleSounds;
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
        // todo select random sound
        int randomIndex = Random.Range(0, sounds.Count); // Random index
        var randomSound = sounds[randomIndex]; // Get the value
        PlaySound(randomSound);
    }
}
