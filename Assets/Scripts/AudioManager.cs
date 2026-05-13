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

    [Header("Ability SFX")]
    public AudioClip fireballSound;
    public AudioClip iceBlastSound;
    public AudioClip arrowVolleySound;
    public AudioClip lightningSound;

    public AudioClip LevelUp;
    private void Start()
    {
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}