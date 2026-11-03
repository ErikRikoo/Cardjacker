using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] selectionRect;

    public bool selected = false;

    public int value = 100;

    public bool isPlayerCard = false;

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI nameText;

    public Image background;

    public Color commonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    private void Start()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CardMovingManager.instance.CardMoved = this;
        transform.parent = transform.parent.parent;

        /*if (!isPlayerCard)
        {
            if (selected)
            {
                FindAnyObjectByType<BuyUI>().AddScore(value);
            }
            else
            {
                FindAnyObjectByType<BuyUI>().AddScore(-value);
            }
        }
        else
        {
            if (selected)
            {
                FindAnyObjectByType<BuyUI>().SelectPlayerCard(this);
            }
            else
            {
                FindAnyObjectByType<BuyUI>().SelectPlayerCard(null);
            }
        }

        UpdateSelectionImages();*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CardMovingManager.instance.CardMoved == this)
        {
            CardMovingManager.instance.DropCard();
        }

        /*if (!isPlayerCard)
        {
            if (selected)
            {
                FindAnyObjectByType<BuyUI>().AddScore(value);
            }
            else
            {
                FindAnyObjectByType<BuyUI>().AddScore(-value);
            }
        }
        else
        {
            if (selected)
            {
                FindAnyObjectByType<BuyUI>().SelectPlayerCard(this);
            }
            else
            {
                FindAnyObjectByType<BuyUI>().SelectPlayerCard(null);
            }
        }

        UpdateSelectionImages();*/
    }

    public void UpdateSelectionImages()
    {
        for (int i = 0; i < selectionRect.Length; i++)
        {
            selectionRect[i].SetActive(selected);
        }
    }

    public void SetInfo(CardData cardData)
    {
        value = cardData.cardValue;
        valueText.text = "$" + value.ToString();
        nameText.text = cardData.cardName;

        switch(cardData.cardType)
        {
            case CardType.COMMON:
                background.color = commonColor;
                break;
            case CardType.RARE:
                background.color = rareColor;
                break;
            case CardType.EPIC:
                background.color = epicColor;
                break;
            case CardType.LEGENDARY:
                background.color = legendaryColor;
                break;
        }
    }
}
