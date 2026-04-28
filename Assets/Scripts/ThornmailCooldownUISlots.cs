using UnityEngine;
using UnityEngine.UI;
public class ThornmailCooldownUISlots : MonoBehaviour
{
    public Thornmail abilitySource;
    public Image cooldownOverlay;
    

    // Update is called once per frame
    void Update()
    {
        if (abilitySource.isOnCooldown)
        {
            cooldownOverlay.fillAmount = abilitySource.GetCooldownTimer() / abilitySource.cooldownTime;
        }
        else
        {
            cooldownOverlay.fillAmount = 0;
        }
    }
}
