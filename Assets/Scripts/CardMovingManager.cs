using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CardMovingManager : MonoBehaviour
{
    public static CardMovingManager instance = null;
    

    private void Awake()
    {
        instance = this;
    }

    public Card CardMoved = null;

    private void Update()
    {
        if (CardMoved != null)
        {
            CardMoved.GetComponent<RectTransform>().position = new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, 0);
        }
    }

    public void DropCard()
    {
        if (CardMoved == null)
        {
            return;
        }

        if (MouseInside(BuyUI.instance.buyingCardsPanel))
        {
            CardMoved.transform.parent = BuyUI.instance.buyingCardsPanel;
            BuyUI.instance.AddCardToGoal(CardMoved);
        }
        else
        {
            CardMoved.transform.parent = BuyUI.instance.buyerCardsPanel;
            BuyUI.instance.RemoveCardToGoal(CardMoved);
        }

        CardMoved = null;
    }

    private bool MouseInside(RectTransform rectTransform)
    {
        if (Mouse.current.position.x.value < rectTransform.GetLocalLeft())
            return false;
        if (Mouse.current.position.x.value > rectTransform.GetLocalRight())
            return false;

        if (Mouse.current.position.y.value > rectTransform.GetLocalTop())
            return false;
        if (Mouse.current.position.y.value < rectTransform.GetLocalBottom())
            return false;

        return true;
    }
}
