using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ShopUI))]
public class ShopUIEditor : Editor
{
    private SerializedProperty m_sectionsToInstantiate;
    private ReorderableList m_ReorderableList;

    private void OnEnable()
    {
        //Find the list in our ScriptableObject script.
        m_sectionsToInstantiate = serializedObject.FindProperty("sectionsToInstantiate");

        //Create an instance of our reorderable list.
        m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_sectionsToInstantiate, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);

        //Set up the method callback to draw our list header
        m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

        //Set up the method callback to draw each element in our reorderable list
        m_ReorderableList.drawElementCallback = DrawElementCallback;

        //Set the height of each element.
        m_ReorderableList.elementHeightCallback += ElementHeightCallback;

        //Set up the method callback to define what should happen when we add a new object to our list.
        m_ReorderableList.onAddCallback += OnAddCallback;
    }

    /// <summary>
    /// Draws the header for the reorderable list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Shop Elements");
    }

    /// <summary>
    /// This methods decides how to draw each element in the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isactive"></param>
    /// <param name="isfocused"></param>
    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //We get the name property of our element so we can display this in our list.
        SerializedProperty elementName = element.FindPropertyRelative("name");
        string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
            ? "New section"
            : $"Section: {elementName.stringValue}";

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent(elementTitle), includeChildren: true);
    }

    /// <summary>
    /// Calculates the height of a single element in the list.
    /// This is extremely useful when displaying list-items with nested data.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float ElementHeightCallback(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

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

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tab Elements", labelStyle);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pfTabElement"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tTabElement"));
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Group Elements", labelStyle);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pfPanelElement"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tPanelElement"));
        EditorGUILayout.Space();

        m_ReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Add Section"))
        {
            AddSection();
        }
    }

    public void AddSection()
    {
        ShopUI shopUI = (ShopUI)target;

        shopUI.sectionsToInstantiate.RemoveAll(x => x == null);
        shopUI.sectionsToInstantiate.Add(new ShopUI.ShopSection());
    }
}
