using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "IsExhaustedDecision", menuName = "Scriptable Object/Pluggable AI/Decision/IsExhausted")]
    public class IsExhaustedDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.IsExhausted;
        }
    }
}
