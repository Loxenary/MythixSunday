using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    Damage,
    Health,
    Lives
}

public class StoreObjects : MonoBehaviour, ISaveLoad
{
    public List<StoreItem> BuyButton; // List of store items
    private Coins userCoins; // Player's coins
    private PricesData pricesData; // Saved data for prices
    private ResourcesSaveData resourcesSaveData;
    private DamageData damageData;

    [SerializeField] private TextMeshProUGUI coinPreview; 
    private void Start()
    {
        // Load saved data
        Load();

        // Initialize buttons
        InitializeButtons();
    }

    public void Load()
    {
        // Load saved data for prices
        pricesData = SaveLoadManager.Load<PricesData>();

        // If no saved data exists, initialize prices to 50
        if (pricesData == null ||  pricesData == default)
        {
            pricesData = new PricesData
            {
                DamagePrice = 50,
                HealthPrice = 50,
                LivesPrice = 50
            };
        }

        // Load player's coins
        ResourcesSaveData resourcesSaveData = SaveLoadManager.Load<ResourcesSaveData>();
        if (resourcesSaveData != null)
        {
            userCoins = new Coins(resourcesSaveData.Coins);
        }
        else
        {
            userCoins = new Coins(0);
        }

        damageData = SaveLoadManager.Load<DamageData>();

        if(damageData == null || damageData == default){
            damageData = new DamageData {
                DamageDealt = 10,
            };
        }
    }

    public void Save()
    {
        // Save the current state of prices and coins
        SaveLoadManager.Save(pricesData);

        resourcesSaveData.Coins = userCoins.Value;
        SaveLoadManager.Save(resourcesSaveData);
        SaveLoadManager.Save(damageData);
    }

    private void InitializeButtons()
    {
        // Set up each button in the store
        foreach (var item in BuyButton)
        {
            // Set the price for this item based on its type
            switch (item.type)
            {
                case UpgradeType.Damage:
                    item.prices = new Coins((int)pricesData.DamagePrice);
                    break;
                case UpgradeType.Health:
                    item.prices = new Coins((int)pricesData.HealthPrice);
                    break;
                case UpgradeType.Lives:
                    item.prices = new Coins((int)pricesData.LivesPrice);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Update the price preview text
            item.pricePreview.text = $"{item.prices.Value} Coins";

            // Add a listener to the button
            item.button.onClick.AddListener(() => BuyItem(item));
        }
    }

    public void BuyItem(StoreItem item)
    {
        // Check if the player has enough coins
        if (userCoins.Value >= item.prices.Value)
        {
            // Deduct the price from the player's coins
            userCoins.Reduce(item.prices.Value);
            coinPreview.text = userCoins.Value.ToString();

            // Increase the price by 1.4x for the next purchase
            float newPrice = item.prices.Value * 1.4f;

            // Update the price in PricesData
            switch (item.type)
            {
                case UpgradeType.Damage:
                    pricesData.DamagePrice = newPrice;
                    break;
                case UpgradeType.Health:
                    pricesData.HealthPrice = newPrice;
                    break;
                case UpgradeType.Lives:
                    pricesData.LivesPrice = newPrice;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Update the price preview text
            item.pricePreview.text = $"{(int)newPrice} Coins";

            // Apply the upgrade based on the item type
            ApplyUpgrade(item.type);

            // Save the updated state
            Save();

            Debug.Log($"Purchased {item.type} upgrade for {item.prices.Value} coins.");
        }
        else
        {
            Debug.Log("Not enough coins to purchase this item.");
        }
    }

    private void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                // Increase player damage
                damageData.DamageDealt *= 1.4f;
                break;
            case UpgradeType.Health:
                // Increase player health
                resourcesSaveData.Health *= 2f;
                break;
            case UpgradeType.Lives:
                // Increase player lives
                resourcesSaveData.Lives += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
    }
}

[Serializable]
public class StoreItem
{
    public Button button; // The button to purchase the item
    public TextMeshProUGUI pricePreview; // Text to display the price
    public Coins prices; // Current price of the item
    public UpgradeType type; // Type of upgrade (Damage, Health, Lives)
}
