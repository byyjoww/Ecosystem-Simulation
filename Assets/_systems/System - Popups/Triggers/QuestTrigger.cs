using UnityEngine;

public class QuestTrigger : BaseTrigger
{
    //Quest Trigger
    [Header("Quest Trigger")]
    public QuestSystem questSystem;
    public ScriptableQuest questToStart;

    protected override void SetTrigger()
    {
        Debug.Log($"Constructor from Button Trigger assigned player the quest: {questToStart.Title}.");
        questSystem.ReceiveQuest(questToStart);
    }
}
