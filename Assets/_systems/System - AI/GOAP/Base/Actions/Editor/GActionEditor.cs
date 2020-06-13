using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Elysium.AI.GOAP;
using UnityEditorInternal;

[CustomEditor(typeof(GAction), true)]
public class GActionEditor : Editor
{
    private SerializedProperty list1;
    private SerializedProperty list2;

    private ReorderableList reorderableList1;
    private ReorderableList reorderableList2;

    private void OnEnable()
    {
        //Find the list in our ScriptableObject script.
        list1 = serializedObject.FindProperty("preConditions");
        list2 = serializedObject.FindProperty("postEffects");

        //Create an instance of our reorderable list.
        reorderableList1 = new ReorderableList(serializedObject: serializedObject, elements: list1, draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: true);
        reorderableList2 = new ReorderableList(serializedObject: serializedObject, elements: list2, draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: true);

        //Set up the method callback to draw our list header
        reorderableList1.drawHeaderCallback = DrawHeaderCallback1;
        reorderableList2.drawHeaderCallback = DrawHeaderCallback2;

        //Set up the method callback to draw each element in our reorderable list
        reorderableList1.drawElementCallback = DrawElementCallback1;
        reorderableList2.drawElementCallback = DrawElementCallback2;

        //Set the height of each element.
        reorderableList1.elementHeightCallback += ElementHeightCallback1;
        reorderableList2.elementHeightCallback += ElementHeightCallback2;

        //Set up the method callback to define what should happen when we add a new object to our list.
        reorderableList1.onAddCallback += OnAddCallback;
        reorderableList2.onAddCallback += OnAddCallback;
    }

    /// <summary>
    /// Draws the header for the reorderable list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeaderCallback1(Rect rect)
    {
        EditorGUI.LabelField(rect, "Pre Conditions");
    }

    private void DrawHeaderCallback2(Rect rect)
    {
        EditorGUI.LabelField(rect, "Post Effects");
    }

    /// <summary>
    /// This methods decides how to draw each element in the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isactive"></param>
    /// <param name="isfocused"></param>
    private void DrawElementCallback1(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element1 = reorderableList1.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //We get the name property of our element so we can display this in our list.
        //SerializedProperty elementName = element.FindPropertyRelative("name");
        //string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
        //    ? "New section"
        //    : $"Section: {elementName.stringValue}";

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position: new Rect(rect.x += 10, rect.y, Screen.width * 0.8f, height: EditorGUIUtility.singleLineHeight), property: element1.FindPropertyRelative("key"), label: new GUIContent("Condition:"), includeChildren: true);
    }

    private void DrawElementCallback2(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element2 = reorderableList2.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //We get the name property of our element so we can display this in our list.
        //SerializedProperty elementName = element.FindPropertyRelative("name");
        //string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
        //    ? "New section"
        //    : $"Section: {elementName.stringValue}";

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position: new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property: element2.FindPropertyRelative("key"), label: new GUIContent("Effect:"), includeChildren: true);
    }

    /// <summary>
    /// Calculates the height of a single element in the list.
    /// This is extremely useful when displaying list-items with nested data.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float ElementHeightCallback1(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight = EditorGUI.GetPropertyHeight(reorderableList1.serializedProperty.GetArrayElementAtIndex(index), false);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    private float ElementHeightCallback2(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight = EditorGUI.GetPropertyHeight(reorderableList2.serializedProperty.GetArrayElementAtIndex(index), false);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    /// <summary>
    /// Defines how a new list element should be created and added to our list.
    /// </summary>
    /// <param name="list"></param>
    private void OnAddCallback(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
    }

    /// <summary>
    /// Draw the Inspector Window
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle labelStyle = EditorStyles.boldLabel;
        labelStyle.fontSize = 12;

        string[] excludedStrings = new string[3];
        excludedStrings[0] = "preConditions";
        excludedStrings[1] = "postEffects";
        excludedStrings[2] = "m_Script";

        EditorGUILayout.LabelField("Action Details", labelStyle);
        Editor.DrawPropertiesExcluding(serializedObject, excludedStrings);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Conditions & Effects", labelStyle);
        reorderableList1.DoLayoutList();
        reorderableList2.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
