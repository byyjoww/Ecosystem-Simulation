using UnityEngine;

[System.Serializable]
public abstract class StoreItemPack : StoreItemData, IBuyable
{
    //Override this if there is a condition for this Item to be purchased
    public virtual bool CanBuy()
    {
        return true;
    }

    public abstract void OnBuyResult(bool success);

    public virtual string SKU => ItemSKU;

    public virtual int USDPrice => usdPrice;
}

public static class StoreItemPackHelpers
{
    public static string GetItemPrice(this StoreItemPack itemPack)
    {
#if UNITY_IOS
        var price = Iap.GetItemPrice(itemPack.SKU);
        return string.IsNullOrEmpty(price) ? "USD " + (itemPack.USDPrice - 0.01f) : price;
#else
        return "$" + (itemPack.USDPrice - 0.01f);
#endif
    }
}
