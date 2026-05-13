using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ArrowVolleyCooldownUISlots : MonoBehaviour
{
    public ArrowVolley abilitySource;
    public Image cooldownOverlay;
    public TextMeshProUGUI TextElement;
    private void Start()
    {
        TextElement.gameObject.SetActive(true);
    }

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
