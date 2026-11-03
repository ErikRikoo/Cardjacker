using TMPro;
using UnityEngine;

public enum ArgType
{
    ADD,
    MULT,
}

public class Argument : MonoBehaviour
{
    public GameObject[] selectionRect;

    public int value = 1;

    public TextMeshProUGUI textMeshPro;

    public Card linkedCard;

    public ArgType argType;

    private void Start()
    {
        value = Random.Range(1, 3);
        textMeshPro.text = "x" + value.ToString();
    }
}
