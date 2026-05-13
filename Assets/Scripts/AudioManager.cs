using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background1;
    public AudioClip background2;
    public AudioClip background3;
    public AudioClip enemyDamaged;
    public AudioClip playerDamaged;
    public AudioClip bossBattle;
    public AudioClip MainMenu;
    public AudioClip LevelUp;
    private AudioClip currentTrack;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (MainMenu != null) {
            musicSource.clip = MainMenu;
            musicSource.Play(); 
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip newClip)
    {
        if (newClip == currentTrack)
        {
            return;
        }

        currentTrack = newClip;

       // StopAllCoroutines();
        StartCoroutine(SmoothTransition(newClip));
    }

    private IEnumerator SmoothTransition(AudioClip newClip)
    {
        float fadeSpeed = 2f;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        while (musicSource.volume < 1)
        {
            musicSource.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 1;
    }
}
