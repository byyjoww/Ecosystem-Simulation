using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupDetailsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(IInventoryElement inventoryObject)
    {
        nameTextComponent.text = inventoryObject.ItemName;
        descriptionTextComponent.text = inventoryObject.ItemQuantity.ToString();
        imageComponent.sprite = inventoryObject.ItemSprite;
        buttonComponent.onClick.AddListener(delegate { ButtonAction(inventoryObject); });
    }

    public void ButtonAction(IInventoryElement inventoryObject)
    {
        //INSERT ACTION
    }
}