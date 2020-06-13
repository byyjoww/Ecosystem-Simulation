using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI;

public class FlockManager : MonoBehaviour
{
    public FlockingData flockingData = new FlockingData();

    private void Start()
    {
        AICrowdSimulation.CreateFlock(flockingData);
    }

    private void Update()
    {
        AICrowdSimulation.Flock(flockingData);
    }
}
