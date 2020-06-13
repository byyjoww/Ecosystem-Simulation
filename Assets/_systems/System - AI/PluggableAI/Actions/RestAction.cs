using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Rest Action", menuName = "Scriptable Object/Pluggable AI/Action/Rest")]
    public class RestAction : Action
    {
        public override void Act(StateController controller)
        {
            Rest(controller);
        }

        public void Rest(StateController controller)
        {
            controller.NavMeshAgent.isStopped = true;
            controller.Exhaustion.LoseExhaustion(1, false);
        }
    }
}
