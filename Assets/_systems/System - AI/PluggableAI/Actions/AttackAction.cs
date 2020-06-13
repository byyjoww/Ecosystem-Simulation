using System.Collections;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Scriptable Object/Pluggable AI/Action/Attack")]
    public class AttackAction : Action
    {
        public override void Act(StateController controller)
        {
            if (controller.IsEating)
            {
                return;
            }

            Attack(controller);
        }

        public void Attack(StateController controller)
        {
            controller.IsEating = true;
            controller.NavMeshAgent.isStopped = true;

            IEatable food = controller.ChaseTarget.GetComponent<IEatable>();
            Debug.Log($"{controller.gameObject.name} is eating {food}.");            
            
            controller.Eater.Eat(food);
            controller.ChaseTarget = null;
        }
    }
}
