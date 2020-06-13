using UnityEngine;
using Elysium.AI.GOAP;
using Elysium.AI.Navmesh;

namespace Elysium.AI.GOAP
{
    [CreateAssetMenu(menuName = "Scriptable Object/GOAP/Action/Flee")]
    public class Flee : GAction
    {        
        public override bool PrePerform(GAgent gAgent)
        {
            var herbivore = gAgent as Herbivore;
            if (herbivore.PredatorTargets.Count > 0)
            {
                return true;
            }

            return false;
        }

        public override void RunMainAction(GAgent gAgent)
        {
            var herbivore = gAgent as Herbivore;
            var pos = AINavigation.Flee(herbivore.PredatorTargets[0].position, gAgent.agent, 40f);

            var data = new GStateData(gAgent.controller, gAgent.agent, pos, gAgent.anim, "run", 2f);

            Debug.Log("Setting navigation course to: " + pos);
            gAgent.controller.Move(data);
        }

        public override bool PostPerform(GAgent gAgent)
        {
            return true;
        }
    }
}