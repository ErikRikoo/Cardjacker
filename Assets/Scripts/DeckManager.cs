using UnityEngine;

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

    public void OpenBook()
    {
        gameObject.SetActive(true);
        FillCardBook();
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
}
