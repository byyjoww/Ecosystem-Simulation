using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Chase Action", menuName = "Scriptable Object/Pluggable AI/Action/Chase")]
    public class ChaseAction : Action
    {
        public override void Act(StateController controller)
        {
            Chase(controller);
        }

        public void Chase(StateController controller)
        {
            StartChase(controller);
        }

        /// <summary>
        /// Function that will start chasing the opponent vision.
        /// </summary>
        public void StartChase(StateController controller)
        {
            if (controller.ChaseTarget == null)
            {
                return;
            }

            controller.NavMeshAgent.destination = controller.ChaseTarget.position;
            controller.NavMeshAgent.isStopped = false;
        }
    }
}
