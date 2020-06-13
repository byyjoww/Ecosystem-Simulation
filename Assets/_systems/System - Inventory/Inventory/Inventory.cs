using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "new Inventory", menuName = "Scriptable Object/Item/Inventory")]
public class Inventory : ScriptableObject
{
    [RequireInterface(typeof(IInventoryElement))]
    [SerializeField] private List<ScriptableObject> allInventoryItems = new List<ScriptableObject>();
    public List<ScriptableObject> AllInventoryItems => allInventoryItems;
    public IInventoryScriptableEvent OnInventoryChanged;

    private void OnEnable()
    {
        // Debug.Log($"{this.name} was Enabled");
        OnInventoryChanged.OnRequestList += RaiseOnInventoryChanged;
    }

    private void OnDisable()
    {
        // Debug.Log($"{this.name} was Disable");
        OnInventoryChanged.OnRequestList -= RaiseOnInventoryChanged;
    }

    private void RaiseOnInventoryChanged()
    {
        allInventoryItems.RemoveAll(x => x == null);

        Debug.Log($"{OnInventoryChanged.name} was Raised");

        //foreach (var item in allInventoryItems.Cast<IInventoryElement>().ToList())
        //{
        //    Debug.Log($"Element {allInventoryItems.Cast<IInventoryElement>().ToList().IndexOf(item)}: {item.ItemName}");
        //}

        OnInventoryChanged.Raise(allInventoryItems.Cast<IInventoryElement>().ToList());
    }

    public int CheckItemAmount(IInventoryElement item)
    {
        int amount = 0;

        foreach (var obj in allInventoryItems.Where(x => x == (UnityEngine.Object)item))
        {
            amount += item.ItemQuantity;
        }

        return amount;
    }

    public void GainItem(IInventoryElement item, int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        foreach (var obj in allInventoryItems.Where(x => x == (UnityEngine.Object)item))
        {
            item.ItemQuantity += amount;
        }

        RaiseOnInventoryChanged();
        Debug.Log($"Player gained x{amount} {item.ItemName}.");
    }

    public bool LoseItem(IInventoryElement item, int amount)
    {
        if (CheckItemAmount(item) < amount)
        {
            Debug.Log($"Player has insufficient {item.ItemName}.");
            return false;
        }

        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        foreach (var obj in allInventoryItems.Where(x => x == (UnityEngine.Object)item))
        {
            item.ItemQuantity -= amount;
            if(item.ItemQuantity < 0)
            {
                item.ItemQuantity = 0;
                Debug.LogError($"{item} amount just went below 0, after having {amount} units removed.");
            }
        }

        RaiseOnInventoryChanged();
        Debug.Log($"Player lost x{amount} {item.ItemName}.");
        return true;
    }

    private void OnValidate()
    {
        allInventoryItems = Tools.GetAllScriptableObjects<ScriptableObject>().Where(x => x is IInventoryElement).ToList();
        // Debug.Log("Validated Inventory");
    }
}
