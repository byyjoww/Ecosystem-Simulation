using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Elysium.AI.Navmesh;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Wander Action", menuName = "Scriptable Object/Pluggable AI/Action/Wander")]
    public class WanderAction : Action
    {
        public override void Act(StateController controller)
        {
            Wander(controller);
        }

        public void Wander(StateController controller)
        {
            controller.IsEating = false;
            GetDestination(controller);
            SetDestination(controller);
        }

        public void GetDestination(StateController controller)
        {
            var destinations = controller.DestinationList;
            if (destinations.Count < 2)
            {
                var location = AINavigation.RandomNavSphere(controller.WanderOrigin, controller.WanderDistance, -1, false);                
                destinations.Add(location);
            }

            //if (destinations.Count < Mathf.Infinity)
            //{
            //    var location = AINavigation.Wander2(controller.WanderOrigin, controller.NavMeshAgent, controller.WanderRadius, controller.WanderDistance, controller.WanderJitter);
            //    controller.WanderOrigin = location.Item1;
            //    destinations.Add(location.Item2);
            //}
        }

        public void SetDestination(StateController controller)
        {
            // SET NEXT DESTINATION LOGIC
            controller.NavMeshAgent.destination = controller.DestinationList[0];
            controller.NavMeshAgent.isStopped = false;

            if (controller.NavMeshAgent.remainingDistance <= controller.NavMeshAgent.stoppingDistance && !controller.NavMeshAgent.pathPending)
            {
                controller.DestinationList.Remove(controller.DestinationList[0]);
            }
        }
    }
}