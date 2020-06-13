using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSystem", menuName = "Scriptable Object/Quest/Quest System")]
public class QuestSystem : ScriptableObject, IInitializable
{
    [Header("Active Quests")]
    [SerializeField] private List<ScriptableQuest> allQuests = new List<ScriptableQuest>();

    private bool initialized;
    public bool Initialized => initialized;

    public void Init()
    {
        foreach (var quest in allQuests)
        {
            quest.Initialize();
        }

        initialized = true;
    }

    public void ReceiveQuest(ScriptableQuest quest)
    {
        Debug.Log($"Player received the quest: {quest.Title}.");
        quest.GetQuest();
    }

    private void OnValidate()
    {
        allQuests = Tools.GetAllScriptableObjects<ScriptableQuest>().ToList();
        allQuests.RemoveAll(x => x == null);
    }
}