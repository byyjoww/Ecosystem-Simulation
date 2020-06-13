using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.AI
{
    public static class AICrowdSimulation
    {
        #region FLOCKING_BEHAVIOR
        /// <summary>
        /// Create a new flock and assign the necessary behavior.
        /// </summary>
        public static void CreateFlock(FlockingData flockingData)
        {
            flockingData.FlockGoal = flockingData.FlockCenter.position;
            flockingData.allUnits = new UnitData[flockingData.numUnits];

            for (int i = 0; i < flockingData.numUnits; i++)
            {
                Vector3 pos = flockingData.FlockGoal + new Vector3(Random.Range(-flockingData.FlockLimits.x, flockingData.FlockLimits.x),
                                                                   Random.Range(-flockingData.FlockLimits.y, flockingData.FlockLimits.y),
                                                                   Random.Range(-flockingData.FlockLimits.z, flockingData.FlockLimits.z));

                flockingData.allUnits[i].transform = GameObject.Instantiate(flockingData.unitPrefab, pos, Quaternion.identity).transform;
                flockingData.allUnits[i].speed = Random.Range(flockingData.unitMinSpeed, flockingData.unitMaxSpeed);
            }
        }

        /// <summary>
        /// Generates a flocking behavior based on parameters contained within FlockingData.
        /// </summary>
        public static void Flock(FlockingData flockingData, bool checkForBounds = true, bool checkForCollision = true)
        {
            // Change goal position within flock limits based on random frequency
            if (Random.Range(0, 100) < flockingData.positionGoalChangeFrequency)
            {
                flockingData.AdjustGoalWithinFlockRange(Random.Range(-flockingData.FlockLimits.x, flockingData.FlockLimits.x),
                                                        Random.Range(-flockingData.FlockLimits.y, flockingData.FlockLimits.y),
                                                        Random.Range(-flockingData.FlockLimits.z, flockingData.FlockLimits.z));
            }

            for (int i = 0; i < flockingData.allUnits.Length; i++)
            {
                UnitData unitData = flockingData.allUnits[i];

                Vector3 direction = Vector3.zero;
                Bounds bounds = new Bounds(flockingData.FlockGoal, flockingData.FlockLimits * 2);
                RaycastHit? nullableHitInfo = CheckForCollision(unitData.transform);

                // Rotate unit back towards the flock if they move outside of flock limit
                if (CheckFlockBounds(unitData.transform, bounds) && checkForBounds)
                {
                    direction = flockingData.FlockGoal - unitData.transform.position;
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Rotate unit away to avoid obstacles
                else if (nullableHitInfo != null && checkForCollision)
                {
                    RaycastHit hitInfo = (RaycastHit)nullableHitInfo;
                    Debug.DrawRay(unitData.transform.position, hitInfo.point, Color.yellow);
                    direction = Vector3.Reflect(unitData.transform.forward, hitInfo.normal);
                    Debug.DrawRay(unitData.transform.position, direction, Color.red);
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Apply regular flocking behavior based on random frequency
                else
                {
                    //direction = new Vector3(Random.Range(-flockingData.FlockLimits.x, flockingData.FlockLimits.x),
                    //                        Random.Range(-flockingData.FlockLimits.y, flockingData.FlockLimits.y),
                    //                        Random.Range(-flockingData.FlockLimits.z, flockingData.FlockLimits.z));

                    if (Random.Range(0, 100) < 1)
                    {
                        unitData.speed = Random.Range(flockingData.unitMinSpeed, flockingData.unitMaxSpeed);
                    }

                    if (Random.Range(0, 100) < flockingData.flockingFrequency)
                    {
                        direction = FlockDirection(unitData, flockingData);
                    }
                }

                if (direction != Vector3.zero)
                {
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }

                unitData.transform.Translate(0, 0, Time.deltaTime * unitData.speed);
            }
        }

        public static Vector3 FlockDirection(UnitData unitData, FlockingData flockingData)
        {
            Vector3 headingVector = Vector3.zero;
            Vector3 avoidanceVector = Vector3.zero;
            Vector3 direction = Vector3.zero;

            float groupSpeed = 0.01f;
            float minDistanceToGroup;
            int groupSize = 0;

            foreach (var unit in flockingData.allUnits)
            {
                if (unit.transform != unitData.transform)
                {
                    // Check distance to each fish in flock
                    minDistanceToGroup = Vector3.Distance(unit.transform.position, unitData.transform.position);
                    if (minDistanceToGroup <= flockingData.maximumGroupDistance)
                    {
                        //Add that fish to group & add their position to group avg (flock center avg)
                        headingVector += unit.transform.position;
                        groupSize++;

                        // Check if distance is bigger than minimum distance value
                        if (minDistanceToGroup < flockingData.minimumUnitProximity)
                        {
                            // Include that unit's position to the avoidance vector (don't hit neighbours)
                            avoidanceVector = avoidanceVector + (unitData.transform.position - unit.transform.position);
                        }

                        // TEMPORARY - Grab the speed of each unit and add it to the global speed value (determine global flock speed)
                        groupSpeed = groupSpeed + unitData.speed;
                    }
                }
            }

            if (groupSize > 0)
            {
                // Find the avg vector of the flock's center based on group size
                headingVector = headingVector / groupSize;

                // If a goal position is set, calculate the flock's heading based on goal location
                if (flockingData.FlockGoal != null)
                {
                    headingVector = headingVector + (flockingData.FlockGoal - unitData.transform.position);
                }

                // Set the individual's speed to the avg group speed
                unitData.speed = groupSpeed / groupSize;

                // Determine the direction the fish wants to travel to (based on center of flock & avoidance of neighbours)
                direction = (headingVector + avoidanceVector) - unitData.transform.position;
            }

            return direction;
        }

        public static RaycastHit? CheckForCollision(Transform currentUnit)
        {
            RaycastHit hitInfo = new RaycastHit();
            var rc = Physics.Raycast(currentUnit.position, currentUnit.forward.normalized, out hitInfo, 1f);

            if (rc)
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                return hitInfo;
            }
            else
            {
                return null;
            }
        }

        public static bool CheckFlockBounds(Transform currentUnit, Bounds bounds)
        {
            if (!bounds.Contains(currentUnit.position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region FLOCKING_CLASS
    [System.Serializable]
    public class FlockingData
    {
        [Header("Unit Details")]
        public GameObject unitPrefab;
        public int numUnits = 20;
        [Range(0.0f, 5.0f)] public float unitMinSpeed = 1f;
        [Range(0.0f, 5.0f)] public float unitMaxSpeed = 2f;
        [Range(0.0f, 5.0f)] public float unitRotationSpeed = 1f;

        [Header("Flock Location")]
        [SerializeField] private Transform flockCenter;
        public Transform FlockCenter => flockCenter;
        [SerializeField] private Vector3 flockLimits = new Vector3(5f, 5f, 5f);
        public Vector3 FlockLimits => flockLimits * flockLimitSizeMultiplier;
        [Range(1.0f, 10.0f)] public float flockLimitSizeMultiplier = 1f;

        [Header("Flock Movement")]
        [Range(0.0f, 100.0f)] public float positionGoalChangeFrequency = 1f;
        [Range(0.0f, 100.0f)] public float flockingFrequency = 20f;
        [Range(0.0f, 5.0f)] public float minimumUnitProximity = 1f;
        [Range(1.0f, 10.0f)] public float maximumGroupDistance = 1f;

        public UnitData[] allUnits;
        public Vector3 FlockGoal { get; set; }

        public void AdjustGoalWithinFlockRange(float x, float y, float z)
        {
            FlockGoal = flockCenter.position + new Vector3(x, y, z);
            Debug.Log($"The flock's goal position has been set to {FlockGoal}.");
        }
    }

    public struct UnitData
    {
        public Transform transform;
        public float speed;
    }
    #endregion
}