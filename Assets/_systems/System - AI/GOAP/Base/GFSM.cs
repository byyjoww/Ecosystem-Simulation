using System;
using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.GOAP
{
    public class GFSM : MonoBehaviour
    {
        [SerializeField] private GState currentState;        
        public System.Action OnActionComplete;
        public System.Action OnActionFailed;

        private void Start()
        {
            var data = new GStateData();
            currentState = new Idle(data);
        }

        private void Update()
        {
            currentState = currentState.Process();
        }

        public void Move(GStateData data)
        {            
            var navigate = new Navigate(data);
            currentState.Transition(navigate);
        }

        public void Activity(GStateData data)
        {
            var activity = new Activity(data);
            currentState.Transition(activity);
        }

        public GState.STATE GetState()
        {
            return currentState.CurrentStateName;
        }
    }

    public struct GStateData
    {
        public GStateData(GFSM controller, NavMeshAgent agent, Vector3 targetLocation, Animator anim, string animationClip, float transitionTime)
        {
            this.controller = controller;
            this.agent = agent;
            this.targetLocation = targetLocation;
            this.targetTransform = null;
            this.anim = anim;
            this.animationClip = animationClip;
            this.transitionTime = transitionTime;
        }

        public GStateData(GFSM controller, NavMeshAgent agent, Transform targetTransform, Animator anim, string animationClip, float transitionTime)
        {
            this.controller = controller;
            this.agent = agent;
            this.targetLocation = null;
            this.targetTransform = targetTransform;
            this.anim = anim;
            this.animationClip = animationClip;
            this.transitionTime = transitionTime;
        }

        public GFSM controller;
        public NavMeshAgent agent;
        public Vector3? targetLocation;
        public Transform targetTransform;
        public Animator anim;
        public string animationClip;
        public float transitionTime;
    }

    [System.Serializable]
    public class GState
    {
        public enum STATE { IDLE = 0, NAVIGATE = 1, ACTIVITY = 2 }
        public enum EVENT { ENTER = 0, UPDATE = 1, EXIT = 2 }

        [SerializeField] protected STATE currentStateName;
        public STATE CurrentStateName => currentStateName;
        [SerializeField] protected EVENT stateStage;

        [SerializeField] protected STATE nextStateName = STATE.IDLE;
        [SerializeField] protected GState nextState;

        [SerializeField] protected GFSM controller;
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Vector3? targetLocation;
        [SerializeField] protected Transform targetTransform;
        [SerializeField] protected Animator anim;
        [SerializeField] protected string animationClip;
        [SerializeField] protected float transitionTime;

        public GState (GStateData data)
        {
            //anim.ResetTrigger(animationClip);
            this.animationClip = data.animationClip;
            this.anim = data.anim;
            this.controller = data.controller;
            this.agent = data.agent;
            this.targetLocation = data.targetLocation;
            this.targetTransform = data.targetTransform;
            this.transitionTime = data.transitionTime;
            stateStage = EVENT.ENTER;
        }

        public virtual void Enter() 
        {
            Debug.Log($"Entered {currentStateName} State.");
            stateStage = EVENT.UPDATE;
        }

        public virtual void Update() 
        {
            //Debug.Log($"{name} Update.");
        }

        public virtual void Exit() 
        {             
            Debug.Log($"Exited {currentStateName} State.");
            Debug.Log($"Next state is {nextState.currentStateName}.");
        }

        public GState Process()
        {
            if (stateStage == EVENT.ENTER) { Enter(); }
            if (stateStage == EVENT.UPDATE) { Update(); }
            if (stateStage == EVENT.EXIT) { Exit(); return nextState; }
            return this;
        }

        public void Transition(GState nextState)
        {
            Debug.Log($"Transitioning states. Next state: {nextState.currentStateName}.");
            this.nextState = nextState;
            this.nextStateName = nextState.currentStateName;
            stateStage = EVENT.EXIT;
        }
    }

    [System.Serializable]
    public class Navigate : GState
    {
        public Navigate(GStateData data) : base(data)
        {
            currentStateName = STATE.NAVIGATE;
            //anim.ResetTrigger(animationClip);
            //this.animationClip = data.animationClip;
            //this.anim = data.anim;
            //this.controller = data.controller;
            //this.agent = data.agent;
            //this.targetLocation = data.targetLocation;
            //this.targetTransform = data.targetTransform;
            //this.transitionTime = data.transitionTime;
            //stateStage = EVENT.ENTER;
        }

        public override void Enter()
        {
            if(targetTransform == null && targetLocation == null) { stateStage = EVENT.EXIT; }

            base.Enter();

            var location = targetTransform != null ? targetTransform.position : (Vector3)targetLocation;

            agent.SetDestination(location);
            agent.isStopped = false;
        }

        public override void Update()
        {
            base.Update();

            if (targetTransform == null && targetLocation == null) 
            { 
                controller.OnActionFailed?.Invoke();
                Debug.Log("No remaining path was found. Exiting State.");
                return;
            }

            var location = targetTransform != null ? targetTransform.position : (Vector3)targetLocation;

            if (!agent.SetDestination(location))
            {
                controller.OnActionFailed?.Invoke();
                Debug.Log("No remaining path was found. Exiting State.");
                return;
            }

            float distanceToTarget = Vector3.Distance(location, agent.transform.position);

            if (agent.hasPath && distanceToTarget <= agent.stoppingDistance + 1f)
            {
                Debug.Log("Agent has arrived at destination.");
                controller.OnActionComplete?.Invoke();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    [System.Serializable]
    public class Activity : GState
    {
        public bool activityComplete = false;
        public bool raised = false;

        public Activity(GStateData data) : base(data)
        {
            currentStateName = STATE.ACTIVITY;
            //anim.ResetTrigger(animationClip);
            //this.animationClip = data.animationClip;
            //this.anim = data.anim;
            //this.controller = data.controller;
            //this.agent = data.agent;
            //this.targetLocation = data.targetLocation;
            //this.targetTransform = data.targetTransform;
            //this.transitionTime = data.transitionTime;
            //stateStage = EVENT.ENTER;
        }

        public override void Enter()
        {
            agent.isStopped = true;
            //if(anim == null) anim.SetTrigger(animationClip);            

            if (transitionTime > 0)
            {
                Tools.Invoke(controller, () => { activityComplete = true; }, transitionTime);
            }
            else
            {
                activityComplete = true;                
            }
            
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (activityComplete && !raised)
            {
                controller.OnActionComplete?.Invoke();
                raised = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Idle : GState
    {
        public Idle(GStateData data) : base(data)
        {
            currentStateName = STATE.IDLE;
            //anim.ResetTrigger(animationClip);
            this.animationClip = data.animationClip;
            this.anim = data.anim;
            this.controller = data.controller;
            this.agent = data.agent;
            this.targetLocation = data.targetLocation;
            this.transitionTime = data.transitionTime;
            stateStage = EVENT.ENTER;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}