using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IEatable
{
    [SerializeField] private FoodChainGroup foodChain = FoodChainGroup.Plant;
    public FoodChainGroup FoodChainPosition => foodChain;

    [SerializeField] private int hungerRecovery = 10;
    public int HungerRecovery => hungerRecovery;

    public bool IsBeingEaten { get; set; }

    public void GetEaten()
    {
        Debug.Log("Plant is ded.");
        IsBeingEaten = true;
        Destroy(gameObject);
    }
}
