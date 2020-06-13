using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManualPurchase : Purchase
{
    public string purchaseName;
    public string purchaseDescription;
    public Sprite purchaseSprite;

    public override GameObject CreateUIElement(Transform tr)
    {
        return ShopManualLayoutElement.Create(tr, purchaseName, purchaseDescription, purchaseSprite, purchasePrice, Buy);
    }
}
