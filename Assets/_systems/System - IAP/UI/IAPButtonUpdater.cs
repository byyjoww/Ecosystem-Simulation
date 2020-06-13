using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IAPButtonUpdater : MonoBehaviour
{
    [SerializeField] StoreItemPack pack;
    [SerializeField] Text buttonLabel;

    void Awake()
    {
        StartCoroutine(UpdateButtonLabel());
    }

    IEnumerator UpdateButtonLabel()
    {
        while (!StoreSystem.IapAvailable)
        {
            yield return null;
        }
            
        buttonLabel.text = pack.GetItemPrice();
    }

    public void Buy()
    {
        StoreSystem.BuyItem(pack);
    }
}
