using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public GAction action;

        // The state of the environment by the time the action assigned to this node is achieved
        public List<State> state;        

        // Base Constructor
        public Node(Node parent, float cost, List<State> allStates, GAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new List<State>(allStates);
            this.action = action;
        }

        // Overloaded Constructor
        public Node(Node parent, float cost, List<State> allStates, List<State> beliefStates, GAction action)
        {
            this.parent = parent;
            this.cost = cost;
            this.state = new List<State>(allStates);
            this.action = action;

            // Merge beliefs into all states dictionary
            foreach (var b in beliefStates)
            {
                if (!this.state.Contains(b))
                {
                    this.state.Add(b);
                }
            }            
        }
    }

    public class GPlanner
    {
        /// <summary>
        /// Returns a plan queue for the agent to use.
        /// </summary>
        public Queue<GAction> Plan(List<GAction> actions, Dictionary<State, int> goal, WorldStates beliefStates)
        {
            // ---------- FILTER OUT UNACHIEVABLE ACTIONS ----------

            List<GAction> usableActions = new List<GAction>();            
            foreach (GAction a in actions)
            {
                if (a.IsAchievable())
                {
                    usableActions.Add(a);
                }
            }

            // -------- CREATE THE FIRST NODE IN THE GRAPH --------

            Node startingNode = new Node(null, 0.0f, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

            // ------------- CALCULATE POSSIBLE PATHS -------------

            List<Node> leaves = new List<Node>(); 

            bool success = BuildGraph(startingNode, leaves, usableActions, goal);

            // ------- CHECK IF AT LEAST ONE PLAN WAS FOUND -------

            if (!success)
            {                
                //Debug.Log("NO PLAN");
                return null;
            }

            // -------------- FIND THE CHEAPEST PLAN --------------

            Node cheapest = null;
            foreach (Node leaf in leaves) // CONTAINS ACCUMULATED COST FROM ALL NODES IN EACH PLAN
            {
                if (cheapest == null || leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }

            // --------- ASSEMBLE RESULTING PLAN INTO LIST ---------

            List<GAction> resultingPlanList = new List<GAction>();
            Node n = cheapest;

            while (n != null)
            {
                if (n.action != null)
                {
                    resultingPlanList.Insert(0, n.action);
                }

                n = n.parent;
            }

            // --------- TRANSFER PLAN LIST INTO A QUEUE ---------

            Queue<GAction> resultingPlanQueue = new Queue<GAction>();

            foreach (GAction a in resultingPlanList)
            {
                resultingPlanQueue.Enqueue(a);
            }

            // --------------- PRINT OUT THE PLAN ----------------

            Debug.Log("The Plan is: ");
            foreach (GAction a in resultingPlanQueue)
            {
                Debug.Log("Q: " + a.actionName);
            }

            // ----------------- RETURN THE PLAN -----------------

            return resultingPlanQueue;
        }

        /// <summary>
        /// Will return a true or false value, based on if it found at least one possible path. All paths will be output into the leaves list.
        /// </summary>
        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<State, int> goal)
        {
            // ------------- KEEP TRACK IF A VALID PLAN IS FOUND -------------

            bool foundPath = false;

            // --------------- LOOP THROUGH ALL USABLE ACTIONS ---------------

            foreach (GAction action in usableActions)
            {

            // ---------------- CHECK IF ACTION IS ACHIEVABLE ----------------

                if (action.IsAchievableBasedOnWorldConditions(parent.state))
                {

            // -------- CARRY ON WORLD AND POST EFFECTS TO NEXT NODE --------

                    List<State> currentState = new List<State>(parent.state);
                    foreach (WorldState eff in action.postEffects)
                    {
                        // HERE!
                        if (!currentState.Contains(eff.key))
                        {
                            currentState.Add(eff.key);
                        }
                    }

            // -------- CREATE NEXT NODE AND SET THIS NODE AS PARENT --------

                    Node node = new Node(parent, parent.cost + action.actionCost, currentState, action);

            // -------- CHECK IF THIS CURRENT NODE ACHIEVES THE GOAL --------

                    //~~~> THIS IS THE END NODE <~~~//
                    if (GoalAchieved(goal, currentState))   
                    {                        
                        leaves.Add(node);
                        foundPath = true;
                    }
                    //~~~> THIS ISN'T THE END NODE <~~~//
                    else
                    {                        

            // ----- CREATE A SUBSET OF ACTIONS THAT DON'T CONTAIN THIS -----

                        List<GAction> subset = ActionSubset(usableActions, action);

            // ---------- RUN THIS FUNCTION AGAIN USING THE SUBSET ----------

                        bool found = BuildGraph(node, leaves, subset, goal);

                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }

            // ------------ RETURN IF WE FOUND AT LEAST ONE PATH ------------

            return foundPath;
        }

        /// <summary>
        /// Returns a new list that doesn't contain the removed action.
        /// </summary>
        private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
        {
            List<GAction> subset = new List<GAction>();

            foreach (GAction a in actions)
            {
                if (!a.Equals(removeMe))
                {
                    subset.Add(a);
                }
            }
            return subset;
        }

        /// <summary>
        /// Check goals against state of the world to determine if the goal has been achieved.
        /// </summary>
        private bool GoalAchieved(Dictionary<State, int> goal, List<State> state)
        {
            // ---- LOOP THROUGH ALL GOALS AND CHECK IF IT EXISTS IN STATE ----

            foreach (KeyValuePair<State, int> g in goal)
            {
                if (!state.Contains(g.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}