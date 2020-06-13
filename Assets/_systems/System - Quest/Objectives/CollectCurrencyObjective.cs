using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectCurrencyObjective : QuestObjective
{
    [SerializeField] private IntScriptableEvent OnCurrencyChanged;
    public Currency requiredCurrency;
    public int requiredQuantity;

    private void CheckRequiredItems(int currencyAmount)
    {
        if (currencyAmount >= requiredQuantity)
        {
            CompleteObjective();
        }
    }

    public override void CheckObjective()
    {
        OnCurrencyChanged.RequestRaise();
    }

    public void OnEnable()
    {
        OnCurrencyChanged.OnRaise += CheckRequiredItems;        
        // Debug.Log("CheckRequiredItems subscribed to OnInventoryChanged.");        
    }

    public void OnDisable()
    {
        OnCurrencyChanged.OnRaise -= CheckRequiredItems;
        // Debug.Log("CheckRequiredItems unsubscribed from OnInventoryChanged.");
    }
}