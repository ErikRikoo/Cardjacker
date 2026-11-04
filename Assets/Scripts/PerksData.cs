using UnityEngine;

[CreateAssetMenu(fileName = "PerksData", menuName = "ScriptableObjects/PerksData", order = 5)]
public class PerksData : ScriptableObject
{
    public string perksName;
    public string perksDescription;
    public Texture perksImage;
    public int perksIndex;
    public PerkType perkType;
}

public enum PerkType
{
    PRE_GAME,
    PRE_CARD,
    PRE_ARG,
    POST_ARG,
    POST_CARD,
    POST_GAME,
    EVERTIME,
}