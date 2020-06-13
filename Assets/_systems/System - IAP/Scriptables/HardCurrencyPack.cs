using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HardCurrencyPack", menuName = "Scriptable Object/IAP/Hard Currency Pack", order = 1)]
public class HardCurrencyPack : StoreItemPack
{
    [SerializeField] int amount;
    [SerializeField] IntValue hardCurrency;

    public string GetAmount()
    {
        return amount.ToString();
    }


    public override void OnBuyResult(bool success)
    {
        Debug.Log("HardCurrencyPack IAP Result: " + success);

        if (!success)
        {
            return;
        }

        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        hardCurrency.Value += amount;
    }
}
