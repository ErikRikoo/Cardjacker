using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyUI : MonoBehaviour
{
    public static BuyUI instance;

    public TextMeshProUGUI goalText;
    public int goal = 0;

    public Slider slider;

    private bool scamming = false;
    public float timer = 30;
    private float currentTimer = 30;

    public Argument currentArgument;
    public Card currentCard;

    public Transform buyerCardsPanel;
    public RectTransform buyingCardsPanel;
    public Card CardPrefab;

    private List<Card> cards = new List<Card>();

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int score)
    {
        goal += score;
        goalText.text = "Goal : " + goal;
    }

    public void AddCardToGoal(Card card)
    {
        if (cards.Contains(card))
            return;

        cards.Add(card);
        AddScore(card.value);
    }

    public void RemoveCardToGoal(Card card)
    {
        if (!cards.Contains(card))
            return;

        cards.Remove(card);
        AddScore(-card.value);
    }

    public void StartScam()
    {
        if (scamming)
        {
            return;
        }

        currentTimer = timer;
        scamming = true;
    }

    private void Update()
    {
        if (!scamming)
        {
            return;
        }

        currentTimer -= Time.deltaTime;
        slider.value = currentTimer/timer;
    }

    public void SelectArgument(Argument argument)
    {
        if (currentArgument != null)
        {
            currentArgument.selected = false;
            currentArgument.UpdateSelectionImages();
        }
        currentArgument = argument;
    }

    public void SelectPlayerCard(Card card)
    {
        if (currentCard != null)
        {
            currentCard.selected = false;
            currentCard.UpdateSelectionImages();
        }
        currentCard = card;
    }

    public void ShowBuyerCards(Buyer buyer)
    {
        for (int i = 0; i < buyer.cards.Length; i++)
        {
            Card c = Instantiate(CardPrefab, buyerCardsPanel).GetComponent<Card>();
            c.SetInfo(buyer.cards[i]);
        }
    }
}
