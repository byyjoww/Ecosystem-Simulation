using UnityEngine;

[CreateAssetMenu(fileName = "new Item Purchase", menuName = "Scriptable Object/Shop/Item Purchase")]
public class ItemPurchase : Purchase
{
    [RequireInterface(typeof(IInventoryElement))]
    public Item item;
    public Inventory itemInventory;
    public int quantity = 1;

    protected override void PurchaseSuccessful()
    {
        itemInventory.GainItem(item as IInventoryElement, quantity);
    }

    public override GameObject CreateUIElement(Transform tr)
    {
        return ShopItemLayoutElement.Create(tr, item, purchasePrice, Buy);
    }
}
