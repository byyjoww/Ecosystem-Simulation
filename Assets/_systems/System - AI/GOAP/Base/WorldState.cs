using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    [System.Serializable]
    public class WorldState
    {        
        public WorldState(State key)
        {
            this.key = key;
        }

        public State key;
    }
}