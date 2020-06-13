using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    public abstract class GAction : ScriptableObject
    {
        public string actionName = "Action";
        public float actionCost = 1.0f;
        public float actionDelay = 0.0f;

        [Header("Interrupted By")]
        public bool GoalPriorityChange = false;
        public bool PredatorDetection = false;
        public bool FoodDetection = false;

        public WorldState[] preConditions;
        public WorldState[] postEffects;

        public abstract bool PrePerform(GAgent gAgent);
        public abstract void RunMainAction(GAgent gAgent);        
        public abstract bool PostPerform(GAgent gAgent);

        public bool IsAchievable()
        {
            return true;
        }

        // Check if the action is achievable given the condition of the world and trying to match with the actions preconditions
        public bool IsAchievableBasedOnWorldConditions(List<State> conditions)
        {
            foreach (WorldState p in preConditions)
            {
                if (!conditions.Contains(p.key))
                {
                    return false;
                }
            }
            return true;
        }
    }
}