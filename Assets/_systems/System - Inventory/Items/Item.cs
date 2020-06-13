using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Scriptable Object/Item/Item")]
public class Item : ScriptableElement, IInventoryElement
{
    public override Database.DatabaseType EnumIdentifier { get => Database.DatabaseType.Item; }

    public string ItemName => itemName;
    public Sprite ItemSprite => itemSprite;
    public int ItemQuantity { get => itemQuantity; set => itemQuantity = value; }
    public string ItemDescription => itemDescription;
    public ItemRarity ItemRarity => itemRarity;
    public ItemType ItemType => itemType;

    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private int itemQuantity;
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private ItemType itemType;
}
