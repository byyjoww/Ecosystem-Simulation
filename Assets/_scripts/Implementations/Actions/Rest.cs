using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    [CreateAssetMenu(menuName = "Scriptable Object/GOAP/Action/Rest")]
    public class Rest : GAction
    {        
        public override bool PrePerform(GAgent gAgent)
        {
            return true;
        }

        public override void RunMainAction(GAgent gAgent)
        {
            var herbivore = gAgent as Herbivore;

            Debug.Log("Resting.");

            var data = new GStateData(gAgent.controller, gAgent.agent, gAgent.transform.position, gAgent.anim, "idle", 5f);
            gAgent.controller.Activity(data);

            herbivore.Exhausted.LoseExhaustion(50, true);
        }

        public override bool PostPerform(GAgent gAgent)
        {
            return true;
        }
    }
}