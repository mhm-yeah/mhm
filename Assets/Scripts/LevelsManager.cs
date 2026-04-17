using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currentLVLText;
    [SerializeField] Image xpBar;

    [Space(10)]
    [Header("Settings")]
    [SerializeField] float targetXP = 100;
    [SerializeField] float targetXPIncrease = 25;

    [SerializeField] int levelsPerCard = 3;
    [SerializeField] CardSelectionUI cardUI;
    int currentLVL;
    float currentXP;

    void Start()
    {
        currentLVL= 0;
        currentXP = 0;
        UpdateUI();
    }
    public void IncreaseXP(float amount)
    {
        currentXP += amount;
        CheckForLVLUP();
        UpdateUI();
    }

    public void CheckForLVLUP()
    {
        while (currentXP >= targetXP)
        {
            currentLVL++;
            currentXP -= targetXP;
            targetXP += targetXPIncrease;
            Debug.Log("Leveled Up! LVL: " + currentLVL);
            if (currentLVL % levelsPerCard == 0)
            {
                if (cardUI.HasCardsAvailable())
                    //TriggerCardSelection();
                    Invoke(nameof(TriggerCardSelection), 2f);
            }
        }
    }

    void TriggerCardSelection() {
        Time.timeScale = 0f;
        cardUI.ShowCards();
    }
    
    void UpdateUI()
    {
         currentLVLText.text = "LV." + currentLVL;
         xpBar.fillAmount = (float)currentXP / (float)targetXP;
    }
}
