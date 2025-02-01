using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int gems = 0;
    public int selectedShipId = 0;
    public List<int> purchasedShips = new List<int>();

    // For Monthly Subscription
    public bool isMonthlySubscriptionActive = false;
    public System.DateTime subscriptionEndDate = System.DateTime.MinValue;
}

public static class GameDataManager
{
    static PlayerData playerData;

    static GameDataManager()
    {
        Initialize();
    }

    public static void Initialize()
    {
        if (playerData == null)
        {
            Debug.Log("<color=blue>[GameDataManager] Initializing playerData.</color>");
            LoadPlayerData();

            if (playerData.purchasedShips == null)
            {
                playerData.purchasedShips = new List<int>();
                SavePlayerData();
                Debug.Log("<color=orange>[GameDataManager] Fixed purchasedShips list and saved.</color>");
            }
        }
    }

    // Monthly Subscription Management -----------------------------------------------------------
    public static void ActivateMonthlySubscription(int days)
    {
        EnsureInitialized();
        playerData.isMonthlySubscriptionActive = true;
        playerData.subscriptionEndDate = System.DateTime.UtcNow.AddDays(days);
        SavePlayerData();
        Debug.Log($"<color=green>[GameDataManager] Monthly subscription activated for {days} days.</color>");
    }

    public static bool IsMonthlySubscriptionActive()
    {
        EnsureInitialized();

        if (playerData.isMonthlySubscriptionActive && System.DateTime.UtcNow <= playerData.subscriptionEndDate)
        {
            return true;
        }

        // Deactivate subscription if expired
        if (playerData.isMonthlySubscriptionActive && System.DateTime.UtcNow > playerData.subscriptionEndDate)
        {
            playerData.isMonthlySubscriptionActive = false;
            SavePlayerData();
            Debug.Log("<color=red>[GameDataManager] Monthly subscription expired.</color>");
        }

        return false;
    }

    public static string GetRemainingSubscriptionTime()
    {
        EnsureInitialized();

        if (playerData.isMonthlySubscriptionActive)
        {
            System.DateTime now = System.DateTime.UtcNow;
            if (now <= playerData.subscriptionEndDate)
            {
                System.TimeSpan remainingTime = playerData.subscriptionEndDate - now;
                return $"{remainingTime.Days} days {remainingTime.Hours} hours {remainingTime.Minutes} minutes left";
            }
            else
            {
                // Subscription expired
                playerData.isMonthlySubscriptionActive = false;
                SavePlayerData();
            }
        }
        return "Subscription expired";
    }

    // Coin Management ---------------------------------------------------------------------------
    public static int GetCoins()
    {
        EnsureInitialized();
        return playerData.coins;
    }

    public static void AddCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins += amount;
        SavePlayerData();
    }

    public static bool CanSpendCoins(int amount)
    {
        EnsureInitialized();
        return playerData.coins >= amount;
    }

    public static void SpendCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins -= amount;
        SavePlayerData();
    }

    // Gems Management ---------------------------------------------------------------------------
    public static int GetGems()
    {
        EnsureInitialized();
        return playerData.gems;
    }

    public static void AddGems(int amount)
    {
        EnsureInitialized();
        playerData.gems += amount;
        SavePlayerData();
    }

    public static bool CanSpendGems(int amount)
    {
        EnsureInitialized();
        return playerData.gems >= amount;
    }

    public static void SpendGems(int amount)
    {
        EnsureInitialized();
        playerData.gems -= amount;
        SavePlayerData();
    }

    // Ship Management ---------------------------------------------------------------------------
    public static bool IsShipPurchased(int shipId)
    {
        EnsureInitialized();

        if (playerData.purchasedShips == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] purchasedShips list is null. Initializing it now.</color>");
            playerData.purchasedShips = new List<int>();
        }

        return playerData.purchasedShips.Contains(shipId);
    }

    public static void PurchaseShip(int shipId, int cost)
    {
        EnsureInitialized();

        if (CanSpendCoins(cost) && !IsShipPurchased(shipId))
        {
            SpendCoins(cost);
            playerData.purchasedShips.Add(shipId);
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Ship {shipId} purchased for {cost} coins!</color>");
        }
    }

    public static void PurchaseVIPShip(int shipId, int cost)
    {
        EnsureInitialized();

        if (CanSpendGems(cost) && !IsShipPurchased(shipId))
        {
            SpendGems(cost);
            playerData.purchasedShips.Add(shipId);
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Ship {shipId} purchased for {cost} coins!</color>");
        }
    }

    public static void SetSelectedShip(int shipId)
    {
        EnsureInitialized();

        if (IsShipPurchased(shipId))
        {
            playerData.selectedShipId = shipId;
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Ship {shipId} selected!</color>");
        }
        else
        {
            Debug.LogWarning($"<color=red>[GameDataManager] Ship {shipId} is not purchased and cannot be selected.</color>");
        }
    }

    public static int GetSelectedShip()
    {
        EnsureInitialized();
        return playerData.selectedShipId;
    }

    public static int GetTotalPurchasedShips()
    {
        EnsureInitialized();
        return playerData.purchasedShips.Count;
    }

    public static void DeactivatePurchasedShip(int shipId)
    {
        EnsureInitialized();

        if (playerData.purchasedShips.Contains(shipId))
        {
            playerData.purchasedShips.Remove(shipId);

            if (playerData.selectedShipId == shipId)
            {
                playerData.selectedShipId = -1;
            }

            SavePlayerData();
        }
    }

    // Save and Load Player Data -----------------------------------------------------------------
    static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "Blackship-gametesting6-data.txt");
        Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
    }

    static void LoadPlayerData()
    {
        try
        {
            if (BinarySerializer.HasSaved("Blackship-gametesting6-data.txt"))
            {
                playerData = BinarySerializer.Load<PlayerData>("Blackship-gametesting6-data.txt");
                Debug.Log("<color=green>[PlayerData] Loaded.</color>");
            }
            else
            {
                playerData = new PlayerData();
                playerData.coins = 0;
                playerData.gems = 0;
                playerData.purchasedShips.Add(0);
                playerData.selectedShipId = 0;
                SavePlayerData();
                Debug.Log("<color=yellow>[GameDataManager] Initialized new data.</color>");
            }
        }
        catch (System.Exception ex)
        {
            playerData = new PlayerData();
            Debug.LogError($"<color=red>[GameDataManager] Failed to load data: {ex.Message}</color>");
        }
    }

    // Ensure Initialized ------------------------------------------------------------------------
    static void EnsureInitialized()
    {
        if (playerData == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] playerData was null. Reinitializing.</color>");
            Initialize();
        }
    }
}
