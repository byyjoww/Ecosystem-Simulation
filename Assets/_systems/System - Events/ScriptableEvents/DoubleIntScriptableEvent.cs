using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewDoubleIntEvent", menuName = "Scriptable Object/Scriptable Events/Multi Primitive/Double Int Event", order = 1)]
public class DoubleIntScriptableEvent : GenericScriptableEvent<int, int>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField] int editorData1, editorData2;
    
    [CustomEditor(typeof(DoubleIntScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as DoubleIntScriptableEvent);
                e.Raise(e.editorData1, e.editorData2);
            }
        }
    }
#endif
}
