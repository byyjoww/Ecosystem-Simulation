using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "TrueDecision", menuName = "Scriptable Object/Pluggable AI/Decision/True")]
    public class TrueDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return true;
        }
    }
}
