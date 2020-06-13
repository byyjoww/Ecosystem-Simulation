using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SetupTitleTextPopup : MonoBehaviour, IPopup
{
    [SerializeField] TMP_Text titleTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(Popup popup)
    {
        titleTextComponent.text = popup.title;
        descriptionTextComponent.text = popup.description;

        foreach (var trigger in popup.Nodes)
        {
            buttonComponent.onClick.AddListener(delegate { trigger.TriggerAction?.Invoke(); });
        }
    }
}
