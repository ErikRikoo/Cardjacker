using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float MovementSpeed = 10;
    public float LookAtSpeed = 2;
    public Transform model;

    public GameObject interactionGO;

    public Interaction currentInteraction = null;

    public List<CardData> cards = new List<CardData>();
    public List<ArgumentData> startingArguments = new List<ArgumentData>();
    private List<ArgumentData> arguments = new List<ArgumentData>();
    public List<RuntimeArgumentData> runtimeArguments = new List<RuntimeArgumentData>();

    public PerksData[] perks = new PerksData[5];

    public TextMeshProUGUI moneyText;

    public int money = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BuyUI.instance.gameObject.SetActive(false);

        for (int i = 0; i < startingArguments.Count; i++)
        {
            AddArgument(startingArguments[i]);
        }
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

        if (keyboard.fKey.isPressed && currentInteraction != null && !BuyUI.instance.gameObject.activeSelf)
        {
            currentInteraction.Execute();
            BuyUI.instance.gameObject.SetActive(true);
            BuyUI.instance.ShowBuyerCards((Buyer)currentInteraction);
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

    public void AddArgument(ArgumentData inArgumentData)
    {
        arguments.Add(inArgumentData);
        runtimeArguments.Add(new RuntimeArgumentData { argumentData = inArgumentData, used = false });
    }

    public void SetArgumentUsed(int index)
    {
        RuntimeArgumentData runtimeArgumentData = runtimeArguments[index];
        runtimeArgumentData.used = true;
        runtimeArguments[index] = runtimeArgumentData;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = "Money : " + money;
    }
}