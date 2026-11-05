using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CardFound
{
    public CardData cardData;
    public bool found;
}

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform cardPanel;
    public int bookIndex;
    public int numberCardMax = 27;
    public Card CardPrefab;
    public CardDataRessource cardDataRessource;

    public List<CardFound> cardsFound = new List<CardFound>();

    public GameObject collectionPanel;

    public Button nextButton;
    public Button prevButton;

    private void Start()
    {
        for (int i = 0; i < cardDataRessource.cardDatas.Length; i++)
        {
            cardsFound.Add(new CardFound { cardData = cardDataRessource.cardDatas[i], found = false });
        }
    }

    public void ReceiveCard(CardData inCardData)
    {
        for (int i = 0; i < cardsFound.Count; i++)
        {
            if (cardsFound[i].cardData == inCardData)
            {
                CardFound c = cardsFound[i];
                c.found = true;
                cardsFound[i] = c;
                return;
            }
        }
    }
    
    public void NextPage()
    {
        if (cardsFound.Count < ((bookIndex + 1) * numberCardMax) + 1)
        {
            return;
        }

        bookIndex++;
        FillCollection();

        prevButton.interactable = true;

        if (cardsFound.Count < ((bookIndex + 1) * numberCardMax) + 1)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
    }

    public void PrevPage()
    {
        if ((bookIndex - 1) < 0)
        {
            return;
        }

        bookIndex--;
        FillCollection();

        nextButton.interactable = true;

        if ((bookIndex - 1) < 0)
        {
            prevButton.interactable = false;
        }
        else
        {
            prevButton.interactable = true;
        }
    }

    public void OpenCollection()
    {
        collectionPanel.SetActive(true);

        bookIndex = 0;

        FillCollection();
    }

    public void FillCollection()
    {
        for (int i = 0; i < cardPanel.childCount; i++)
        {
            Destroy(cardPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < numberCardMax; i++)
        {
            if (cardsFound.Count < (bookIndex * numberCardMax) + i + 1)
            {
                return;
            }

            Card c = Instantiate(CardPrefab, cardPanel).GetComponent<Card>();
            if (cardsFound[(bookIndex * numberCardMax) + i].found)
            {
                c.SetInfo(cardsFound[(bookIndex * numberCardMax) + i].cardData);
            }
            c.canBeMoved = false;
        }
    }

    public void CloseCollection()
    {
        collectionPanel.SetActive(false);
    }
}
