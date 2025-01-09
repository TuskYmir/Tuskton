using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time value")]
    public int maxActs = 2;
    public int currentActs = 2;
    public int day = 0;
    public int week = 0;
    public int month = 0;
    public int year = 0;

    [Header("Time text")]
    public Text actsText;
    public Text dayText;
    public Text weekText;
    public Text monthText;
    public Text yearText;

    [Header("Skip buttons")]
    public GameObject skipDayButton;
    public GameObject skipWeekButton;
    public GameObject skipMonthButton;
    public GameObject skipYearButton;

    private void Update()
    {
        actsText.text = $"Acts: {currentActs}/{maxActs}";
        dayText.text = $"Day: {day}";
        weekText.text = $"Week: {week}";
        monthText.text = $"Month: {month}";
        yearText.text = $"Year: {year}";

        skipDayButton.GetComponent<Image>().color = currentActs == 0 ? new Color(0, 1, 0, 1) : new Color(1, 1, 1, 1);
        skipWeekButton.SetActive(day == 5);
        skipMonthButton.SetActive(week == 4 && day == 5);
        skipYearButton.SetActive(month == 6 && week == 4 && day == 5);
    }

    public void SkipDay()
    {
        if (day <= 4)
        {
            day += 1;
            currentActs = maxActs;
        }
    }

    public void SkipWeek()
    {
        if (week <= 3)
        {
            week += 1;
            currentActs = maxActs;
            day = 0;
            skipWeekButton.SetActive(false);
        }
    }
    public void SkipMonth()
    {
        if (month <= 5)
        {
            month += 1;
            currentActs = maxActs;
            day = 0;
            week = 0;
            skipMonthButton.SetActive(false);
        }
    }

    public void SkipYear()
    {
        year += 1;
        currentActs = maxActs;
        day = 0;
        week = 0;
        month = 0;
        skipYearButton.SetActive(false);
    }
}
