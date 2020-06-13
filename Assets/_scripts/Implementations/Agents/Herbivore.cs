using Elysium.AI.GOAP;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : GAgent 
{
    public IEater Eater;
    public IEatable Eatable;
    public Exhaustion Exhausted;
    public UnitDetection Detection;

    public bool PredatorDetected { get; set; }
    public bool FoodDetected { get; set; }

    public List<Transform> FoodTargets;
    public List<Transform> PredatorTargets;

    protected override void Start() 
    {
        FoodTargets = new List<Transform>();
        PredatorTargets = new List<Transform>();

        base.Start();
        PredatorDetected = false;
        FoodDetected = false;

        Detection = GetComponent<UnitDetection>();
        Eater = GetComponent<IEater>();
        Eatable = GetComponent<IEatable>();
        Exhausted = GetComponent<Exhaustion>();

        SubGoal s1 = new SubGoal(State.NotHungry, 1, false);
        // Add it to the goals
        goals.Add(s1, 0);

        SubGoal s2 = new SubGoal(State.NotExhausted, 1, false);
        // Add it to the goals
        goals.Add(s2, 0);

        SubGoal s3 = new SubGoal(State.NotEaten, 1, false);
        // Add it to the goals
        goals.Add(s3, 2);

        Detection.OnTargetsUpdated += FilterEatableTargets;
        Detection.OnTargetsUpdated += FilterEaterTargets;
    }

    private void Update()
    {
        if (currentAction != null && currentAction.PredatorDetection && PredatorDetected) { ForceNewPlan(); Debug.LogError("Predator detected. Forced new plan"); }
        if (currentAction != null && currentAction.FoodDetection && FoodDetected) { ForceNewPlan(); Debug.LogError("Food detected. Forced new plan"); }

        if (Exhausted.CurrentFill <= 5) ChangePriority(State.NotExhausted, 0);
        else if (Exhausted.CurrentFill > 5 && Exhausted.CurrentFill <= 25) ChangePriority(State.NotExhausted, 1);
        else if (Exhausted.CurrentFill > 25 && Exhausted.CurrentFill <= 50) ChangePriority(State.NotExhausted, 2);
        else if (Exhausted.CurrentFill > 50 && Exhausted.CurrentFill <= 75) ChangePriority(State.NotExhausted, 3);
        else if (Exhausted.CurrentFill > 75 && Exhausted.CurrentFill < 100) ChangePriority(State.NotExhausted, 4);
        else if (Exhausted.CurrentFill == 100) ChangePriority(State.NotExhausted, 10);

        if ((Eater as Predator).CurrentFill <= 5) ChangePriority(State.NotHungry, 0);
        else if ((Eater as Predator).CurrentFill > 5 && (Eater as Predator).CurrentFill <= 25) ChangePriority(State.NotHungry, 1);
        else if ((Eater as Predator).CurrentFill > 25 && (Eater as Predator).CurrentFill <= 50) ChangePriority(State.NotHungry, 2);
        else if ((Eater as Predator).CurrentFill > 50 && (Eater as Predator).CurrentFill <= 75) ChangePriority(State.NotHungry, 3);
        else if ((Eater as Predator).CurrentFill > 75 && (Eater as Predator).CurrentFill < 100) ChangePriority(State.NotHungry, 4);
        else if ((Eater as Predator).CurrentFill == 100) ChangePriority(State.NotHungry, 10);

        if (PredatorTargets.Count > 0) ChangePriority(State.NotEaten, 11);
        if (PredatorTargets.Count == 0) ChangePriority(State.NotEaten, 0);
    }

    public void FilterEatableTargets(List<Transform> targets)
    {
        if (targets == null) { return; }

        var t = new List<Transform>(targets);

        for (int i = t.Count; i > 0; i--)
        {
            var v = t[i - 1].GetComponent<IEatable>();

            if (v == null || v.FoodChainPosition != Eater.Eats || t[i - 1].gameObject == controller.gameObject)
            {
                t.RemoveAt(i - 1);
            }
        }

        if (t.Count < 1)
        {
            if (beliefs.HasState(State.HasTarget)) { beliefs.RemoveState(State.HasTarget); }

            FoodDetected = false;
            FoodTargets.Clear();
            return;
        }

        if (!beliefs.HasState(State.HasTarget)) { beliefs.AddState(State.HasTarget); }
        
        FoodDetected = true;
        FoodTargets = t;
    }

    public void FilterEaterTargets(List<Transform> targets)
    {
        if (targets == null) { return; }        

        var t = new List<Transform>(targets);

        for (int i = t.Count; i > 0; i--)
        {
            var v = t[i - 1].GetComponent<IEater>();

            if (v == null || v.Eats != Eatable.FoodChainPosition || t[i - 1].gameObject == controller.gameObject)
            {
                t.RemoveAt(i - 1);
            }
        }

        if (t.Count < 1)
        {
            PredatorDetected = false;
            PredatorTargets.Clear();
            return;
        }

        PredatorDetected = true;
        PredatorTargets = t;
    }
}