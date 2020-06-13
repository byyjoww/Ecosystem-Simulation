using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Health : MonoBehaviour, IFillable
{
    [Header("Health Change Events")]
    public UnityEvent OnHealthEmpty;
    public event Action<int, int> OnTakeDamage, OnHeal, OnChanged;

    #region INTERFACE
    public float CurrentFill => Current;
    public float MaxFill => Max;

    public event Action OnFillValueChanged;
    #endregion

    [SerializeField] IntValue max;
    public int Max { get { return max.Value; } }
    public int Current { get; private set; }

    public void Start()
    {
        Current = Max;
        OnChanged += (a, b) => OnFillValueChanged();
        OnFillValueChanged?.Invoke();
        Debug.Log($"Health initialized and set to {Max}.");
    }

    public void TakeDamage(int amount)
    {
        int prev = Current;
        Current = Mathf.Clamp(Current - amount, 0, Max);
        Debug.Log($"Takes {amount} damage | Before: {prev} | After: {Current}.");

        OnTakeDamage?.Invoke(prev, Current);
        OnChanged?.Invoke(prev, Current);

        if (Current <= 0)
        {
            Current = 0;
            OnHealthEmpty?.Invoke();
            Debug.Log($"Health Empty = Dead.");
        }
    }

    public void Heal(int amount)
    {
        int prev = Current;
        Current = Mathf.Clamp(Current + amount, 0, Max);
        Debug.Log($"Heals {amount} health | Before: {prev} | After: {Current}.");

        OnHeal?.Invoke(prev, Current);
        OnChanged?.Invoke(prev, Current);
    }

    public void Fill(bool invokeEvents = true)
    {
        int prev = Current;
        Current = Max;
        Debug.Log($"Heals {Current - prev} health | Before: {prev} | After: {Current}.");

        OnHeal?.Invoke(prev, Current);
        OnChanged?.Invoke(prev, Current);
    }
}