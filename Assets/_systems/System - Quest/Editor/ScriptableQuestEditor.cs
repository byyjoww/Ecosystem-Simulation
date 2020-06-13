using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableQuest))]
public class ScriptableQuestEditor : AssetContainerEditor<QuestObjective>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScriptableQuest scriptableQuest = (ScriptableQuest)target;

        serializedObject.Update();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Complete Quest"))
        {
            scriptableQuest.ForceCompleteQuest();
        }

        if (GUILayout.Button("Turn In Quest"))
        {
            scriptableQuest.HandInQuest();
        }

        if (GUILayout.Button("Reset Quest"))
        {
            scriptableQuest.ResetQuest();
        }
        GUILayout.EndHorizontal();
    }
}