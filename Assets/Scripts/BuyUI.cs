using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Phase
{
    BUYER_SELECTION,
    SCAMMING
}

public class BuyUI : MonoBehaviour
{
    public static BuyUI instance;

    public TextMeshProUGUI[] goalText;
    public int goal = 0;
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public Slider slider;

    private bool scamming = false;
    public float timer = 30;
    private float currentTimer = 30;

    public Argument currentArgument;
    public Card currentCard;

    public Transform buyerCardsPanel;
    public RectTransform buyingCardsPanel;
    public Card CardPrefab;
    public Argument argumentPrefab;

    private List<Card> cards = new List<Card>();

    public Phase currentPhase = Phase.BUYER_SELECTION;

    public GameObject buyerSelectionGO;
    public GameObject scammingGO;

    public Transform scammingBuyerCardsPanel;

    public bool showingPlayerCards = true;
    public GameObject playerCardsPanel;
    public GameObject playerArgsPanel;
    public RectTransform scammingDropPanel;

    public Image[] perksImages;

    public Button startButton;

    private bool ending = false;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int score)
    {
        goal += score;
        for (int i = 0; i < goalText.Length; i++)
        {
            goalText[i].text = "Goal : " + goal;
        }
    }

    public void AddCardToGoal(Card card)
    {
        if (cards.Contains(card))
            return;

        cards.Add(card);
        AddScore(card.value);

        startButton.interactable = true;
    }

    public void RemoveCardToGoal(Card card)
    {
        if (!cards.Contains(card))
            return;

        cards.Remove(card);
        AddScore(-card.value);

        if (cards.Count <= 0)
        {
            startButton.interactable = false;
        }
    }

    public void StartScam()
    {
        startButton.interactable = false;
        for (int p = 0; p < Player.instance.perks.Length; p++)
        {
            if (Player.instance.perks[p] != null)
            {
                perksImages[p].color = Color.red;
            }
            else
            {
                perksImages[p].color = Color.white;
            }
        }

        SwitchPhase(Phase.SCAMMING);
        int max = buyingCardsPanel.childCount;
        for (int i = 0; i < max; i++)
        {
            buyingCardsPanel.GetChild(0).GetComponent<Card>().canBeMoved = false;
            buyingCardsPanel.GetChild(0).parent = scammingBuyerCardsPanel;
        }
        for (int i = 0; i < Player.instance.cards.Count; i++)
        {
            Card c = Instantiate(CardPrefab, playerCardsPanel.transform).GetComponent<Card>();
            c.SetInfo(Player.instance.cards[i]);
        }
        for (int i = 0; i < Player.instance.runtimeArguments.Count; i++)
        {
            if (!Player.instance.runtimeArguments[i].used)
            {
                Argument c = Instantiate(argumentPrefab, playerArgsPanel.transform).GetComponent<Argument>();
                c.SetArgInfo(Player.instance.runtimeArguments[i].argumentData);
                c.index = i;
            }
        }
    }

    public void Leave()
    {
        Clean();
        Quit();
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

    public void ShowBuyerCards(Buyer buyer)
    {
        for (int i = 0; i < buyer.cards.Count; i++)
        {
            Card c = Instantiate(CardPrefab, buyerCardsPanel).GetComponent<Card>();
            c.SetInfo(buyer.cards[i]);
        }
    }

    public void SwitchPhase(Phase phase)
    {
        switch (currentPhase)
        {
            case Phase.BUYER_SELECTION:
                buyerSelectionGO.SetActive(false);
                break;
            case Phase.SCAMMING:
                scammingGO.SetActive(false);
                break;
        }

        currentPhase = phase;

        switch(currentPhase)
        {
            case Phase.BUYER_SELECTION:
                buyerSelectionGO.SetActive(true);
                break;
            case Phase.SCAMMING:
                scammingGO.SetActive(true);
                break;
        }
    }

    public void SwitchPlayerPanel()
    {
        showingPlayerCards = !showingPlayerCards;
        if (showingPlayerCards)
        {
            playerCardsPanel.SetActive(true);
            playerArgsPanel.SetActive(false);
        }
        else
        {
            playerCardsPanel.SetActive(false);
            playerArgsPanel.SetActive(true);
        }
    }

    public void EndScam()
    {
        if(ending)
        {
            return;
        }    

        ending = true;

        StartCoroutine(EndingScam());
    }

    public IEnumerator EndingScam()
    {
        float upValue = 40;

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        WaitForSeconds waitForSecondsBig = new WaitForSeconds(1);
        WaitForSeconds waitForSecondsSmall = new WaitForSeconds(0.3f);

        int max = scammingDropPanel.childCount;
        for (int i = 0; i < max; i++)
        {
            Card c = scammingDropPanel.GetChild(i).GetComponent<Card>();
            if (!c.isArgumentCard)
            {
                c.transform.position += Vector3.up * upValue;

                yield return waitForSecondsSmall;

                c.Shake();
                int tempScore = c.value;
                c.scoreText.text = tempScore.ToString();
                int index = i + 1;
                int argsUsed = 0;

                while (index < max)
                {
                    Card cBis = scammingDropPanel.GetChild(index).GetComponent<Card>();
                    if (cBis.isArgumentCard)
                    {
                        yield return waitForSecondsSmall;

                        cBis.transform.position += Vector3.up * upValue * 0.66f;

                        yield return waitForSecondsSmall;

                        cBis.Shake();

                        Argument arg = cBis.GetComponent<Argument>();
                        switch(arg.currentArgumentData.argumentType)
                        {
                            case ArgumentType.MULT:
                                cBis.scoreText.text = "x" + arg.value;

                                yield return waitForSecondsSmall;

                                tempScore *= arg.value;
                                c.scoreText.text = tempScore.ToString();
                                break;
                        }
                        yield return waitForSecondsSmall;
                    }
                    else
                    {
                        break;
                    }
                    index++;
                }

                yield return waitForSecondsBig;

                for (int p = 0; p < Player.instance.perks.Length; p++)
                {
                    if (Player.instance.perks[p] != null)
                    {
                        perksImages[p].transform.localScale = Vector3.one;
                        yield return waitForSecondsSmall;
                        switch (Player.instance.perks[p].perkType)
                        {
                            case PerkType.POST_CARD:
                                tempScore *= 2;
                                c.scoreText.text = tempScore.ToString();
                                break;
                        }
                        yield return waitForSecondsSmall;
                        perksImages[p].transform.localScale = Vector3.one * 0.75f;
                    }
                }

                yield return waitForSeconds;

                score += tempScore;
                scoreText.text = "Score : " + score;
                i += argsUsed;
            }
        }

        yield return new WaitForSeconds(1);

        CheckScore();
        Clean();
        Quit();
    }

    private void Clean()
    {
        for (int i = 0; i < buyerCardsPanel.childCount; i++)
        {
            Destroy(buyerCardsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < buyingCardsPanel.childCount; i++)
        {
            Destroy(buyingCardsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < scammingBuyerCardsPanel.childCount; i++)
        {
            Destroy(scammingBuyerCardsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerCardsPanel.transform.childCount; i++)
        {
            Destroy(playerCardsPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerArgsPanel.transform.childCount; i++)
        {
            Destroy(playerArgsPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < scammingDropPanel.childCount; i++)
        {
            Destroy(scammingDropPanel.GetChild(i).gameObject);
        }

        score = 0;
        scoreText.text = "Score : " + score;

        goal = 0;
        for (int i = 0; i < goalText.Length; i++)
        {
            goalText[i].text = "Goal : " + goal;
        }

        if (!showingPlayerCards)
        {
            SwitchPlayerPanel();
        }
    }

    private void CheckScore()
    {
        if (score >= goal)
        {
            int max = scammingBuyerCardsPanel.childCount;
            for (int i = 0; i < max; i++)
            {
                CardData cardData = scammingBuyerCardsPanel.GetChild(i).GetComponent<Card>().currentCardData;
                Player.instance.cards.Add(cardData);
                CollectionManager.instance.ReceiveCard(cardData);
            }
            Buyer b = Player.instance.currentInteraction.GetComponent<Buyer>();
            for (int i = 0; i < max; i++)
            {
                b.cards.Remove(scammingBuyerCardsPanel.GetChild(i).GetComponent<Card>().currentCardData);
            }

            max = scammingDropPanel.childCount;
            for (int i = 0; i < max; i++)
            {
                Card c = scammingDropPanel.GetChild(i).GetComponent<Card>();
                if (!c.isArgumentCard)
                { 
                    b.cards.Add(scammingDropPanel.GetChild(i).GetComponent<Card>().currentCardData);
                }
            }
            for (int i = 0; i < max; i++)
            {
                Player.instance.cards.Remove(scammingDropPanel.GetChild(i).GetComponent<Card>().currentCardData);
            }

            Player.instance.AddMoney(score - goal);
        }
        else
        {
        }
    }

    private void Quit()
    {
        ending = false;
        SwitchPhase(Phase.BUYER_SELECTION);
        gameObject.SetActive(false);
    }
}
