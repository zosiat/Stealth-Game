using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Default Volumes")]
    public float defaultMusicVolume = 0.3f;
    public float defaultSFXVolume = 0.8f;

    public float SFXVolume { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}