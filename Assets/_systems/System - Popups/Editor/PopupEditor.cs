using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Popup))]
public class PopupEditor : AssetContainerEditor<BaseTrigger>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Popup Popup = (Popup)target;

        serializedObject.Update();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();
    }
}