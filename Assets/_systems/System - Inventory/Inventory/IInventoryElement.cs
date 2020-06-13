using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryElement
{
    string ItemName { get; }
    Sprite ItemSprite { get; }
    int ItemQuantity { get; set; }
}
