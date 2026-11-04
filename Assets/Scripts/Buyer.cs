using System.Collections.Generic;
using UnityEngine;

public class Buyer : Interaction
{
    public List<CardData> cards = new List<CardData>();

    public CardDataRessource cardDataRessource;

    private void Start()
    {
        int rand = Random.Range(2, 5);
        for (int i = 0; i < rand; i++)
        {
            int randCard = Random.Range(0, cardDataRessource.cardDatas.Length);
            cards.Add(cardDataRessource.cardDatas[randCard]);
        }
    }

    public override void Execute()
    {
    }
}
