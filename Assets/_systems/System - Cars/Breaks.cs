using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Breaks
{
    [Header("Car Stats")]
    private float breakTorque;
    public float BreakTorque => breakTorque;

    [Header("Input Events")]
    private Action<bool> isBreak;

    [Header("Components")]
    private Axles[] axles;

    public Breaks(Axles[] axles, float breakTorque)
    {
        this.breakTorque = breakTorque;
        this.axles = axles;
        isBreak += Break;
    }

    ~Breaks()
    {
        isBreak -= Break;
    }

    public void Break(bool isBreak)
    {
        if (isBreak)
        {
            foreach (Axles axleInfo in axles)
            {
                if (axleInfo.hasBrakes)
                {
                    axleInfo.leftWheel.brakeTorque = breakTorque;
                    axleInfo.leftWheel.brakeTorque = breakTorque;
                }
            }
        }
        else
        {
            foreach (Axles axleInfo in axles)
            {
                if (axleInfo.hasBrakes)
                {
                    axleInfo.leftWheel.brakeTorque = 0f;
                    axleInfo.leftWheel.brakeTorque = 0f;
                }
            }
        }
    }
}