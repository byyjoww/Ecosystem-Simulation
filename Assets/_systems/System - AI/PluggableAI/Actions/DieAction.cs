using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Die Action", menuName = "Scriptable Object/Pluggable AI/Action/Die")]
    public class DieAction : Action
    {
        public override void Act(StateController controller)
        {
            Die(controller);
        }

        public void Die(StateController controller)
        {
            controller.NavMeshAgent.isStopped = true;
            Destroy(controller.gameObject, 1f);
        }
    }
}
