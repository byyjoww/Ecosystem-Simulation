using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelAnimation
{
    private Rigidbody rigidbody;
    private List<WheelCollider> wheels;

    public WheelAnimation(Rigidbody rigidbody, Axles[] axles, IUpdater updater)
    {
        this.rigidbody = rigidbody;
        wheels = new List<WheelCollider>();

        foreach (Axles axle in axles)
        {
            wheels.Add(axle.leftWheel);
            wheels.Add(axle.rightWheel);
        }

        updater.OnUpdateEvent += RotateWheels;
        updater.OnUpdateEvent += SpinWheels;
    }

    public void RotateWheels()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.transform.localEulerAngles = new Vector3(0f, wheel.steerAngle, 0f);
        }
    }

    public void SpinWheels()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.transform.GetChild(0).Rotate(rigidbody.velocity.magnitude * (rigidbody.transform.InverseTransformDirection(rigidbody.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * wheel.radius), 0f, 0f);
        }
    }
}
