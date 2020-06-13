using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GestureRecognizer))]
public class TouchInputController : MonoBehaviour
{
    [Header("Touch Events")]
    public Vector3ScriptableEvent OnSingleTap;
    public Vector3ScriptableEvent OnDoubleTap;

    #region INITIALIZE
    [Header("Raw Input")]
    [SerializeField] public GestureRecognizer gestureRecognizer;
    private bool isInitialized = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;

        gestureRecognizer.Tapped += SingleTap;
        gestureRecognizer.DoubleTapped += DoubleTap;

        Debug.Log("TouchInputController Initialized");
    }

    private void OnValidate()
    {
        gestureRecognizer = gestureRecognizer ?? GetComponent<GestureRecognizer>();
    }
    #endregion

    #region SINGLE_TAP
    void SingleTap(Vector2 pos)
    {
        Debug.Log($"Single Tap: {pos}");
        if (OnSingleTap) OnSingleTap.Raise(pos);
        else Debug.LogError("OnSingleTap not assigned!");
    }
    #endregion

    #region DOUBLE_TAP
    void DoubleTap(Vector2 pos)
    {
        Debug.Log($"Double Tap: {pos}");
        if (OnDoubleTap) OnDoubleTap.Raise(pos);
        else Debug.LogError("OnDoubleTap not assigned!");
    }
    #endregion

}
