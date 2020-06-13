using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axles
{
    [Header("Settings")]
    public bool motor;
    public bool steering;
    public bool hasBrakes;

    [Header("References")]
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
}