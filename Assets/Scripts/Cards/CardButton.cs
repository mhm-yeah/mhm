using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Button button;
    public Image iconImage;

    CardData card;
    CardSelectionUI parent;

    public void Setup(CardData newCard, CardSelectionUI ui)
    {
        card = newCard;
        parent = ui;

        title.text = card.name;
        description.text = card.description;

        Ability ability = parent.player.GetAbility(card.abilityID);

        if (iconImage != null)
        {
            iconImage.enabled = true;

            if (ability != null && ability.icon != null)
            {
                iconImage.sprite = ability.icon;

                Debug.Log("Assigned icon: " + ability.icon.name);
            }
            else
            {
                Debug.LogWarning("Icon missing for: " + card.abilityID);
            }
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        parent.OnCardSelected(card);
    }
}