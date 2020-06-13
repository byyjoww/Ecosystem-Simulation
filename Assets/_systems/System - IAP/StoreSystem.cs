using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "StoreSystem", menuName = "Scriptable Object/IAP/Store System", order = 1)]
public class StoreSystem : ScriptableObject, IInitializable
{
    //    [SerializeField] IAPStoreItems storeItems;
    //    [SerializeField] IntValue hardCurrency; //NEED TO ADD

    //    public static Action<Iap.Transaction> OnIapTransactionReceived;  //NEED TO ADD

    public static bool IapAvailable { get; private set; }

    static bool isInitialized = false;

    //    #region IAPs

    //#if UNITY_EDITOR
    //    static Action<Iap.Transaction> FakeOnPurchaseEvent; //NEED TO ADD
    //#endif

    public void InitializeStore()
    {
        //if (isInitialized) return;
        //InitializeIap();
        //isInitialized = true;
    }

    public static void BuyItem(StoreItemPack itemPack, string placement = "Default")
    {
        //if (!isInitialized)
        //{
        //    throw new Exception("Trying to call BuyItem without initializing the StoreSystem!");
        //}

        //#if UNITY_IOS && !UNITY_EDITOR
        //        Iap.Buy(itemPack.SKU, placement);
        //#elif UNITY_EDITOR

        //        var t = new Iap.Transaction();
        //        t.Success = true;
        //        t.IsSubscription = false;
        //        t.ProductSku = itemPack.SKU;
        //        FakeOnPurchaseEvent(t);
        //#endif
        //    }

        //    public static void RestorePurchase()
        //    {
        //#if UNITY_IOS
        //        Iap.UpdateItems();
        //#endif
        //    }

        //    void OnPurchaseEvent(Iap.Transaction transaction)
        //    {
        //        Debug.Log("SM-IAP: PurchaseEvent " + transaction.Success);
        //        if (!transaction.Success)
        //        {
        //            Debug.LogError(transaction.Error + " | Message: " + transaction.ErrorMessage);
        //        }

        //        var pack = storeItems.itemsList.FirstOrDefault(i =>
        //            string.Compare(i.SKU, transaction.ProductSku) == 0);

        //        pack.OnBuyResult(transaction.Success);

        //        if (OnIapTransactionReceived != null)
        //        {
        //            OnIapTransactionReceived(transaction);
        //        }
    }

    //    void InitializeIap()
    //    {
    //        Iap.InitializedEvent += iapEvent =>
    //        {
    //            Debug.Log("SM-IAP: InitializedEvent " + iapEvent.Success);
    //            IapAvailable = iapEvent.Success;
    //        };

    //        Iap.ItemUpdateEvent += iapEvent =>
    //        {
    //            Debug.Log("SM-IAP: ItemUpdateEvent " + iapEvent.Success);
    //            if (iapEvent.Success)
    //            {

    //            }
    //        };

    //        Iap.SubscriptionUpdateEvent += iapEvent =>
    //        {
    //            Debug.Log("SM-IAP: SubscriptionUpdateEvent " + iapEvent.IsActive);
    //        };

    //        Iap.PurchaseEvent += OnPurchaseEvent;

    //#if UNITY_EDITOR
    //        FakeOnPurchaseEvent = OnPurchaseEvent;
    //#endif

    //#if UNITY_ANDROID
    //            Iap.ConsumeEvent += productEvent => Debug.Log("SM-IAP: Consume " + productEvent.ProductSku);
    //#endif

    //        // Initializing IAP to pre-fetch item data
    //        var ids = storeItems.itemsList.Select(i => i.SKU).ToList();
    //        Iap.Initialize(ids, null, null);
    //    }

    //    #endregion

    public void Init()
    {
        InitializeStore();
    }

    public bool Initialized => isInitialized;
}