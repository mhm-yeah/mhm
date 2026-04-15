using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Button button;

    CardData card;
    CardSelectionUI parent;

    public void Setup(CardData newCard, CardSelectionUI ui)
    {
        card = newCard;
        parent = ui;
        title.text = card.name;
        description.text = card.description;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        parent.OnCardSelected(card);
    }
}