using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Engine
{
    [Header("Car Stats")]
    private float maxSpeed;
    public float MaxSpeed => maxSpeed;
    private float maxMotorTorque;
    public float MaxMotorTorque => maxMotorTorque;

    [Header("Input Events")]
    private Action<float> vertical;

    [Header("Components")]
    private Rigidbody rigidbody;
    private Axles[] axles;

    public float CurrentSpeed => rigidbody.velocity.magnitude;

    public Engine(Rigidbody rigidbody, Axles[] axles, float maxSpeed, float maxMotorTorque, IUpdater updater)
    {
        this.maxSpeed = maxSpeed;
        this.maxMotorTorque = maxMotorTorque;
        this.rigidbody = rigidbody;
        this.axles = axles;
        vertical += MoveVertical;
        updater.OnFixedUpdateEvent += AntiFlip;
        updater.OnFixedUpdateEvent += LimitMaxValues;
    }

    ~Engine()
    {
        vertical -= MoveVertical;
    }

    public void Nitro()
    {
        rigidbody.AddForce(rigidbody.transform.forward * 1000f, ForceMode.Impulse);
    }

    public void Jump()
    {
        rigidbody.AddForce(rigidbody.centerOfMass + new Vector3(0f, 10000f, 0f), ForceMode.Impulse);
    }

    public void MoveVertical(float input)
    {
        foreach (Axles axleInfo in axles)
        {            
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = maxMotorTorque * input;
                axleInfo.rightWheel.motorTorque = maxMotorTorque * input;
            }
        }
    }

    public void AntiFlip()
    {
        rigidbody.centerOfMass = new Vector3(0, 0, 0);
        rigidbody.AddForce(0, -500f, 0);
    }

    public void LimitMaxValues()
    {
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            Vector3 newVelocity = rigidbody.velocity.normalized;
            newVelocity *= maxSpeed;
            rigidbody.velocity = newVelocity;
        }
    }
}