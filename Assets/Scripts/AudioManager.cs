using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // This script's singleton (in case we need these, we may go without singeltons at all)
    public static AudioManager instance;

    // Audio Sources
    public AudioSource sfxSource;
    public AudioSource musicSource;

    // SFX Clips
    public AudioClip coinCollect;
    public AudioClip bouncePad;
    public AudioClip growPad;
    public AudioClip shrinkPad;
    public AudioClip wallBreak;
    public AudioClip portalEnter;
    public AudioClip portalOpen;
    public AudioClip deathSound;
    public AudioClip buttonSound;
    public AudioClip projectileSound;

    // Music Clips
    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;
    public AudioClip loseMusic;
    public AudioClip winMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persists between levels - Keeps GameObject alive between scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // The method and parameter to help shorten code in other scripts when playinga sound from this one.
    // Clip is the parameter here, and the argument in another script, where clip will be replaced with the sound's name
    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip track)
    {
        musicSource.clip = track;
        musicSource.loop = true;
        musicSource.Play();
    }
}