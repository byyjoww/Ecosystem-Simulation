using UnityEngine;

public class ItemTrigger : BaseTrigger
{
    //Item Trigger
    [Header("Item Trigger")]
    public Inventory inventory;
    [RequireInterface(typeof(IInventoryElement))] public ScriptableObject itemToGet;
    public int itemQuantity;

    protected override void SetTrigger()
    {
        Debug.Log($"Constructor from Button Trigger assigned player to get.");
        IInventoryElement item = itemToGet as IInventoryElement;
        inventory.GainItem(item, itemQuantity);
    }
}
