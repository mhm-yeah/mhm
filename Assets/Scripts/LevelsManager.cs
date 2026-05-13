using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    [Header("References")]
    private GameManager gameManager;
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
    private float waitForCardSelection = 2f;
    AudioManager audioManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentLVL= 0;
        currentXP = 0;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        UpdateUI();
    }
    public void IncreaseXP(float amount)
    {
        gameManager.totalScore += amount;
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
            audioManager.PlaySFX(audioManager.LevelUp);
            Debug.Log("Leveled Up! LVL: " + currentLVL);
            if (currentLVL % levelsPerCard == 0)
            {
                if (cardUI.HasCardsAvailable())
                    TriggerCardSelection();
                    //Invoke(nameof(TriggerCardSelection), waitForCardSelection);
            }
        }
    }

    void TriggerCardSelection() {
        Time.timeScale = 0;
        cardUI.ShowCards();
    }
    
    void UpdateUI()
    {
         currentLVLText.text = "LV." + currentLVL;
         xpBar.fillAmount = (float)currentXP / (float)targetXP;
    }
}
