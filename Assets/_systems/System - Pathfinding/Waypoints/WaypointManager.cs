using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Color regularLineColor;
    public Color startLineColor;

    public List<Waypoint> waypoints = new List<Waypoint>();

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 currentNode = waypoints[i].transform.position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
            {
                previousNode = waypoints[i - 1].transform.position;
            }
            else if (i == 0 && waypoints.Count > 1)
            {
                previousNode = waypoints[waypoints.Count - 1].transform.position;
            }

            if (i == 0)
            {
                Gizmos.color = startLineColor;
            }
            else
            {
                Gizmos.color = regularLineColor;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }

    private void OnValidate()
    {
        Waypoint[] waypointArray = GetComponentsInChildren<Waypoint>();
        waypoints = waypointArray.OrderBy(x => x.index).ToList();
    }
}
