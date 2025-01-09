using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText; // Assign the Text UI element for money in the Inspector
    private float money = 0f;

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: {money}";
        }
    }
}
