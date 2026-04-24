using UnityEngine;
using TMPro;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int totalCoins = 0;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initialize total coins with a default value of 10 if not already set
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 10);
        UpdateCoinUI(totalCoins);
        Debug.Log("Initial Total Coins: " + totalCoins);
    }

    public int GetTotalCoins() // Method to get the total coins
    {
        return totalCoins;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        UpdateCoinUI(totalCoins);
        Debug.Log("Added Coins: " + amount + ", Total Coins: " + totalCoins);
    }

    private void UpdateCoinUI(int amount)
    {
        if (coinText != null)
        {
            coinText.text = amount.ToString();
        }
    }

    // New Method: Add coins for level completion
    public void AddLevelCompletionCoins(int levelReward)
    {
        Debug.Log("Adding Level Completion Coins: " + levelReward);
        AddCoins(levelReward);
    }
}

