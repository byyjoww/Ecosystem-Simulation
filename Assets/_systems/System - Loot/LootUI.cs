using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(LootHandler))]
public class LootUI : MonoBehaviour
{
    private LootHandler lootHandler;

    [Header("Currencies")]
    [SerializeField] private List<TMP_Text> CurrencyTextComponents;

    [Header("Items")]
    [SerializeField] private GameObject pfItemIcon;
    [SerializeField] private Transform tItemIcon;

    private void Start()
    {
        lootHandler = GetComponent<LootHandler>();
        lootHandler.OnDeliverLoot += EndOfBattleRewards;
    }

    private void EndOfBattleRewards(List<int> currencyData, List<IInventoryElement> items)
    {
        if(CurrencyTextComponents != null && CurrencyTextComponents.Count > 0)
        {
            for (int i = 0; i < CurrencyTextComponents.Count; i++)
            {
                CurrencyTextComponents[i].text = $"+{currencyData[i]}";
            }
        }        
        
        GenerateItemRewardIcons(items);
    }

    private void GenerateItemRewardIcons(List<IInventoryElement> items)
    {
        foreach (var item in items)
        {
            Debug.Log($"Reward: {item}.");
            //GameObject icon = Instantiate(pfItemIcon, tItemIcon);
            //icon.GetComponent<SetupLootElement>().Setup(item);
        }
    }
}