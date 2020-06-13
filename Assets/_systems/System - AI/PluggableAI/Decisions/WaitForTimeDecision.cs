using System.Timers;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(fileName = "New Active State Decision", menuName = "Scriptable Object/Pluggable AI/Decision/WaitForTime")]
    public class WaitForTimeDecision : Decision
    {
        [SerializeField] float exitTime;

        public override bool Decide(StateController controller)
        {
            if (controller.ExitTime)
            {
                controller.ExitTime = false;
                return true;
            }

            if (!controller.HasExitTimer)
            {
                Timer timer = new Timer
                {
                    Interval = exitTime * 1000,
                    Enabled = true
                };

                timer.Elapsed += (Object, ElapsedEventArgs) => ExitState(controller, timer);
                controller.HasExitTimer = true;
            }

            return false;
        }

        private void ExitState(StateController controller, Timer timer)
        {
            controller.ExitTime = true;
            controller.HasExitTimer = false;
            timer.Dispose();
        }
    }
}
