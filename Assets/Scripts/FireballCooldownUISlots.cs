using UnityEngine;
using UnityEngine.UI;
public class FireballCooldownUISlots : MonoBehaviour
{
    public Fireball abilitySource;
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
