using UnityEngine;
using Elysium.AI.Navmesh;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Decision", menuName = "Scriptable Object/Pluggable AI/Decision/CheckAttackRange")]
    public class CheckAttackRangeDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            bool targetVisible = CheckIfInAttackRange(controller);
            return targetVisible;
        }

        private bool CheckIfInAttackRange(StateController controller)
        {
            if (controller.ChaseTarget == null )
            {
                return false;
            }

			if (Vector3.Distance(controller.ChaseTarget.position, controller.transform.position) <= controller.AttackRange)
			{
                return true;
			}

            return false;
        }
    }
}
