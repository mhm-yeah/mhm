using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image healthBar;
    PlayerHealth playerHealth;
    PlayerStats playerStats;
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Debug.Log(healthBar.fillAmount);
    }

    void Update()
    {
        healthBar.fillAmount = playerHealth.GetCurrentHealth() / playerStats.maxHealth;
    }
}
