using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class SoftCurrencyButtonUpdater : CurrencyButtonUpdater
{
    public override void Buy()
    {
        if (currency.SpendCurrency(currencyPrice))
        {
            if (OnPurchaseSucceeded != null)
            {
                OnPurchaseSucceeded.Invoke();
            }
        }
        else
        {
            if (OnPurchaseFailed != null)
            {
                OnPurchaseFailed.Invoke();
            }
        }
    }
}
