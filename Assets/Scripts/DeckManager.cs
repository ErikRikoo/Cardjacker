using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform cardPanel;
    public int bookIndex;
    public int numberCardMax = 27;
    public Card CardPrefab;

    public Button nextButton;
    public Button prevButton;

    public void OpenBook()
    {
        gameObject.SetActive(true);

        bookIndex = 0;

        FillCardBook();
        if (Player.instance.cards.Count < ((bookIndex + 1) * numberCardMax) + 1)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
    }

    public void FillCardBook()
    {
        for (int i = 0; i < cardPanel.childCount; i++)
        {
            Destroy(cardPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < numberCardMax; i++)
        {
            if (Player.instance.cards.Count < i + 1)
            {
                return;
            }

            Card c = Instantiate(CardPrefab, cardPanel).GetComponent<Card>();
            c.SetInfo(Player.instance.cards[i]);
            c.canBeMoved = false;
        }
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (Player.instance.cards.Count < ((bookIndex + 1) * numberCardMax) + 1)
        {
            return;
        }

        bookIndex++;
        FillCardBook();

        prevButton.interactable = true;

        if (Player.instance.cards.Count < ((bookIndex + 1) * numberCardMax) + 1)
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
        FillCardBook();

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
}
