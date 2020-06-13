using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectItemObjective : QuestObjective
{
    [SerializeField] private IInventoryScriptableEvent OnInventoryChanged;
    [RequireInterface(typeof(IInventoryElement))] public ScriptableObject requiredItem;
    public IInventoryElement RequiredItem => requiredItem as IInventoryElement;
    public int requiredQuantity;

    private void CheckRequiredItems(List<IInventoryElement> inventoryElements)
    {
        IInventoryElement itemDataElement = inventoryElements.Single(x => x == RequiredItem);
        if (itemDataElement != null && itemDataElement.ItemQuantity >= requiredQuantity)
        {
            CompleteObjective();
        }
    }

    public override void CheckObjective()
    {
        OnInventoryChanged.RequestRaise();

    }

    public void OnEnable()
    {        
        OnInventoryChanged.OnRaise += CheckRequiredItems;        
        // Debug.Log("CheckRequiredItems subscribed to OnInventoryChanged.");        
    }

    public void OnDisable()
    {        
        OnInventoryChanged.OnRaise -= CheckRequiredItems;
        // Debug.Log("CheckRequiredItems unsubscribed from OnInventoryChanged.");
    }
}