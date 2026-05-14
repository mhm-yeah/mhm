using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color lowHealthColor = Color.red;
    public Color highHealthColor = Color.green;
    public Vector3 offset;

    private TextMeshProUGUI healthText;

    void Start()
    {
        healthText = slider.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;

        slider.fillRect.GetComponent<Image>().color = Color.Lerp(lowHealthColor, highHealthColor, slider.normalizedValue);
        if (healthText != null)
        {
            //slider.GetComponentInChildren<TextMeshProUGUI>();
            healthText.text = $"{Mathf.RoundToInt(health)}";
        }
        
    }
}
