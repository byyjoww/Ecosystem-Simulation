using UnityEngine;
using Elysium.AI.Navmesh;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Decision", menuName = "Scriptable Object/Pluggable AI/Decision/Look")]
    public class SearchForTargetDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            if (!controller.IsHungry)
            {
                return false;
            }

            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(StateController controller)
        {
            var possibleTargets = AINavigation.LocateTargetBlind(controller.transform, controller.ViewSphereRadius, controller.ViewAngle);

            for (int i = possibleTargets.Count; i > 0; i--)
            {
                var v = possibleTargets[i - 1].GetComponent<IEatable>();

                if (v == null || v.FoodChainPosition != controller.Eater.Eats || possibleTargets[i - 1].gameObject == controller.gameObject)
                {
                    possibleTargets.RemoveAt(i - 1);
                }
            }

            if (possibleTargets != null && possibleTargets.Count > 0)
            {
                controller.ChaseTarget = possibleTargets[0];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
