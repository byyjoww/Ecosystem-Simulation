using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManualLayoutElement : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] TMP_Text priceTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(string name, string description, Sprite sprite, int price, Action action)
    {
        nameTextComponent.text = name;
        descriptionTextComponent.text = description;
        priceTextComponent.text = price.ToString();
        imageComponent.sprite = sprite;
        buttonComponent.onClick.AddListener(delegate { ButtonAction(name, description, sprite, price, action); });
    }

    public static GameObject Create(Transform transform, string name, string description, Sprite sprite, int price, Action action)
    {
        GameObject obj = Instantiate(GameAssets.Instance.pfShopManualLayoutElement, transform);

        ShopManualLayoutElement element = obj.GetComponent<ShopManualLayoutElement>();
        element.Setup(name, description, sprite, price, action);

        return obj;
    }

    public void ButtonAction(string name, string description, Sprite sprite, int price, Action action)
    {
        ShopManualPurchaseDetailsPanel.Create(GameAssets.Instance.transform, name, description, sprite, price, action);
    }
}