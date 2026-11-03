using UnityEngine;

public class Buyer : Interaction
{
    public CardData[] cards;

    public override void Execute()
    {
        Debug.Log("Start Buy");
    }
}
