using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.Navmesh;
using UnityEngine.AI;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Flee Action", menuName = "Scriptable Object/Pluggable AI/Action/Flee")]
    public class FleeAction : Action
    {
        public override void Act(StateController controller)
        {
            Flee(controller);
        }

        public void Flee(StateController controller)
        {
            GetDestination(controller);
            SetDestination(controller);            
        }

        public void GetDestination(StateController controller)
        {
            controller.DestinationList.Clear();

            if (controller.FleeTarget == null)
            {
                return;
            }

            var pos = AINavigation.Flee(controller.FleeTarget.position, controller.NavMeshAgent, 20f);
            controller.DestinationList.Add(pos);
        }

        public void SetDestination(StateController controller)
        {
            if (controller.DestinationList.Count < 1)
            {
                return;
            }

            // SET NEXT DESTINATION LOGIC
            NavMeshPath path = new NavMeshPath();
            controller.NavMeshAgent.CalculatePath(controller.DestinationList[0], path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                controller.NavMeshAgent.SetDestination(path.corners[path.corners.Length - 1]);
            }

            if (controller.NavMeshAgent.remainingDistance <= controller.NavMeshAgent.stoppingDistance && !controller.NavMeshAgent.pathPending)
            {
                controller.DestinationList.Remove(controller.DestinationList[0]);
            }

            controller.NavMeshAgent.isStopped = false;
        }
    }
}
