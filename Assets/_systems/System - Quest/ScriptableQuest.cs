using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Object/Quest/Quest")]
public class ScriptableQuest : AssetContainer<QuestObjective>
{
    [Header("Quest Details")]
    [SerializeField] private string title;
    public string Title => title;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private bool isAutoComplete;
    [SerializeField] private SavableBoolValue isActive;
    [SerializeField, ReadOnly] private bool isQuestComplete;
    [SerializeField, ReadOnly] private bool isInitialized;

    [SerializeField] private UnityEvent OnQuestComplete;

    public List<QuestObjective> objectives => Nodes;

    #region INITIALIZE
    public void Initialize()
    {
        if (!isActive.Value)
        {
            Debug.Log($"Quest {title} isn't active, initialization skipped.");
            return;
        }

        foreach (var objective in objectives)
        {
            objective.OnObjectiveStatusChange += CheckObjectives;
            objective.ActivateObjective();
        }

        this.isInitialized = true;
        Debug.Log($"Quest {title} has been initialized.");

        CheckObjectives();
    }

    private void Terminate()
    {
        foreach (var objective in objectives)
        {
            objective.OnObjectiveStatusChange -= CheckObjectives;
        }

        isInitialized = false;

        Debug.Log($"Quest {title} has been terminated.");
    }
    #endregion

    public void GetQuest()
    {
        isActive.Value = true;
        Debug.Log($"Quest {title} was activated.");

        if (!isInitialized)
        {            
            Initialize();
        }
        else
        {
            Debug.LogError($"Quest {title} was initialized before starting!");
        }        
    }

    private void CheckObjectives()
    {
        if (!isInitialized)
        {
            return;
        }

        foreach (var objective in objectives)
        {
            if (!objective.IsObjectiveComplete)
            {
                isQuestComplete = false;
                return;
            }
        }

        Debug.Log($"All conditions for quest '{title}' have been met. Quest is marked as complete.");
        isQuestComplete = true;

        if (isAutoComplete) HandInQuest();
    }

    public void ForceCompleteQuest()
    {
        Debug.LogError($"Warning, attempting to force complete quest {title}! This should only be used for debugging purposes!");

        //if (!isActive.Value)
        //{
        //    isActive.Value = true;
        //    Initialize();
        //}

        //foreach (var objective in objectives)
        //{
        //    objective.CompleteObjective();
        //}
    }

    public void HandInQuest()
    {
        if (!isQuestComplete || !isActive.Value)
        {
            Debug.Log($"Failed to turn in Quest {title} | Active: {isActive.Value} | Completed: {isQuestComplete}.");
            return;
        }

        Debug.Log($"Quest {title} has been handed in.");
        OnQuestComplete?.Invoke();
        ResetQuest();        
    }

    public void ResetQuest()
    {
        foreach (var objective in objectives)
        {
            objective.ResetObjective();
        }

        isQuestComplete = false;
        isActive.Value = false;

        Debug.Log($"Quest {title} has been reset.");
        Terminate();        
    }

    //private void OnDisable()
    //{
    //    Terminate();
    //}
}