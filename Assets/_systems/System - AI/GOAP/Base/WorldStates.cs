using System.Collections.Generic;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    [System.Serializable]
    public class WorldStates
    {
        // Constructor
        public List<State> states;

        public WorldStates()
        {
            states = new List<State>();
        }

        /************** Helper funtions ****************/

        public bool HasState(State key)
        {
            return states.Contains(key);
        }

        public void AddState(State key)
        {
            states.Add(key);
        }

        public void RemoveState(State key)
        {
            // Check if it frist exists
            if (HasState(key))
            {
                states.Remove(key);
            }
        }

        public List<State> GetStates()
        {
            return states;
        }
    }
}