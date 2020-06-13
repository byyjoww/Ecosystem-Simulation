using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Active State Decision", menuName = "Scriptable Object/Pluggable AI/Decision/CheckAlive")]
    public class CheckAliveDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            bool chaseTargetIsActive = CheckIsActive(controller);
            return chaseTargetIsActive;
        }

        private bool CheckIsActive(StateController controller)
        {
            bool isAlive = false;

            if(controller.ChaseTarget != null)
            {
                isAlive = true;
            }

            return isAlive;
        }
    }
}
