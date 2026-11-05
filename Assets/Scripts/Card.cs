using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] selectionRect;

    public int value = 100;

    public bool isPlayerCard = false;

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI nameText;

    public CardData currentCardData;

    public TextMeshProUGUI scoreText;
    public GameObject startingState;

    public Image background;

    public Color commonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    public bool isArgumentCard = false;

    public bool canBeMoved = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canBeMoved)
        {
            return;
        }

        CardMovingManager.instance.cardMoved = this;
        transform.parent = transform.parent.parent;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canBeMoved)
        {
            return;
        }

        if (CardMovingManager.instance.cardMoved == this)
        {
            CardMovingManager.instance.DropCard();
        }
    }

    public void SetInfo(CardData cardData)
    {
        currentCardData = cardData;
        value = cardData.cardValue;
        valueText.text = "$" + value.ToString();
        nameText.text = cardData.cardName;

        startingState.SetActive(false);

        switch (cardData.cardType)
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

    public void Shake()
    {
        StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Cos(Time.time * 50) * 5);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }
}
