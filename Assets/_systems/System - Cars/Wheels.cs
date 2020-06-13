using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheels
{
    [Header("Car Stats")]
    private float maxSteeringAngle;
    public float MaxSteeringAngle => maxSteeringAngle;

    [Header("Input Events")]
    private Action<float> horizontal;

    [Header("Components")]
    private Rigidbody rigidbody;
    private Axles[] axles;

    public Wheels(Rigidbody rigidbody, Axles[] axles, float maxSteeringAngle)
    {
        this.maxSteeringAngle = maxSteeringAngle;
        this.rigidbody = rigidbody;
        this.axles = axles;
        horizontal += MoveHorizontal;
    }

    ~Wheels()
    {
        horizontal -= MoveHorizontal;
    }

    public void MoveHorizontal(float input)
    {
        foreach (Axles axleInfo in axles)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = maxSteeringAngle * input;
                axleInfo.rightWheel.steerAngle = maxSteeringAngle * input;
            }
        }
    }

    public void MoveHorizontal(float angle, bool idk)
    {
        foreach (Axles axleInfo in axles)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = angle;
                axleInfo.rightWheel.steerAngle = angle;
            }
        }
    }
}