using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseTrigger : ScriptableObject
{
    private Action triggerAction;
    public Action TriggerAction => triggerAction;

    private void OnEnable()
    {
        triggerAction = SetTrigger;
    }

    protected abstract void SetTrigger();
}