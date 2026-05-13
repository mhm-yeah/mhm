using UnityEngine;

public class AbilitySoundPlayer : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            audioManager.PlaySFX(audioManager.fireballSound);

        if (Input.GetKeyDown(KeyCode.Q))
            audioManager.PlaySFX(audioManager.lightningSound);

        if (Input.GetKeyDown(KeyCode.R))
            audioManager.PlaySFX(audioManager.arrowVolleySound);

        if (Input.GetKeyDown(KeyCode.T))
            audioManager.PlaySFX(audioManager.iceBlastSound);
    }
}