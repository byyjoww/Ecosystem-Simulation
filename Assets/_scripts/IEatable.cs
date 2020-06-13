using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodChainGroup { Plant = 0, Herbivore = 1, Carnivore = 2 }

public interface IEatable
{
    FoodChainGroup FoodChainPosition { get; }
    void GetEaten();
    bool IsBeingEaten { get; set; }
    int HungerRecovery { get; }
}
