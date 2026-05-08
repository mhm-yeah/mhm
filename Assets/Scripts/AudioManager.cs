using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip enemyDamaged;
    public AudioClip playerDamaged;
    public AudioClip bossBattle;
    public AudioClip MainMenu;
    private void Start()
    {
        if (background != null) {
            musicSource.clip = background;
            musicSource.Play(); 
        }
        if (MainMenu != null) {
            musicSource.clip = MainMenu;
            musicSource.Play(); 
        }


    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
