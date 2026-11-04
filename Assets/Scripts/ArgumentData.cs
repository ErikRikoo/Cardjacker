using UnityEngine;

[CreateAssetMenu(fileName = "ArgumentData", menuName = "ScriptableObjects/ArgumentData", order = 4)]
public class ArgumentData : ScriptableObject
{
    public string argumentName;
    public string argumentDescription;
    public ArgumentType argumentType;
    public int argumentValue;
    public Texture argumentImage;
    public int argumentIndex;
}

[System.Serializable]
public struct RuntimeArgumentData
{
    public ArgumentData argumentData;
    public bool used;
}

public enum ArgumentType
{
    ADD,
    MULT,
}
