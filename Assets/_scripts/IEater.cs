using PluggableAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEater
{
    FoodChainGroup Eats { get; }
    int Hunger { get; }
    bool IsHungry { get; set; }
    void Eat(IEatable target);
    void IncreaseHunger();
    void RecoverHunger(int amount);
}
