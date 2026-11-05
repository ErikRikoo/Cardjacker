using Sirenix.Serialization;
using System.Collections.Generic;
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
    public ArgumentDataRessource argumentDataRessource;

    public Card CardPrefab;
    public Card ArgPrefab;

    public GameObject boosterMenu;

    public int cardsBooster = 1;
    public int argsBooster = 1;

    public bool isCardBooster = true;

    private void Start()
    {
        CloseBoosterMenu();
    }

    public void ClickedOnBooster()
    {
        boosterClickNum++;
        if (boosterClickNum >= boosterClickNumToReach)
        {
            boosterClickNum = 0;
            List<CardData> cardsReceived = new List<CardData>();
            List<ArgumentData> argsReceived = new List<ArgumentData>();
            if (isCardBooster)
            {
                for (int i = 0; i < 3; i++)
                {
                    int rand = Random.Range(0, cardDataRessource.cardDatas.Length);
                    Player.instance.cards.Add(cardDataRessource.cardDatas[rand]);
                    cardsReceived.Add(cardDataRessource.cardDatas[rand]);
                    CollectionManager.instance.ReceiveCard(cardDataRessource.cardDatas[rand]);
                }
            }
            else
            {
                int rand = Random.Range(0, argumentDataRessource.argumentDatas.Length);
                Player.instance.AddArgument(argumentDataRessource.argumentDatas[rand]);
                argsReceived.Add(argumentDataRessource.argumentDatas[rand]);
            }
            boosterButton.SetActive(false);

            showingPanel.gameObject.SetActive(true);

            for (int i = 0; i < cardsReceived.Count; i++)
            {
                Card c = Instantiate(CardPrefab, showingPanel).GetComponent<Card>();
                c.SetInfo(cardsReceived[i]);
                c.canBeMoved = false;
            }
            for (int i = 0; i < argsReceived.Count; i++)
            {
                Argument c = Instantiate(ArgPrefab, showingPanel).GetComponent<Argument>();
                c.SetArgInfo(argsReceived[i]);
                c.canBeMoved = false;
            }

            okButton.SetActive(true);
        }
    }

    public void CloseBoosterMenu()
    {
        showingPanel.gameObject.SetActive(false);

        okButton.SetActive(false);

        for (int i = 0; i < showingPanel.childCount; i++)
        {
            Destroy(showingPanel.GetChild(i).gameObject);
        }

        if (cardsBooster > 0)
        {
            boosterButton.SetActive(true);
            isCardBooster = true;
            cardsBooster--;
            return;
        }
        else if (argsBooster > 0)
        {
            boosterButton.SetActive(true);
            isCardBooster = false;
            argsBooster--;
            return;
        }

        boosterMenu.SetActive(false);
    }
}
