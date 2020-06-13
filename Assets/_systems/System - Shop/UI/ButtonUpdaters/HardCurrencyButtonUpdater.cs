using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class HardCurrencyButtonUpdater : CurrencyButtonUpdater
{
    public override void Buy()
    {
        //Create Popup
    }

    public void CompletePurchase()
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
