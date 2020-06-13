using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Clock))]
public class ClockUI : MonoBehaviour
{
    [SerializeField] Clock timer;    
    [SerializeField] TMP_Text textComponent;
    private bool isInitialized = false;

    private void OnEnable()
    {
        StartCoroutine(Initialize());
    }

    private void OnDisable()
    {
        timer.OnTimerTick -= (currentTime) => RefreshUI(currentTime);
    }

    private IEnumerator Initialize()
    {
        while (!timer.IsInitialized)
        {
            yield return new WaitUntil(() => timer.IsInitialized);
        }

        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
        timer.OnTimerTick += (currentTime) => RefreshUI(currentTime);

        isInitialized = true;
    }

    private void RefreshUI(float currentTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        textComponent.text = $"{time.ToString("mm':'ss")}";
    }

    private void OnValidate()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
    }
}
