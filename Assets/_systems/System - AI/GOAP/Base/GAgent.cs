using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Elysium.AI.GOAP;
using System;

namespace Elysium.AI.GOAP
{
    public class GAgent : MonoBehaviour
    {
        [Header("LIST OF POSSIBLE ACTIONS")]
        public List<GAction> actions = new List<GAction>();

        // ----- TASKS IN PROGRESS -----

        [Header("GOALS")]
        public GAction currentAction;
        SubGoal currentGoal;        

        public Transform target { get; set; }
        public IResource resourceInUse;

        // ----- GOAL AND PRIORITY -----

        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        public void ChangePriority(State g, int newPriority)
        {
            SubGoal key = goals.Keys.Single(x => x.goalName == g);

            if (key != null)
            {
                goals[key] = newPriority;
            }

            if (currentAction != null && currentAction.GoalPriorityChange)
            {
                ForceNewPlan();
            }
        }

        // --------- INVENTORY ---------

        public GInventory inventory = new GInventory();

        // ---------- BELIEFS ----------

        public WorldStates beliefs = new WorldStates();

        // ---------- PLANNER ----------

        private GPlanner planner;
        protected Queue<GAction> actionQueue;
        public List<GAction> ActionList;

        // ---------- REFERENCES ----------

        public NavMeshAgent agent;
        public GFSM controller;
        public Animator anim;

        // ---------- VARIABLES ----------

        // If an action is already being run
        public bool isRunningAction = false;

        [SerializeField] private float viewSphereRadius = 10f;
        public float ViewSphereRadius => viewSphereRadius;

        [SerializeField] private float viewAngle = 60f;
        public float ViewAngle => viewAngle;

        private void OnDrawGizmos()
        {                        
            Gizmos.color = Color.red;
            foreach (var location in locationList)
            {
                Gizmos.DrawWireSphere(location, 1f);
            }
        }

        private List<Vector3> locationList = new List<Vector3>();

        public void GenerateLocationGizmo(Vector3 location)
        {
            locationList.Add(location);
        }

        // ------------- FUNCTIONS -------------

        protected virtual void Start()
        {
            controller.OnActionComplete += CompleteAction;
            controller.OnActionFailed += ForceNewPlan;
        }

        void LateUpdate()
        {
            if (!CheckCurrentActionIsValid()) { return; }

            // Current action is null, or isn't running

            FindNewPlan();

            // No current action, but has a planner or an action queue

            CheckForEmptyActionQueue();

            // No current action, but a valid action queue

            LoadNextAction();
        }

        #region FUNCTIONS
        /// <summary>
        /// Break out of 'update' if there's a current action and if it is still running.
        /// </summary>
        private bool CheckCurrentActionIsValid()
        {
            // --- CHECK IF THERE IS AN ACTION, AND IF ITS CURRENTLY RUNNING ---

            if (currentAction != null && isRunningAction)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// If the planner and action queue are null, find and start a new plan.
        /// </summary>
        private void FindNewPlan()
        {
            // ------- CHECK IF THE PLANNER OR ACTION QUEUE ARE NULL --------

            if (planner == null || actionQueue == null)
            {

            // -------------------- CREATE A NEW PLANNER --------------------

                planner = new GPlanner();
                target = null;

            // --------------- SORT GOALS IN DESCENDING ORDER ---------------

                var sortedGoals = SortGoals();

            // ---------------------- FIND A NEW PLAN -----------------------

                foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.Plan(actions, sg.Key.sGoals, beliefs);
                    if (actionQueue != null) { ActionList = new List<GAction>(actionQueue); }                    

            // ------------------ CHECK IF WE FOUND A PLAN ------------------

                    if (actionQueue != null)
                    {                        
                        currentGoal = sg.Key; // Set the current goal
                        break; // Goal is found, break out of the loop
                    }
                }
            }
        }

        /// <summary>
        /// Checks if action queue is empty and both: 1) removes sub-goal (if applicable) 2) sets planner to null, so a new plan is created.
        /// </summary>
        private void CheckForEmptyActionQueue()
        {
            // ------------- HAS AN EMPTY ACTION QUEUE --------------

            if (actionQueue != null && actionQueue.Count == 0)
            {

            // ----- CHECK IF ACTION IS REMOVABLE AND REMOVE IT -----

                if (currentGoal.remove)
                {                    
                    goals.Remove(currentGoal);
                }

            // ----------- TRIGGER A NEW PLAN NEXT UPDATE -----------

                planner = null;
            }
        }

        /// <summary>
        /// Load next action in the queue.
        /// </summary>
        private void LoadNextAction()
        {
            // ------- HAS AN ACTION QUEUE CONTAINING ACTIONS -------

            if (actionQueue != null && actionQueue.Count > 0)
            {

            // --------- SET CURRENT ACTION TO NEXT ACTION ----------

                currentAction = actionQueue.Dequeue();

            // ----------- CHECKS PRE PERFORM CONDITIONS ------------

                if (currentAction.PrePerform(this))
                {
                    Debug.Log($"Pre perform successful for {currentAction.name}.");

            // ------------------ ACTIVATE ACTION -------------------

                    isRunningAction = true;
                    currentAction.RunMainAction(this);            
                }
                else
                {

            // ----------------- FORCE A NEW PLAN -----------------

                    actionQueue = null;
                }
            }
        }

        /// <summary>
        /// An invoked method to allow an agent to be performing a task for a set location.
        /// </summary>
        public void CompleteAction()
        {
            isRunningAction = false;
            currentAction.PostPerform(this);
        }

        /// <summary>
        /// Forces a new plan, regardless of the status of the current action.
        /// </summary>
        public void ForceNewPlan()
        {
            isRunningAction = false;
            actionQueue = null;
            planner = null;
        }

        /// <summary>
        /// Sort goals based on their priority.
        /// </summary>
        public IOrderedEnumerable<KeyValuePair<SubGoal, int>> SortGoals()
        {
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            //Debug.LogError("Reorganized List - Printing Sorted Goals: ");

            foreach (var goal in sortedGoals)
            {
                //Debug.Log($"Goal: {goal.Key.goalName} | Value: {goal.Value}.");
            }

            return sortedGoals;
        }
        #endregion

        private void OnValidate()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (controller == null) controller = GetComponent<GFSM>();
        }
    }
}