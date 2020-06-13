using UnityEngine;
using Elysium.AI.GOAP;
using Elysium.AI.Navmesh;

namespace Elysium.AI.GOAP
{
    [CreateAssetMenu(menuName = "Scriptable Object/GOAP/Action/Wander")]
    public class Wander : GAction
    {        
        public override bool PrePerform(GAgent gAgent)
        {
            return true;
        }

        public override void RunMainAction(GAgent gAgent)
        {
            Vector3 locationTarget = Vector3.zero;

            locationTarget = AINavigation.RandomNavSphere(gAgent.transform.position, gAgent.ViewSphereRadius + 20, -1, false);

            var data = new GStateData(gAgent.controller, gAgent.agent, locationTarget, gAgent.anim, "walk", 2f);

            Debug.Log("Setting navigation course to: " + locationTarget);
            gAgent.GenerateLocationGizmo(locationTarget);
            gAgent.controller.Move(data);
        }

        public override bool PostPerform(GAgent gAgent)
        {
            return true;
        }
    }
}