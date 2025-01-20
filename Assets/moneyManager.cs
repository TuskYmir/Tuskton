using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    public Text CurrentMoneyText; // Assign the Text UI element for money in the Inspector
    public Text CurrentFoodText;
    public Text CurrentResouceText;
    private int money = 0;
    private int food = 0;
    private int resources = 0;

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void AddFood(int amount)
    {
        food += amount;
        UpdateFoodText();
    }

    public void AddResource(int amount)
    {
        resources += amount;
        UpdateResourceText();
    }

    private void UpdateMoneyText()
    {
        if (CurrentMoneyText != null)
        {
            CurrentMoneyText.text = $"Money: {money}";
        }
    }

    public void UpdateFoodText()
    {
        if (CurrentFoodText != null)
        {
            CurrentFoodText.text = $"Food: {food}";
        }

    }

    public void UpdateResourceText()
    {
        if (CurrentResouceText != null)
        {
            CurrentResouceText.text = $"Resources: {resources}";
        }
    }

}
