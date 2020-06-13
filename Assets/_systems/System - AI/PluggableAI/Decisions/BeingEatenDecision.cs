using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "BeingEatenDecision", menuName = "Scriptable Object/Pluggable AI/Decision/BeingEaten")]
    public class BeingEatenDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.IsBeingEaten;
        }
    }
}
