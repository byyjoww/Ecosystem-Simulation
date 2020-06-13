using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Scan Decision", menuName = "Scriptable Object/Pluggable AI/Decision/Scan")]
    public class ScanDecision : Decision
    {
        [SerializeField] private float stateTimeLimitSeconds;

        public override bool Decide(StateController controller)
        {
            bool noEnemyInSight = Scan(controller);
            return noEnemyInSight;
        }

        private bool Scan(StateController controller)
        {
            return controller.RotateInCircles(controller, stateTimeLimitSeconds);
        }
    }
}
