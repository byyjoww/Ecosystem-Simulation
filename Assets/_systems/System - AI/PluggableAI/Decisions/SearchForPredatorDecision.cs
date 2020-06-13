using UnityEngine;
using System.Collections.Generic;
using Elysium.AI.Navmesh;
using System.IO;
using UnityEngine.AI;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Decision", menuName = "Scriptable Object/Pluggable AI/Decision/SearchForPredator")]
    public class SearchForPredatorDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(StateController controller)
        {
            bool b = true;

            List<Transform> possibleTargets;

            if (controller.FleeTarget == null)
            {
                possibleTargets = AINavigation.LocateTargetBlind(controller.transform, controller.ViewSphereRadius, controller.ViewAngle);
            }
            else
            {
                possibleTargets = AINavigation.LocateTargetBlind(controller.transform, controller.ViewSphereRadius);
            }            

            for (int i = possibleTargets.Count; i > 0; i--)
            {
                var v = possibleTargets[i - 1].GetComponent<IEater>();

                if (v == null || v.Eats != controller.Eatable.FoodChainPosition || possibleTargets[i - 1].gameObject == controller.gameObject)
                {
                    possibleTargets.RemoveAt(i - 1);
                }
            }

            // CONCLUSION

            if (possibleTargets != null && possibleTargets.Count > 0)
            {
                controller.FleeTarget = possibleTargets[0];
                return true;
            }

            if (controller.FleeTarget != null && controller.DestinationList.Count == 0)
            {
                return true;
            }

            controller.FleeTarget = null;
            return false;
        }

        public bool NavMeshDone(NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
