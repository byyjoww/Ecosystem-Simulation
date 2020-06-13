using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Purchase : ScriptableObject
{
    [SerializeField] protected int purchasePrice;
    [SerializeField] protected Currency currency;
    public ScriptableEvent OnPurchaseFailed;
    public ScriptableEvent OnPurchaseSucceeded;    

    protected void Buy()
    {
        if (PurchaseCondition())
        {
            if (currency.SpendCurrency(purchasePrice))
            {
                Debug.Log("Purchase succeeded.");
                OnPurchaseSucceeded.Raise();
                PurchaseSuccessful();
            }
            else
            {
                Debug.Log("Purchase failed, not enough currency.");
                OnPurchaseFailed.Raise();
            }
        }
        else
        {
            Debug.Log(ConditionMessage);
            OnPurchaseFailed.Raise();
        }
    }

    protected virtual bool PurchaseCondition()
    {
        return true;
    }
    protected virtual string ConditionMessage { get { return "Puurchase Failed. Didn't meet the required conditions"; } }

    protected abstract void PurchaseSuccessful();
    public abstract GameObject CreateUIElement(Transform tr);
}
