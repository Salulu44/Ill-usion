using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance { get; private set; }
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource dialogueSource;
    public float currentMusicVolume = .3f;
    public float currentPitch = .6f;
    void Start()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        //DontDestroyOnLoad(gameObject);
        SFXSource.volume = .3f;
    }
    public AudioSource GetMusicAudio() => musicSource;
    public AudioClip GetCurrentClip()
    {
        return musicSource.clip;
    }
    public void SetNoMusic()
    {
        musicSource.clip = null;
    }
    public void SetAudioTime(float time)
    {
        musicSource.time = time;
    }
    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        musicSource.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!SFXSource.isPlaying)
        {
            SFXSource.clip = clip;
            musicSource.volume = volume;
            SFXSource.Play();
        }
    }
    public void PlayMusic(AudioClip clip, float volume = 1f, float pitch = 1)
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.pitch = pitch;
            currentPitch = musicSource.pitch;
            musicSource.Play();
        }
    }
    public void PlayDialogue(AudioClip clip, float volume = 1f, float pitch = 1)
    {
        if (!dialogueSource.isPlaying)
        {
            dialogueSource.clip = clip;
            dialogueSource.volume = volume;
            dialogueSource.pitch = pitch;
            currentPitch = dialogueSource.pitch;
            dialogueSource.Play();
        }
    }
}
