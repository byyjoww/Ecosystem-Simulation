using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

[System.Serializable]
public class NPCInputController : MonoBehaviour
{
    [Header("AI Profile")]
    [SerializeField] public AIDriverProfile driverProfile;
    public enum AIDriverProfile { Passive = 0, Agressive = 1 }    

    private enum AIState { Pathfinding = 0, Crashed = 1, Pursuit = 2 }
    private Dictionary<AIState, Action> stateDictionary;
    [SerializeField] private List<Transition> transitionList;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Engine engine;
    [SerializeField] private Wheels wheels;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private WaypointManager waypointManager;

    //AI
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField, ReadOnly] private int currentNode = 0;
    [SerializeField] private AIState state = AIState.Pathfinding;

    public NPCInputController(Engine engine)
    {
        waypoints = waypointManager.waypoints;
        this.engine = engine;

        stateDictionary = new Dictionary<AIState, Action>() 
        {       
            { AIState.Pathfinding, Pathfinding },
            { AIState.Crashed, Crashed },
            { AIState.Pursuit, Pursuit }
        };

        transitionList = new List<Transition>()
        {
            new Transition(AIState.Pathfinding, AIState.Crashed, AnyToCrashed),
            new Transition(AIState.Pathfinding, AIState.Pursuit, PathfindingToPursuit),
            new Transition(AIState.Crashed, AIState.Pathfinding, CrashedToPathfinding),
            new Transition(AIState.Pursuit, AIState.Pathfinding, PursuitToPathfinding),
            new Transition(AIState.Pursuit, AIState.Crashed, AnyToCrashed),
        };
    }

    [SerializeField] private float secondsBeforeCrashCheck;
    [SerializeField] private float crashDistance = 2f;
    [SerializeField] private float reverseDistance = 5f;
    [SerializeField] private float directTargetDistance = 10f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float fieldOfView = 10f;
    [SerializeField] private float curveCoeficient = 15f;
    [SerializeField] private bool checkingForCrashed = false;
    [SerializeField] private bool crashOverride = false;

    private float CurveModifier => 1 - 0.7f / (1 + Mathf.Pow(engine.CurrentSpeed / curveCoeficient,2));

    private bool AnyToCrashed()
    {
        bool isCrashed = false;

        if(engine.CurrentSpeed < 0.5f)
        {
            Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            Debug.DrawRay(raycastPosition, transform.forward * crashDistance, Color.cyan);
            isCrashed = Physics.Raycast(raycastPosition, transform.forward, crashDistance);

            if (!isCrashed && !checkingForCrashed)
            {
                checkingForCrashed = true;

                // CREATE NEW 3000 ms TIMER
                Timer timer = new Timer();
                timer.Interval = secondsBeforeCrashCheck * 1000;
                timer.Enabled = true;
                timer.Elapsed += (object o, ElapsedEventArgs e) => CheckForStillCrashed();
            }

            if (crashOverride)
            {
                isCrashed = true;
                crashOverride = false;
            }

            if (isCrashed)
            {
                ResetWaypoint();
            }
        }

        return isCrashed;
    }

    protected bool isEnabled = true;

    public void SetActive(bool enabled)
    {
        Debug.LogError($"I have been disabled.");
        this.isEnabled = enabled;
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckForStillCrashed()
    {
        if (engine.CurrentSpeed < 0.5f)
        {
            crashOverride = true;
        }

        checkingForCrashed = false;
    }

    private bool CrashedToPathfinding()
    {
        Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

        Debug.DrawRay(raycastPosition, transform.forward * reverseDistance, Color.cyan);
        bool frontRay = Physics.Raycast(raycastPosition, transform.forward, reverseDistance);

        Debug.DrawRay(raycastPosition, transform.forward * 2f, Color.cyan);
        bool backRay = Physics.Raycast(raycastPosition, - transform.forward, 2f);

        return !frontRay || backRay;
    }

    private bool PursuitToPathfinding()
    {
        bool changeToPathfinding = false;

        if (driverProfile == NPCInputController.AIDriverProfile.Passive)
        {
            if (!CheckForDirectTarget())
            {
                changeToPathfinding = true;
            }
            else
            {
                changeToPathfinding = false;
            }
        }

        if (driverProfile == NPCInputController.AIDriverProfile.Agressive)
        {
            if (CheckForSpecificTarget(this.target))
            {
                changeToPathfinding = false;
            }
            else
            {
                changeToPathfinding = true;
            }
        }

        return changeToPathfinding;
    }

    private bool PathfindingToPursuit()
    {        
        bool changeToPursuit = false;

        if (driverProfile == NPCInputController.AIDriverProfile.Passive)
        {
            Transform target = CheckForDirectTarget();

            if (target)
            {
                changeToPursuit = true;
                this.target = target;
            }
        }

        if (driverProfile == NPCInputController.AIDriverProfile.Agressive)
        {
            Transform target = CheckForTarget();

            if (target)
            {
                changeToPursuit = true;
                this.target = target;
            }
        }

        return changeToPursuit;
    }

    private void CheckState()
    {
        foreach (Transition transition in transitionList)
        {
            if (transition.input == state && transition.transition.Invoke())
            {
                state = transition.output;
                break;
            }
        }

        stateDictionary[state]?.Invoke();
    }

    private void MoveTowards(Vector3 movementVector)
    {        
        CheckWaypoint(movementVector);
        CheckForVertical(1);

        if (Vector3.Dot(rigidbody.velocity, transform.forward) > 0)
        {
            CheckForHorizontal(movementVector);
        }
    }

    private void Pathfinding()
    {
        Vector3 movementVector = GetAlternatePath(transform.position);
        MoveTowards(movementVector);
    }

    private void Crashed()
    {
        wheels.MoveHorizontal(0f, true);
        CheckForVertical(-1);
    }

    private void Pursuit()
    {
        Vector3 movementVector = GetPath(target.position);
        MoveTowards(movementVector);
        Shoot(CheckForDirectTarget());
    }

    private Transform CheckForDirectTarget()
    {
        Transform target = null;
        RaycastHit hit;

        Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Debug.DrawRay(raycastPosition, transform.forward * directTargetDistance, Color.red);
        if (Physics.Raycast(raycastPosition, transform.forward, out hit, directTargetDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                target = hit.collider.transform;
            }
        }

        return target;
    }    

    private Transform CheckForTarget()
    {
        Transform target = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, fieldOfView);
        float minDistance = Mathf.Infinity;
        Vector3 alt = waypoints[currentNode].transform.position - transform.position;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (Vector3.Dot(alt, collider.transform.position - transform.position) > 0)
                {
                    var distance = Vector3.Distance(collider.transform.position, transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = collider.transform;
                    }
                }
            }
        }

        return target;
    }

    private bool CheckForSpecificTarget(Transform target)
    {
        bool targetStillInRange = false;

        if (!target)
        {
            return false;
        }

        if (Vector3.Dot(waypoints[currentNode].transform.position - transform.position, target.position - transform.position) > 0)
        {
            var distance = Vector3.Distance(target.position, transform.position);

            if (distance < fieldOfView)
            {
                targetStillInRange = true;
            }
        }

        return targetStillInRange;
    }

    private void Shoot(bool targetAcquired)
    {
        if (targetAcquired)
        {
            int i = UnityEngine.Random.Range(0, weapons.Length);
            weapons[i].Shoot();
        }
    }

    private Vector3 GetPath(Vector3 vectorToMoveTowards)
    {
        Vector3 relativeVector = transform.InverseTransformPoint(vectorToMoveTowards);
        return relativeVector;
    }

    private Vector3 GetAlternatePath(Vector3 position)
    {        
        int previousNode = (int)Tools.Modulus((currentNode - 1), waypoints.Count);
        Vector3 line = waypoints[currentNode].transform.position - waypoints[previousNode].transform.position;
        Vector3 worldRelativePosition = position - waypoints[previousNode].transform.position;

        Vector3 delta = worldRelativePosition - (Vector3.Dot(worldRelativePosition, line) / line.sqrMagnitude) * line;
        Vector3 fakeWaypoint = waypoints[currentNode].transform.position + delta;

        Vector3 relativeVector = transform.InverseTransformPoint(fakeWaypoint);
        Debug.DrawLine(transform.position, fakeWaypoint);

        return relativeVector;
    }

    private void CheckWaypoint(Vector3 vector)
    {
        int previousNode = (int)Tools.Modulus((currentNode - 1), waypoints.Count);
        Vector3 line = waypoints[currentNode].transform.position - waypoints[previousNode].transform.position;
        Vector3 worldRelativePosition = waypoints[currentNode].transform.position - transform.position;

        if (Vector3.Dot(line, worldRelativePosition) < (CurveModifier * line.sqrMagnitude))
        {
            currentNode++;
            currentNode %= waypoints.Count;
        }
    }

    private void ResetWaypoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestWaypoint = currentNode;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = i;
            }
        }

        currentNode = closestWaypoint;
    }

    private void CheckForVertical(int input)
    {
        engine.MoveVertical(input);
    }

    private void CheckForHorizontal(Vector3 vector)
    {        
        float newSteer = (vector.x / vector.magnitude) * wheels.MaxSteeringAngle;
        wheels.MoveHorizontal(newSteer, true);
    }

    private class Transition
    {
        public readonly AIState input;
        public readonly AIState output;
        public readonly Func<bool> transition;

        public Transition(AIState input, AIState output, Func<bool> transition)
        {
            this.input = input;
            this.output = output;
            this.transition = transition;
        }
    }
}