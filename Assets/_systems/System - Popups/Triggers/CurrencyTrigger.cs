using UnityEngine;

public class CurrencyTrigger : BaseTrigger
{
    //Currency Trigger
    [Header("Currency Trigger")]
    public Currency currencyToGet;
    public int currencyQuantity;

    protected override void SetTrigger()
    {
        Debug.Log($"Constructor from Button Trigger assigned player to get.");
        currencyToGet.GetCurrency(currencyQuantity);
    }
}
