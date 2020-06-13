using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace PluggableAI
{
    public class StateController : MonoBehaviour
    {
        [Header("STATE DETAILS")]
        public State currentState;
        public State remainState;

        [Header("AI DETAILS")]
        [SerializeField] private Transform firepoint;
        public Transform Firepoint => firepoint;
        [SerializeField, Range(0, 50)] private float viewSphereRadius = 10;
        public float ViewSphereRadius => viewSphereRadius;
        [SerializeField, Range(0, 90)] private float viewAngle = 60;
        public float ViewAngle => viewAngle;
        [SerializeField, Range(0, 50)] private float attackRange = 5;
        public float AttackRange => attackRange;
        [SerializeField, Range(0, 50)] private float rotateSpeed = 2;
        public float RotateSpeed => rotateSpeed;

        [Header("WANDER DETAILS")]
        [SerializeField, Range(0, 100)] private float wanderRadius = 10f;
        public float WanderRadius => wanderRadius;
        [SerializeField, Range(0, 100)] private float wanderDistance = 20f;
        public float WanderDistance => wanderDistance;
        [SerializeField, Range(0, 100)] private float wanderJitter = 1f;
        public float WanderJitter => wanderJitter;

        [SerializeField] private List<Vector3> destinationList = new List<Vector3>();
        public List<Vector3> DestinationList
        {
            get
            {
                destinationList.RemoveAll(x => x == null);
                return destinationList;
            }
        }

        [Header("NAV MESH NAVIGATION")]
        [SerializeField] private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent => navMeshAgent;

        [Header("Behaviour")]
        public IEater Eater;
        public IEatable Eatable;
        public Exhaustion Exhaustion;

        // PROPERTIES
        private bool aiActive { get; set; }

        public bool IsBeingEaten => Eatable.IsBeingEaten;
        public bool IsEating { get; set; }

        public bool IsExhausted => Exhaustion.IsExhausted;
        public bool IsHungry => Eater.IsHungry;

        public Transform ChaseTarget { get; set; }
        public Transform FleeTarget { get; set; }

        public Vector3 WanderOrigin { get; set; }        

        public bool ExitTime { get; set; }
        public bool HasExitTimer { get; set; }

        private void Start()
        {
            Eater = GetComponent<IEater>();
            Eatable = GetComponent<IEatable>();
            Exhaustion = GetComponent<Exhaustion>();
            WanderOrigin = new Vector3(0,0,1);
            aiActive = true;
        }

        private void Update()
        {
            if (!aiActive)
            {
                return;
            }

            currentState.UpdateState(this);
        }

        private void OnDrawGizmos()
        {
            if(currentState != null)
            {
                // Draw wire sphere indicating the detection radius, with the color indicating the AI's state
                Gizmos.color = currentState.sceneGizmoColor;
                Gizmos.DrawWireSphere(firepoint.position, viewSphereRadius);

                // Draw wire sphere indicating the wander radius
                Gizmos.color = Color.black;
                Gizmos.DrawRay(transform.position, (transform.forward * wanderDistance));
                Gizmos.DrawWireSphere(transform.position + (transform.forward * wanderDistance), wanderRadius);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(WanderOrigin, 1f);

                // Draw rays indicating the field of view limits
                Gizmos.color = Color.black;

                Vector3 minFieldOfView = Quaternion.AngleAxis(-viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
                Gizmos.DrawRay(transform.position, minFieldOfView);

                Vector3 maxFieldOfView = Quaternion.AngleAxis(viewAngle, Vector3.up) * transform.forward * viewSphereRadius;
                Gizmos.DrawRay(transform.position, maxFieldOfView);

                // Draw wire spheres at travel destinations
                Gizmos.color = Color.blue;
                foreach (var item in DestinationList)
                {
                    Gizmos.DrawWireSphere(item, 1f);
                }
                //if (DestinationList.Count > 0)
                //{
                //    Gizmos.color = Color.blue;
                //    Gizmos.DrawWireSphere(DestinationList[0], 1f);
                //}
                //if (DestinationList.Count > 1)
                //{
                //    Gizmos.color = Color.cyan;
                //    Gizmos.DrawWireSphere(DestinationList[1], 1f);
                //}                    
            }
        }

        public void TransitionToState(State nextState)
        {
            
            if(nextState != remainState)
            {
                currentState = nextState;
            }
        }

        /// <summary>
        /// Function that will start chasing the opponent vision.
        /// </summary>
        private Timer rotateTimer { get; set; }
        private bool isTimerComplete { get; set; }

        public bool RotateInCircles(StateController controller, float stateTimeLimitSeconds)
        {
            if (rotateTimer == null)
            {
                isTimerComplete = false;
                rotateTimer = new Timer(stateTimeLimitSeconds * 1000);
                rotateTimer.Enabled = true;
                rotateTimer.Elapsed += (object o, ElapsedEventArgs e) => { isTimerComplete = true; rotateTimer.Dispose(); };
            }

            if (isTimerComplete)
            {
                return true;
            }

            controller.NavMeshAgent.isStopped = true;
            controller.transform.Rotate(0, controller.RotateSpeed * Time.deltaTime, 0);
            return false;
        }
    }
}
