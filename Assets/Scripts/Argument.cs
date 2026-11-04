using TMPro;

public class Argument : Card
{
    public TextMeshProUGUI descriptionText;

    public Card linkedCard;

    public ArgumentData currentArgumentData;

    public int index;

    public void SetArgInfo(ArgumentData argumentData)
    {
        currentArgumentData = argumentData;
        value = argumentData.argumentValue;
        valueText.text = "x" + value.ToString();
        nameText.text = argumentData.argumentName;
        descriptionText.text = argumentData.argumentDescription;
    }
}
