using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopItemLayoutElement : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text priceTextComponent;
    [SerializeField] TMP_Text rarityTextComponent;
    [SerializeField] TMP_Text typeTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(Item item, int price, Action action)
    {
        nameTextComponent.text = item.ItemName;
        priceTextComponent.text = price.ToString();

        rarityTextComponent.color = item.ItemRarity.RarityTextColor;
        rarityTextComponent.text = item.ItemRarity.itemRarityLevel.ToString();

        typeTextComponent.color = item.ItemType.TypeTextColor;
        typeTextComponent.text = item.ItemType.itemType.ToString();

        imageComponent.sprite = item.ItemSprite;
        buttonComponent.onClick.AddListener(delegate { ButtonAction(item, price, action); });
    }

    public static GameObject Create(Transform transform, Item item, int price, Action action)
    {
        GameObject obj = Instantiate(GameAssets.Instance.pfShopItemLayoutElement, transform);

        ShopItemLayoutElement element = obj.GetComponent<ShopItemLayoutElement>();
        element.Setup(item, price, action);

        return obj;
    }

    public void ButtonAction(Item item, int price, Action action)
    {
        ShopItemPurchaseDetailsPanel.Create(GameAssets.Instance.transform, item.ItemName, item.ItemDescription, item.ItemSprite, price, action);
    }
}