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
    [SerializeField] int targetXP = 100;
    [SerializeField] int targetXPIncrease= 25;
    int currentLVL;
    int currentXP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLVL= 0;
        Update();
    }
    public void IncreaseXP(int amount)
    {
        currentXP += amount;
        CheckForLVLUP();
        Update();
    }
    public void CheckForLVLUP()
    {
        while(currentXP >= targetXP)
        {
            currentLVL++;
            currentXP -= targetXP;
            targetXP += targetXPIncrease;
        }
    }
    // Update is called once per frame
    void Update()
    {
        currentLVLText.text = "LV." + currentLVL;
        xpBar.fillAmount = (float)currentXP / (float)targetXP;
    }
}
