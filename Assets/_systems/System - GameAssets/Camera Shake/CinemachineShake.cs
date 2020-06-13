using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineShake : Singleton<CinemachineShake>
{
    [SerializeField] private bool lerp = false;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer = 0f;

    private void Start()
    {
        if (cinemachineVirtualCamera == null) cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float intensity, float time, bool lerp)
    {
        if (lerp)
        {
            SetAmplitude(intensity);
            Lerp(intensity, 0, time);
        }
        else
        {
            SetAmplitude(intensity);
            shakeTimer = time;
        }        
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                shakeTimer = 0f;
                SetAmplitude(0);
            }
        }
    }

    private void SetAmplitude(float intensity)
    {
        var perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
    }

    IEnumerator Lerp(float amountFrom, float amountTo, float timeToComplete)
    {
        float timeRemaining = timeToComplete;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            var lerp = Mathf.Lerp(amountFrom, amountTo, Mathf.InverseLerp(timeToComplete, 0, timeRemaining));
            SetAmplitude(Mathf.RoundToInt(lerp));

            yield return null;
        }

        SetAmplitude(Mathf.RoundToInt(amountTo));
    }

    private void OnValidate()
    {
        if (cinemachineVirtualCamera == null) cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
