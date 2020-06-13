using UnityEngine;
using Elysium.AI.GOAP;

namespace Elysium.AI.GOAP
{
    [CreateAssetMenu(menuName = "Scriptable Object/GOAP/Action/Eat")]
    public class Eat : GAction
    {        
        public override bool PrePerform(GAgent gAgent)
        {
            if (gAgent.target != null)
            {
                if(Vector3.Distance(gAgent.transform.position, gAgent.target.position) < 5f)
                {
                    return true;
                }                
            }

            return false;
        }

        public override void RunMainAction(GAgent gAgent)
        {
            Debug.Log("Eating.");

            var data = new GStateData(gAgent.controller, gAgent.agent, gAgent.target.position, gAgent.anim, "idle", 2f);
            gAgent.controller.Activity(data);

            IEatable food = gAgent.target.GetComponent<IEatable>();

            (gAgent as Herbivore).Eater.Eat(food);
        }

        public override bool PostPerform(GAgent gAgent)
        {
            return true;
        }        
    }
}