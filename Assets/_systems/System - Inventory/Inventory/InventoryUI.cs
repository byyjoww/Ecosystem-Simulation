using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private IInventoryScriptableEvent OnInventoryChanged;
    [SerializeField] private GameObject pfInventoryElement;
    [SerializeField] private GameObject pfEmptySlotElement;
    [SerializeField] private Transform tInventoryElement;
    private List<GameObject> objectList = new List<GameObject>();    

    private bool isInitialized;

    public void OnEnable()
    {
        if (isInitialized)
        {
            return;
        }

        OnInventoryChanged.OnRaise += RefreshUI;
        OnInventoryChanged.RequestRaise();

        isInitialized = true;
    }

    private void OnDisable()
    {
        OnInventoryChanged.OnRaise -= RefreshUI;
        isInitialized = false;
    }

    public void RefreshUI(List<IInventoryElement> inventoryItems)
    {
        foreach (GameObject obj in objectList)
        {
            Destroy(obj);
        }

        objectList.Clear();

        foreach (var element in inventoryItems)
        {
            var obj = Instantiate(pfInventoryElement, tInventoryElement);
            obj.GetComponent<SetupInventoryElement>().Setup(element as IInventoryElement);
            objectList.Add(obj);
        }

        while (objectList.Count < 20)
        {
            var obj = Instantiate(pfEmptySlotElement, tInventoryElement);
            objectList.Add(obj);
        }

        while (objectList.Count % 4 != 0)
        {
            var obj = Instantiate(pfEmptySlotElement, tInventoryElement);
            objectList.Add(obj);
        }
    }    
}