using System.Collections.Generic;
using UnityEngine;

public class CardSelectionUI : MonoBehaviour
{
    public PlayerAbilities player;
    public List<CardData> allCards;
    public CardButton[] buttons;
    List<CardData> currentCards;

    public void ShowCards()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        int amount = Mathf.Min(3, allCards.Count);
        currentCards = GetRandomCards(amount);

        // disable all buttons first
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(i < amount);
        }

        for (int i = 0; i < amount; i++)
        {
            buttons[i].Setup(currentCards[i], this);
        }
    }

    public void OnCardSelected(CardData card)
    {
        player.ApplyAbility(card.abilityID);
        allCards.RemoveAll(c => c.abilityID == card.abilityID);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    List<CardData> GetRandomCards(int amount)
    {
        List<CardData> pool = new List<CardData>(allCards);
        List<CardData> result = new List<CardData>();

        int actualAmount = Mathf.Min(amount, pool.Count);

        for (int i = 0; i < actualAmount; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }
    public bool HasCardsAvailable()
    {
        return allCards.Count > 0;
    }
}