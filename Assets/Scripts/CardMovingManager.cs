using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovingManager : MonoBehaviour
{
    public static CardMovingManager instance = null;
    

    private void Awake()
    {
        instance = this;
    }

    public Card cardMoved = null;
    public Card currentCardPlayed = null;

    private void Update()
    {
        if (cardMoved != null)
        {
            cardMoved.GetComponent<RectTransform>().position = new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, 0);
        }
    }

    public void DropCard()
    {
        if (cardMoved == null)
        {
            return;
        }

        switch(BuyUI.instance.currentPhase)
        {
            case Phase.BUYER_SELECTION:
                if (MouseInside(BuyUI.instance.buyingCardsPanel))
                {
                    cardMoved.transform.parent = BuyUI.instance.buyingCardsPanel;
                    BuyUI.instance.AddCardToGoal(cardMoved);
                }
                else
                {
                    cardMoved.transform.parent = BuyUI.instance.buyerCardsPanel;
                    BuyUI.instance.RemoveCardToGoal(cardMoved);
                }
                break;
            case Phase.SCAMMING:
                if (MouseInside(BuyUI.instance.scammingDropPanel))
                {
                    cardMoved.transform.parent = BuyUI.instance.scammingDropPanel;
                    if (!cardMoved.isArgumentCard)
                    {
                        currentCardPlayed = cardMoved;
                        BuyUI.instance.SwitchPlayerPanel();
                    }
                    else
                    {
                        cardMoved.GetComponent<Argument>().linkedCard = currentCardPlayed;
                    }
                    cardMoved.canBeMoved = false;
                }
                else
                {
                    if (!cardMoved.isArgumentCard)
                    {
                        cardMoved.transform.parent = BuyUI.instance.playerArgsPanel.transform;
                    }
                    else
                    {
                        cardMoved.transform.parent = BuyUI.instance.playerCardsPanel.transform;
                    }
                }
                break;
        }

        cardMoved = null;
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
