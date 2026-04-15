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
    int currentLVL;
    float currentXP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        while(currentXP >= targetXP)
        {
            currentXP -= targetXP;
            currentLVL++;
            targetXP += targetXPIncrease;
            Debug.Log("Leveled Up!");
        }
    }
    // Update is called once per frame
    void UpdateUI()
    {
         currentLVLText.text = "LV." + currentLVL;
         xpBar.fillAmount = (float)currentXP / (float)targetXP;
    }
}
