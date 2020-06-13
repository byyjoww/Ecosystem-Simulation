using UnityEngine;
using System;

namespace PluggableAI
{
    [Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;
    }
}

