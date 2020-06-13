using System;
using System.IO;
using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    [SerializeField, ReadOnly] protected bool IsActive = false;
    [SerializeField] protected SavableBoolValue isObjectiveComplete;
    public bool IsObjectiveComplete => isObjectiveComplete.Value;

    public event Action OnObjectiveStatusChange;

    public void ActivateObjective()
    {
        IsActive = true;
        CheckObjective();
    }

    public abstract void CheckObjective();

    public virtual void ResetObjective()
    {        
        isObjectiveComplete.Value = false;
        IsActive = false;        
    }

    public void CompleteObjective()
    {
        if (!IsActive)
        {
            return;
        }

        isObjectiveComplete.Value = true;
        OnObjectiveStatusChange?.Invoke();
    }

    public void IncompleteObjective()
    {
        if (!IsActive)
        {
            return;
        }

        isObjectiveComplete.Value = false;
        OnObjectiveStatusChange?.Invoke();
    }
}