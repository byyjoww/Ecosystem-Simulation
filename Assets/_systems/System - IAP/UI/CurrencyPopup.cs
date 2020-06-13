using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyPopup : MonoBehaviour
{
    [System.Serializable]
    public class StorePackInfo
    {
        public StoreItemPack pack;
        public TMP_Text priceLabel;
        public TMP_Text quantityLabel;
    }

    [SerializeField] private StorePackInfo[] packList;

    private void OnEnable()
    {
        SetupText();

        if (!StoreSystem.IapAvailable)
        {
            StartCoroutine(UpdateButtonLabel());
        }
    }

    private IEnumerator UpdateButtonLabel()
    {
        while (!StoreSystem.IapAvailable)
        {
            yield return null;
        }

        SetupText();
    }

    protected void SetupText()
    {
        foreach (var item in packList)
        {
            if (item.pack == null)
            {
                item.priceLabel.text = "-";
                item.quantityLabel.text = "-";
            }
            else
            {
                item.priceLabel.text = (item.pack as HardCurrencyPack).GetItemPrice();
                if (item.pack is HardCurrencyPack)
                {
                    item.quantityLabel.text = (item.pack as HardCurrencyPack).GetAmount();
                }                
            }

        }
    }

    public void Buy(int id)
    {
        if(id < 0 || id >= packList.Length || packList[id].pack == null)
        {
            return;
        }

        StoreSystem.BuyItem(packList[id].pack);
    }
}
