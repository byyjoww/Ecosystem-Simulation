using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/Blur Panel")]
public class BlurPanel : Image
{
    public float time = 0.5f;
    public float delay = 0f;

    CanvasGroup canvas;

#if UNITY_EDITOR
    protected override void Reset()
    {
        color = Color.black * 0.1f;
    }
#endif

    private void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        if (Application.isPlaying)
        {
            material.SetFloat("_Size", 0);
            canvas = GetComponent<CanvasGroup>();
            canvas.alpha = 0;
            LeanTween.value(gameObject, UpdateBlur, 0, 1, time).setDelay(delay);
        }
    }

    private void UpdateBlur(float value)
    {
        material.SetFloat("_Size", value);
        canvas.alpha = value;
    }
}
