using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float maxTime;
    public float currentTime { get; set; }

    private bool isInitialized = false;
    public bool IsInitialized => isInitialized;

    public event Action<float> OnTimerTick;
    public event Action OnTimerEnd;

    private void Start()
    {
        
    }

    private void Initialize(float maxTime)
    {
        if (isInitialized)
        {
            return;
        }

        this.maxTime = maxTime;
        this.currentTime = maxTime;
        isInitialized = true;
    }

    private void Tick()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            OnTimerEnd?.Invoke();
        }

        OnTimerTick?.Invoke(currentTime);
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        Tick();
    }
}