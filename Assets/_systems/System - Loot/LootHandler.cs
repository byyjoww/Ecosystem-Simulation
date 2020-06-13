using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    [Header("Currency Rewards")]
    [SerializeField] List<CurrencyLootData> currencyData = new List<CurrencyLootData>();

    [Header("Items")]
    [SerializeField] Inventory inventory;
    [SerializeField, RequireInterface(typeof(IInventoryElement))] private List<ScriptableObject> lootItems = new List<ScriptableObject>();

    public event Action<List<int>, List<IInventoryElement>> OnDeliverLoot;

    public void CalculateLoot()
    {
        DeliverLoot(GetCurrencies(), GetLoot());
    }

    private void DeliverLoot(List<CurrencyLootData> currencies, List<IInventoryElement> items)
    {
        List<int> currencyAmounts = new List<int>();

        if (currencies != null && currencies.Count > 0)
        {
            foreach (var data in currencies)
            {
                int amount = (int)data.FinalCurrencyAmount;
                data.currency.GetCurrency(amount);
                currencyAmounts.Add(amount);
            }
        }

        if(items != null && items.Count > 0)
        {
            foreach (var item in items)
            {
                this.inventory.GainItem(item, item.ItemQuantity);
            }
        }      

        OnDeliverLoot?.Invoke(currencyAmounts, items);
    }

    private List<CurrencyLootData> GetCurrencies()
    {
        return currencyData;
    }

    private List<IInventoryElement> GetLoot()
    {
        return lootItems.Cast<IInventoryElement>().ToList();
    }

    [System.Serializable]
    public class CurrencyLootData
    {
        [SerializeField] public Currency currency;
        [SerializeField] public int baseCurrencyToGet;
        [SerializeField, Range(0, 100)] public int currencyVariancePercent;
        public double FinalCurrencyAmount => baseCurrencyToGet * RandomVariance(currencyVariancePercent);

        private double RandomVariance(int maxVariancePercent)
        {
            double maxVariance = (double)maxVariancePercent / 100;
            double randomNumber = UnityEngine.Random.Range((float)-maxVariance, (float)maxVariance);
            double random = 1 - Math.Round(randomNumber, 2);
            //Debug.Log($"maxVariancePercent: {maxVariancePercent}");
            //Debug.Log($"maxVariance: {maxVariance}");
            //Debug.Log($"randomNumber: {randomNumber}");
            //Debug.Log($"Random Variance: {random}");
            return random;
        }
    }
}
