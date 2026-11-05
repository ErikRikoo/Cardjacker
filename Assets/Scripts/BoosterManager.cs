using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int boosterClickNum = 0;
    public int boosterClickNumToReach = 7;
    public GameObject boosterButton;
    public Transform showingPanel;
    public GameObject okButton;

    public CardDataRessource cardDataRessource;

    public Card CardPrefab;

    public GameObject boosterMenu;

    public void ClickedOnBooster()
    {
        boosterClickNum++;
        if (boosterClickNum >= boosterClickNumToReach)
        {
            boosterClickNum = 0;
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, cardDataRessource.cardDatas.Length);
                Player.instance.cards.Add(cardDataRessource.cardDatas[rand]);
                CollectionManager.instance.ReceiveCard(cardDataRessource.cardDatas[rand]);
            }
            boosterButton.SetActive(false);

            showingPanel.gameObject.SetActive(true);

            for (int i = 0; i < Player.instance.cards.Count; i++)
            {
                Card c = Instantiate(CardPrefab, showingPanel).GetComponent<Card>();
                c.SetInfo(Player.instance.cards[i]);
                c.canBeMoved = false;
            }

            okButton.SetActive(true);
        }
    }

    public void CloseBoosterMenu()
    {
        boosterMenu.SetActive(false);
    }
}
