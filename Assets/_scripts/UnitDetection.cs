using Elysium.AI.Navmesh;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitDetection : MonoBehaviour
{
    [SerializeField] private float viewSphereRadius = 20f;
    [SerializeField] private float viewAngle = 60f;

    public List<Transform> AllTargets = new List<Transform>();

    public System.Action<List<Transform>> OnTargetsUpdated;

    private void Start()
    {
        DetectUnit();
    }

    private void Update()
    {
        DetectUnit();
    }

    private void DetectUnit()
    {
        AllTargets = AINavigation.LocateTargetBlind(transform, viewSphereRadius, viewAngle);

        OnTargetsUpdated?.Invoke(AllTargets);
    }

    private void OnDrawGizmos()
    {
        // Draw wire sphere indicating the detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewSphereRadius);

        // Draw rays indicating the field of view limits
        Gizmos.color = Color.black;

        Vector3 minFieldOfView = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
        Gizmos.DrawRay(transform.position, minFieldOfView);

        Vector3 maxFieldOfView = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
        Gizmos.DrawRay(transform.position, maxFieldOfView);
    }
}
