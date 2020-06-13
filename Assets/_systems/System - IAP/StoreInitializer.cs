using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInitializer : MonoBehaviour
{
    [SerializeField] StoreSystem storeSystem;

    void Awake()
    {
        DontDestroyOnLoad(this);
        storeSystem.InitializeStore();
    }
}
