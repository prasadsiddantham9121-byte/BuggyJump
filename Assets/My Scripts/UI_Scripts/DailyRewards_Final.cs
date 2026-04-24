using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DailyRewards_Final : MonoBehaviour
{
    private int dayValue;
    public GameObject rewards;

    // Start is called before the first frame update
    void Start()
    {
        //============================= This block is for only Testing for day=7 reward ==============
        //dayValue = 6;
        //PlayerPrefs.SetInt("DayValue", 6);
        //PlayerPrefs.Save();
        //============================= This block is for only Testing for day=7 reward ==============


        dayValue = PlayerPrefs.GetInt("DayValue", 0);
        CheckDailyRewards();
        SetDailyRewardButtonInteraction();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Close()
    {
        MenuScript.instance.Sub.SetActive(true);
        MenuScript.instance.MenuSoundOff();
        rewards.SetActive(false);
    }
    System.DateTime NextRewardTime, FirstRewardTime;
    public void CheckDailyRewards()
    {
        //PlayerPrefs.GetString("Day", DateTime.Now.ToString());
        //PlayerPrefs.SetString("Day", DateTime.Now.ToString());
        //Show Daily Rewards Screen


        if (DateLoader.Date == null && dayValue < 7)
        {
            //First Time run
            if (PlayerPrefs.GetString("Day") == "")
            {
                rewards.SetActive(true);
                ButtonHighlighter buttonHighlighter = rewards.GetComponent<ButtonHighlighter>();
                if (buttonHighlighter != null && rewardButtons != null && rewardButtons.Count > dayValue && rewardButtons[dayValue].gameObject != null)
                {
                    buttonHighlighter.SetDefaultButton(rewardButtons[dayValue].gameObject);
                    Debug.Log("You got a reward today");
                }

                Debug.Log("You got a reward today");

            }
            else
            {
                string s = PlayerPrefs.GetString("Day");
                NextRewardTime = Convert.ToDateTime(s);

                if (NextRewardTime.Subtract(DateTime.Now).Hours <= 0)
                {

                    rewards.SetActive(true);
                    ButtonHighlighter buttonHighlighter = rewards.GetComponent<ButtonHighlighter>();
                    if (buttonHighlighter != null && rewardButtons != null && rewardButtons.Count > dayValue && rewardButtons[dayValue].gameObject != null)
                    {
                        buttonHighlighter.SetDefaultButton(rewardButtons[dayValue].gameObject);
                        Debug.Log("You got a reward today");
                    }
                    Debug.Log("You got a reward today");
                }
                else
                {
                    // rewards.SetActive(false);

                    MenuScript.instance.Sub.SetActive(true);
                    MenuScript.instance.MenuSoundOff();
                    //Debug.LogError("You already claimed reward");
                    print("Reward Already Claimd");


                }
            }

            DateLoader.Date = PlayerPrefs.GetString("Day");
        }
        else
        {
            MenuScript.instance.Sub.SetActive(true);
            MenuScript.instance.MenuSoundOff();
            print("Condition checking");
        }


        //Show Spin wheel if wheels > 0
    }
    public List<Button> rewardButtons = new List<Button>();
    public List<int> dailyCoins = new List<int> { 100, 150, 200, 250, 300, 350, 400 };
    public List<Sprite> dailySprites = new List<Sprite>();
    public GameObject rewardPopupPanel;
    public TextMeshProUGUI rewardPopupText;
    //public Image rewardImage;
    public void RewardClaimButton()
    {

        FirstRewardTime = DateTime.Now;
        NextRewardTime = FirstRewardTime.AddDays(1);
        PlayerPrefs.SetString("Day", NextRewardTime.ToString());
        int coinsForToday = dailyCoins[dayValue];

        if (!PlayerPrefs.HasKey("TotalCoins"))
        {
            PlayerPrefs.SetInt("TotalCoins", coinsForToday);
            PlayerPrefs.Save();
        }
        else
        {
            int totalCoins = PlayerPrefs.GetInt("TotalCoins");
            totalCoins += coinsForToday;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.Save();
        }
        //MenuScript.instance.UpdateTotalCoins(coinsForToday);
        ShowRewardPopup(coinsForToday);
        dayValue++;
        PlayerPrefs.SetInt("DayValue", dayValue);

        // 🎁 Day 7 reward
        if (dayValue == 7)
        {
            UnlockNextCar();
        }

        if (dayValue >= 7)
        {
            dayValue = 0;
            PlayerPrefs.SetInt("DayValue", dayValue);
        }

    }
    void SetDailyRewardButtonInteraction()
    {
        foreach (Button b in rewardButtons)
        {
            b.interactable = false;
        }
        rewardButtons[dayValue].interactable = true;
    }
    void ShowRewardPopup(int coins)
    {
        rewardPopupText.text = $"DAILY REWARD {coins} RECEIVED!";
        //rewardImage.sprite = dailySprites[dayValue];  // Change the sprite based on the current day
        rewardPopupPanel.SetActive(true);
        foreach (Button b in rewardButtons)
        {
            b.interactable = false;
        }// Activate the panel to show the message

    }
    public void Okay()
    {
        rewardPopupPanel.SetActive(false);
        MenuScript.instance.Sub.SetActive(true);
        MenuScript.instance.MenuSoundOff();
        rewards.SetActive(false);
    }
    public int GetCoinsForToday()
    {
        return dailyCoins[dayValue];
    }
    // Hide the panel after delay

    void UnlockNextCar()
    {
        int totalCars = 10; // 👈 set based on your game

        for (int i = 1; i < totalCars; i++) // start from 1 (0 is default unlocked)
        {
            if (PlayerPrefs.GetInt("Char" + i, 0) == 0)
            {
                // 🔓 Unlock this car
                PlayerPrefs.SetInt("Char" + i, 1);
                PlayerPrefs.Save();

                Debug.Log("Unlocked Car: Char" + i);

                // Optional UI
                rewardPopupText.text = $"NEW CHARACTER UNLOCKED! Char {i + 1}";
                rewardPopupPanel.SetActive(true);

                return; // 🚨 VERY IMPORTANT → stop after unlocking ONE car
            }
        }

        Debug.Log("All cars already unlocked");
    }

}

public static class DateLoader
{
    public static string Date = null;
    public static int volume = 1;
}


