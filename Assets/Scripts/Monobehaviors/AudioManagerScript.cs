using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance { get; private set; }
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX;
    public float currentMusicVolume = .3f;
    public float currentPitch = .6f;
    void Start()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSourceSFX.volume = .3f;
    }
    public AudioSource GetMusicAudio() => audioSourceMusic;
    public AudioClip GetCurrentClip()
    {
        return audioSourceMusic.clip;
    }
    public void SetNoMusic()
    {
        audioSourceMusic.clip = null;
    }
    public void SetAudioTime(float time)
    {
        audioSourceMusic.time = time;
    }
    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        audioSourceMusic.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        audioSourceSFX.volume = volume;
    }
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!audioSourceSFX.isPlaying)
        {
            audioSourceSFX.clip = clip;
            audioSourceMusic.volume = volume;
            audioSourceSFX.Play();
        }
    }
    public void PlayMusic(AudioClip clip, float volume = 1f, float pitch = 1)
    {
        audioSourceMusic.clip = clip;
        audioSourceMusic.volume = volume;
        audioSourceMusic.pitch = pitch;
        currentPitch = audioSourceMusic.pitch;
        audioSourceMusic.Play();
    }
}
