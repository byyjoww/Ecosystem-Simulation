using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceData : MonoBehaviour
{
    [Header("Lap Management")]
    [SerializeField, ReadOnly] public WaypointManager waypointManager;
    [SerializeField, ReadOnly] public int currentNode;
    [SerializeField, ReadOnly] public int numberOfLaps = 0;

    [Header("Race Complete Event")]
    public UnityEvent OnRaceComplete;

    private void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
    }

    private void Update()
    {
        CheckWaypoint();
    }

    private void CheckWaypoint()
    {
        int previousNode = (int)Tools.Modulus((currentNode - 1), waypointManager.waypoints.Count);
        Vector3 line = waypointManager.waypoints[currentNode].transform.position - waypointManager.waypoints[previousNode].transform.position;
        Vector3 worldRelativePosition = waypointManager.waypoints[currentNode].transform.position - transform.position;

        if (Vector3.Dot(line, worldRelativePosition) < (0.5 * line.sqrMagnitude))
        {
            if (currentNode == waypointManager.waypoints.Count - 1)
            {
                Debug.Log($"{this.gameObject.name} has completed lap {numberOfLaps}.");
                numberOfLaps++;
            }

            currentNode++;
            currentNode %= waypointManager.waypoints.Count;

            if (numberOfLaps >= 3 && currentNode >= 2)
            {
                Debug.Log($"{this.gameObject.name} has completed the race.");
                OnRaceComplete?.Invoke();
            }
        }
    }
}
