﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Joystick), true)]
public class JoystickEditor : Editor
{
    private SerializedProperty OnHorizontalDrag;
    private SerializedProperty OnVerticalDrag;
    private SerializedProperty OnTouchStarted;
    private SerializedProperty OnDragging;
    private SerializedProperty OnTouchEnded;
    private SerializedProperty handleRange;
    private SerializedProperty deadZone;
    private SerializedProperty axisOptions;
    private SerializedProperty snapX;
    private SerializedProperty snapY;
    protected SerializedProperty background;
    private SerializedProperty handle;

    protected Vector2 center = new Vector2(0.5f, 0.5f);

    protected virtual void OnEnable()
    {
        OnHorizontalDrag = serializedObject.FindProperty("OnHorizontalDrag");
        OnVerticalDrag = serializedObject.FindProperty("OnVerticalDrag");
        OnTouchStarted = serializedObject.FindProperty("OnTouchStarted");
        OnDragging = serializedObject.FindProperty("OnDragging");
        OnTouchEnded = serializedObject.FindProperty("OnTouchEnded");
        handleRange = serializedObject.FindProperty("handleRange");
        deadZone = serializedObject.FindProperty("deadZone");
        axisOptions = serializedObject.FindProperty("axisOptions");
        snapX = serializedObject.FindProperty("snapX");
        snapY = serializedObject.FindProperty("snapY");
        background = serializedObject.FindProperty("background");
        handle = serializedObject.FindProperty("handle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawActions();
        EditorGUILayout.Space();
        DrawValues();
        EditorGUILayout.Space();
        DrawComponents();

        serializedObject.ApplyModifiedProperties();

        if(handle != null)
        {
            RectTransform handleRect = (RectTransform)handle.objectReferenceValue;
            handleRect.anchorMax = center;
            handleRect.anchorMin = center;
            handleRect.pivot = center;
            handleRect.anchoredPosition = Vector2.zero;
        }
    }

    protected virtual void DrawActions()
    {
        EditorGUILayout.PropertyField(OnHorizontalDrag, new GUIContent("On Horizontal Drag", "Event containing horizontal axis value."));
        EditorGUILayout.PropertyField(OnVerticalDrag, new GUIContent("On Vertical Drag", "Event containing vertical axis value."));
        EditorGUILayout.PropertyField(OnTouchStarted, new GUIContent("On Touch Started", "Event triggered when drag starts, containing a vector3 joystic position value."));
        EditorGUILayout.PropertyField(OnDragging, new GUIContent("On Dragging", "Event triggered during the drag, containing a vector3 joystic position value."));
        EditorGUILayout.PropertyField(OnTouchEnded, new GUIContent("On Touch Ended", "Event triggered when drag ends, containing a vector3 joystic position value."));
    }

    protected virtual void DrawValues()
    {
        EditorGUILayout.PropertyField(handleRange, new GUIContent("Handle Range", "The distance the visual handle can move from the center of the joystick."));
        EditorGUILayout.PropertyField(deadZone, new GUIContent("Dead Zone", "The distance away from the center input has to be before registering."));
        EditorGUILayout.PropertyField(axisOptions, new GUIContent("Axis Options", "Which axes the joystick uses."));
        EditorGUILayout.PropertyField(snapX, new GUIContent("Snap X", "Snap the horizontal input to a whole value."));
        EditorGUILayout.PropertyField(snapY, new GUIContent("Snap Y", "Snap the vertical input to a whole value."));
    }

    protected virtual void DrawComponents()
    {
        EditorGUILayout.ObjectField(background, new GUIContent("Background", "The background's RectTransform component."));
        EditorGUILayout.ObjectField(handle, new GUIContent("Handle", "The handle's RectTransform component."));
    }
}