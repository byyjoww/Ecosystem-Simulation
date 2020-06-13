using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManualPurchaseDetailsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(string name, string description, Sprite sprite, int price, Action action)
    {
        nameTextComponent.text = name;
        descriptionTextComponent.text = description;
        imageComponent.sprite = sprite;
        buttonComponent.onClick.AddListener(delegate { ButtonAction(action); });
    }

    public static GameObject Create(Transform transform, string name, string description, Sprite sprite, int price, Action action)
    {
        GameObject obj = Instantiate(GameAssets.Instance.pfManualPurchaseDetailsPanel, transform);

        ShopManualPurchaseDetailsPanel element = obj.GetComponent<ShopManualPurchaseDetailsPanel>();
        element.Setup(name, description, sprite, price, action);

        return obj;
    }

    public void ButtonAction(Action action)
    {
        action?.Invoke();
    }
}