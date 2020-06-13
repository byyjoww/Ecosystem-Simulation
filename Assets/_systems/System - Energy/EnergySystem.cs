using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy System", menuName = "Scriptable Object/Energy/Energy System", order = 1)]
public class EnergySystem : ScriptableObject, IInitializable
{
    [SerializeField] IntValue energy;
    public int CurrentEnergy => energy.Value;
    [SerializeField] IntValue maxEnergy;
    public int MaxEnergy => maxEnergy.Value;

    [SerializeField, Tooltip("Call the event every <timeTrigger> seconds.")]
    double timeTrigger;

    [SerializeField] protected ScriptableEvent eventTarget;
    [SerializeField] bool dontDestroy = false;
    [SerializeField] DateTimeValue dateTimeInitializer;

    public bool FullEnergy => energy.Value >= MaxEnergy;

    EventTimeTrigger recoverTrigger;

#pragma warning disable 0067
    public event Action OnEnergyRecovered;
    public event Action OnEnergyUsed;
#pragma warning restore 0067

    public void RecoverEnergy()
    {
        if (!recoverTrigger)
        {
            Init();
        }

        Debug.Log("RecoverEnergy Called");
        if (energy.Value < MaxEnergy)
        {
            Debug.Log("Recovered 1 Energy");
            energy.Value++;
        }

        if (FullEnergy)
        {
            Debug.Log("Energy Full | RecoverEnergy Paused");
            recoverTrigger.Pause();
        }

        OnEnergyRecovered?.Invoke();
    }

    public void UseEnergy()
    {
        TryUseEnergy();
    }

    public bool TryUseEnergy()
    {
        Debug.Log("UseEnergy Called");
        if (energy.Value <= 0)
        {
            Debug.Log("Out of Energy");
            return false;
        }

        Debug.Log("Energy Used");
        energy.Value--;

        if (energy.Value < MaxEnergy)
        {
            if (recoverTrigger.IsRunning)
            {
                if (recoverTrigger.IsPaused)
                {
                    Debug.Log("RecoverEnergy Resumed");
                    recoverTrigger.SetLastEvent(DateTime.UtcNow);
                    recoverTrigger.Resume();
                }
            }
            else
            {
                Debug.Log("RecoverEnergy ReStarted");
                recoverTrigger.SetLastEvent(DateTime.UtcNow);
                recoverTrigger.StartTimer();
            }
        }

        return true;
    }

    public void Init()
    {
        recoverTrigger = new GameObject("EnergyRecoverEventTrigger").AddComponent<EventTimeTrigger>();

        eventTarget.OnRaise -= RecoverEnergy;
        eventTarget.OnRaise += RecoverEnergy;

        recoverTrigger.StartTimer(timeTrigger, eventTarget, dateTimeInitializer, dontDestroy);
    }

    public bool Initialized => true;
}
