using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefense : MonoBehaviour
{
    [Header("Defense Settings")]
    [SerializeField] private float damageReductionMultiplier = 0.5f;

    [Header("Visual Indicator")]
    [SerializeField] private GameObject defenseIndicator;

    private bool isDefending;

    public bool IsDefending => isDefending;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        isDefending = Mouse.current.rightButton.isPressed;

        if (defenseIndicator != null)
        {
            defenseIndicator.SetActive(isDefending);
        }
    }

    public int ApplyDefense(int incomingDamage)
    {
        if (!isDefending)
            return incomingDamage;

        return Mathf.CeilToInt(incomingDamage * damageReductionMultiplier);
    }
}