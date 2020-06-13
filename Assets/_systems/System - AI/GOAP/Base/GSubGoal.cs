using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Elysium.AI.GOAP;
using System;

namespace Elysium.AI.GOAP
{
    public class SubGoal
    {
        public State goalName;

        // ---------- USELESS DICTIONARY -----------

        public Dictionary<State, int> sGoals;

        // ----- REMOVE THE GOAL ONCE COMPLETE -----

        public bool remove;

        // -------------- CONSTRUCTOR --------------

        public SubGoal(State s, int i, bool r)
        {
            sGoals = new Dictionary<State, int>();
            sGoals.Add(s, i);
            remove = r;
            this.goalName = s;
        }
    }
}