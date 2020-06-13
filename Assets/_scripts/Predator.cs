using PluggableAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Predator : MonoBehaviour, IEatable, IEater, IFillable
{
    [SerializeField] FoodChainGroup foodChainPosition = FoodChainGroup.Herbivore;
    public FoodChainGroup FoodChainPosition => foodChainPosition;

    [SerializeField] FoodChainGroup eats = FoodChainGroup.Plant;
    public FoodChainGroup Eats => eats;

    [SerializeField] private int hungerRecovery = 10;
    public int HungerRecovery => hungerRecovery;

    [SerializeField] int hunger = 0;
    public int Hunger => hunger;

    [SerializeField] int hungerThreshold = 15;

    public bool IsHungry { get; set; }

    public bool IsBeingEaten { get; set; }

    // FILL INTERFACE
    public float CurrentFill => hunger;

    public float MaxFill => 100;

    public event System.Action OnFillValueChanged;

    // ---------------

    [SerializeField] private float hungerCooldownPeriod = 10f;
    private float hungerCooldown;

    private void Start()
    {
        hungerCooldown = hungerCooldownPeriod;
        IsHungry = false;
    }

    private void Update()
    {
        hungerCooldown -= Time.deltaTime;

        if (hungerCooldown <= 0)
        {
            hungerCooldown = hungerCooldownPeriod;
            IncreaseHunger();
        }

        if (hunger > hungerThreshold)
        {
            IsHungry = true;
        }
        else
        {
            IsHungry = false;
        }
    }

    public void Eat(IEatable target)
    {
        RecoverHunger(target.HungerRecovery);
        target.GetEaten();
    }

    public void GetEaten()
    {
        IsBeingEaten = true;
        Destroy(this.gameObject);
    }

    public void RecoverHunger(int amount)
    {
        hunger -= amount;

        if (hunger <= 0)
        {
            hunger = 0;
        }

        OnFillValueChanged?.Invoke();
    }

    public void IncreaseHunger()
    {
        hunger += 1;

        if (hunger >= 100)
        {
            hunger = 100;
            
        }

        OnFillValueChanged?.Invoke();

        if (hunger == 100)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject} is dead.");
        Destroy(gameObject);
    }
}
