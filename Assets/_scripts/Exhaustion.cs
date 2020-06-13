using PluggableAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Exhaustion : MonoBehaviour, IFillable
{
    NavMeshAgent agent;

    [Header("Exhaustion Status")]
    [SerializeField, ReadOnly] private int maxExhaustionLevel = 100;
    private int exhaustionLevel { get; set; }

    [Header("Parameters")]
    [SerializeField] private float exhaustionCooldownCycle = 1f;
    private float exhaustionCooldown { get; set; }

    [SerializeField] private float recoveryCooldownCycle = 1f;
    private float recoveryCooldown { get; set; }

    public bool IsExhausted { get; set; }
    public System.Action OnExhaustionChange;

    // FILLABLE INTERFACE
    public float CurrentFill => exhaustionLevel;
    public float MaxFill => maxExhaustionLevel;

    public event System.Action OnFillValueChanged;

    // --------------------

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        exhaustionCooldown = exhaustionCooldownCycle;
        recoveryCooldown = recoveryCooldownCycle;
        exhaustionLevel = 0;
    }

    private void Update()
    {
        TimedUpdateExhaustion(1);
        CheckExhaustionStatus();
    }

    private void CheckExhaustionStatus()
    {
        if (exhaustionLevel >= maxExhaustionLevel)
        {
            IsExhausted = true;
        }
        else
        {
            IsExhausted = false;
        }

        OnExhaustionChange?.Invoke();
    }

    // -------- Natural Exhaustion Gain --------
    private void TimedUpdateExhaustion(int amount)
    {
        exhaustionCooldown -= agent.velocity.magnitude * Time.deltaTime;
        if (exhaustionCooldown <= 0)
        {
            exhaustionCooldown += exhaustionCooldownCycle;
            exhaustionLevel += amount;

            if (exhaustionLevel >= maxExhaustionLevel)
            {
                exhaustionLevel = maxExhaustionLevel;
            }

            OnFillValueChanged?.Invoke();
        }
    }

    // ----------- Gain Exhaustion -----------
    public void GainExhaustion(int amount, bool lerp)
    {
        UpdateExhaustion(amount, lerp);
        CheckExhaustionStatus();
    }

    private void UpdateExhaustion(int amount, bool lerp)
    {
        if (lerp)
        {
            var to = exhaustionLevel + (float)amount;

            if (to >= maxExhaustionLevel)
            {
                to = maxExhaustionLevel;
            }

            StartCoroutine(Lerp(exhaustionLevel, to, 5f));
        }
        else
        {
            exhaustionLevel += amount;

            if (exhaustionLevel >= maxExhaustionLevel)
            {
                exhaustionLevel = maxExhaustionLevel;
            }
        }       

        OnFillValueChanged?.Invoke();
    }

    // ----------- Lose Exhaustion -----------
    public void LoseExhaustion(int amount, bool lerp)
    {
        UpdateRecovery(amount, lerp);
        CheckExhaustionStatus();
    }

    private void UpdateRecovery(int amount, bool lerp)
    {
        if (lerp)
        {
            var to = exhaustionLevel - (float)amount;

            if (to <= 0)
            {
                to = 0;
            }

            StartCoroutine(Lerp(exhaustionLevel, to, 5f));
        }
        else
        {
            exhaustionLevel -= amount;
        }        

        if (exhaustionLevel <= 0)
        {
            exhaustionLevel = 0;
        }

        OnFillValueChanged?.Invoke();
    }

    // ----------- Lerp Coroutine -----------

    IEnumerator Lerp(float amountFrom, float amountTo, float timeToComplete)
    {
        //Debug.LogError($"AmountFrom: {amountFrom} | AmountTo: {amountTo} | timeToComplete: {timeToComplete}");

        float timeRemaining = timeToComplete;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            var lerp = Mathf.Lerp(amountFrom, amountTo, Mathf.InverseLerp(timeToComplete, 0, timeRemaining));
            exhaustionLevel = Mathf.RoundToInt(lerp);

            OnFillValueChanged?.Invoke();

            //Debug.Log($"timeRemaining {timeRemaining}");

            yield return null;
        }

        exhaustionLevel = Mathf.RoundToInt(amountTo);
        OnFillValueChanged?.Invoke();
    }

}
