using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewIInventoryElementEvent", menuName = "Scriptable Object/Scriptable Events/Complex/Inventory Element Event", order = 1)]
public class IInventoryScriptableEvent : GenericScriptableEvent<List<IInventoryElement>>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    List<IInventoryElement> editorData;

    [CustomEditor(typeof(IInventoryScriptableEvent), true)]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as IInventoryScriptableEvent);
                e.Raise(e.editorData);
            }

            if (GUILayout.Button("RequestRaise"))
            {
                var e = (target as IInventoryScriptableEvent);
                e.RequestRaise();
            }
        }
    }
#endif
}
