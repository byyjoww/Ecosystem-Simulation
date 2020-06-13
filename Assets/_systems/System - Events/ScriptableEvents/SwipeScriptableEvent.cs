using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewQuadrantEvent", menuName = "Scriptable Object/Scriptable Events/Complex/Quadrant Event", order = 1)]
public class SwipeScriptableEvent : GenericScriptableEvent<Vector2, Vector2, SwipeInputController.Quadrant>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField] Vector2 editorData1;
    [SerializeField] Vector2 editorData2;
    [SerializeField] SwipeInputController.Quadrant editorData3;

    [CustomEditor(typeof(SwipeScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as SwipeScriptableEvent);
                e.Raise(e.editorData1, e.editorData2, e.editorData3);
            }
        }
    }
#endif
}
