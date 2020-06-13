using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupInventoryElement : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text countTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;
    [SerializeField] GameObject pfDetailsPanel;

    public void Setup(IInventoryElement inventoryObject)
    {
        nameTextComponent.text = inventoryObject.ItemName;
        countTextComponent.text = inventoryObject.ItemQuantity.ToString();
        imageComponent.sprite = inventoryObject.ItemSprite;
        buttonComponent.onClick.AddListener(delegate { ButtonAction(inventoryObject); });
    }

    public void ButtonAction(IInventoryElement inventoryObject)
    {
        var obj = Instantiate(pfDetailsPanel, GameAssets.Instance.transform);
        //obj.GetComponent<SetupDetailsPanel>().Setup(inventoryObject);
    }
}
