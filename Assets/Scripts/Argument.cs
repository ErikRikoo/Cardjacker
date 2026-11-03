using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Argument : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] selectionRect;

    public bool selected = false;

    public int value = 1;

    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        value = Random.Range(1, 3);
        textMeshPro.text = "x" + value.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;

        if (selected)
        {
            FindAnyObjectByType<BuyUI>().SelectArgument(this);
        }
        else
        {
            FindAnyObjectByType<BuyUI>().SelectArgument(null);
        }

        UpdateSelectionImages();
    }

    public void UpdateSelectionImages()
    {
        for (int i = 0; i < selectionRect.Length; i++)
        {
            selectionRect[i].SetActive(selected);
        }
    }
}
