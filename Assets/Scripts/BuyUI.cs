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

    private List<Card> cards = new List<Card>();

    public Phase currentPhase = Phase.BUYER_SELECTION;

    public GameObject buyerSelectionGO;
    public GameObject scammingGO;

    public Transform scammingBuyerCardsPanel;

    public bool showingPlayerCards = true;
    public GameObject playerCardsPanel;
    public GameObject playerArgsPanel;
    public RectTransform scammingDropPanel;

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
        for (int i = 0; i < buyer.cards.Length; i++)
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
                        switch(arg.argType)
                        {
                            case ArgType.MULT:
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

                yield return waitForSeconds;

                score += tempScore;
                scoreText.text = "Score : " + score;
                i = index;
            }
        }

        yield return new WaitForSeconds(1);

        CleanAndQuit();
    }

    private void CleanAndQuit()
    {
        if (score >= goal)
        {
            int max = scammingBuyerCardsPanel.childCount;
            for (int i = 0; i < max; i++)
            {
                Player.instance.cards.Add(scammingBuyerCardsPanel.GetChild(i).GetComponent<Card>().currentCardData);

            }
        }
        else
        {
        }

        SwitchPhase(Phase.BUYER_SELECTION);
        gameObject.SetActive(false);
    }
}
