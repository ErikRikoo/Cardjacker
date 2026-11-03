using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 10;
    public float LookAtSpeed = 2;
    public Transform model;

    public GameObject interactionGO;

    public GameObject BuyGo;
    private BuyUI buyUI;

    private Interaction currentInteraction = null;

    public CardData[] cards;

    private void Start()
    {
        buyUI = BuyGo.GetComponent<BuyUI>();
        BuyGo.SetActive(false);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return; // No Keyboard connected.

        Vector3 desiredMovement = Vector3.zero;

        if (keyboard.wKey.isPressed)
        {
            desiredMovement += Vector3.forward;
        }
        else if (keyboard.sKey.isPressed)
        {
            desiredMovement -= Vector3.forward;
        }

        if (keyboard.dKey.isPressed)
        {
            desiredMovement += Vector3.right;
        }
        else if (keyboard.aKey.isPressed)
        {
            desiredMovement -= Vector3.right;
        }

        transform.position += desiredMovement.normalized * Time.deltaTime * MovementSpeed;

        if (keyboard.fKey.isPressed && currentInteraction != null && !BuyGo.activeSelf)
        {
            currentInteraction.Execute();
            BuyGo.SetActive(true);
            buyUI.ShowBuyerCards((Buyer)currentInteraction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();
        if (interaction != null)
        {
            currentInteraction = interaction;
            interactionGO.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interaction interaction = other.GetComponentInParent<Interaction>();
        if (interaction == currentInteraction)
        {
            currentInteraction = null;
            interactionGO.SetActive(false);
        }
    }
}