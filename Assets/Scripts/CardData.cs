using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 2)]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int cardValue;
    public Texture cardImage;
    public int cardIndex;
}

public struct RuntimeCardData
{
    public CardData cardData;
}

public enum CardType
{
    LEGENDARY,
    EPIC,
    RARE,
    COMMON
}
