using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new Currency", menuName = "Scriptable Object/Item/Currency")]
public class Currency : ScriptableObject
{
    public string currencyName;
    [SerializeField] private SavableIntValue currencyAmount;
    public SavableIntValue CurrencyAmount => currencyAmount;
    public IntScriptableEvent OnCurrencyChange;

    private void OnEnable()
    {
        // Debug.Log($"{this.name} was Enabled");
        OnCurrencyChange.OnRequestList += RaiseOnCurrencyChanged;
    }
    private void OnDisable()
    {
        // Debug.Log($"{this.name} was Disable");
        OnCurrencyChange.OnRequestList -= RaiseOnCurrencyChanged;
    }

    private void RaiseOnCurrencyChanged()
    {
        OnCurrencyChange.Raise(currencyAmount.Value);
    }

    public bool HaveEnoughCurrency(int amount)
    {
        return amount <= currencyAmount.Value;
    }

    public void GetCurrency(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        currencyAmount.Value += amount;
        RaiseOnCurrencyChanged();
        Debug.Log($"Player gained {amount} {currencyName}.");
    }

    public bool SpendCurrency(int amount)
    {
        if (!HaveEnoughCurrency(amount))
        {
            Debug.Log($"Player has insufficient {currencyName}.");
            return false;
        }

        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        currencyAmount.Value -= amount;
        RaiseOnCurrencyChanged();
        Debug.Log($"Player spent {amount} {currencyName}.");

        return true;
    }
}