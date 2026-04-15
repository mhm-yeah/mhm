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
            currentXP -= targetXP;
            currentLVL++;
            targetXP += targetXPIncrease;
            Debug.Log("Leveled up to " + currentLVL);
            
            if (currentLVL % levelsPerCard == 0)
            {
                Debug.Log("Level up trigger");
                if (cardUI.HasCardsAvailable())
                    TriggerCardSelection();
            }
        }
    }

    IEnumerator TriggerCardSelection()
    {
        Debug.Log("Triggering card selection");
        yield return new WaitForSeconds(3);
        Debug.Log("Showing card selection");
        cardUI.ShowCards();
    }
    // Update is called once per frame
    void UpdateUI()
    {
         currentLVLText.text = "LV." + currentLVL;
         xpBar.fillAmount = (float)currentXP / (float)targetXP;
    }
}
