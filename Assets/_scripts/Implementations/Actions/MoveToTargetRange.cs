using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    [CreateAssetMenu(menuName = "Scriptable Object/GOAP/Action/MoveToTargetRange")]
    public class MoveToTargetRange : GAction
    {        
        public override bool PrePerform(GAgent gAgent)
        {
            var herbivore = gAgent as Herbivore;
            if (herbivore.FoodTargets.Count > 0)
            {
                gAgent.target = herbivore.FoodTargets[0];
                return true;
            }

            return false;
        }

        public override void RunMainAction(GAgent gAgent)
        {
            var data = new GStateData(gAgent.controller, gAgent.agent, gAgent.target, gAgent.anim, "walk", 2f);

            Debug.Log("Setting navigation course to: " + gAgent.target.gameObject.name);
            gAgent.controller.Move(data);
        }

        public override bool PostPerform(GAgent gAgent)
        {
            return true;
        }
    }
}