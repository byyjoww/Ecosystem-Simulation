using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Popup", menuName = "Scriptable Object/Popup/Popup")]
public class Popup : AssetContainer<BaseTrigger>
{
    [Header("Popup System")]
    [SerializeField] private PopupSystem popupSystem;

    [Header("Popup Details")]
    public string title;
    public string description;
    public Sprite sprite;
    [SerializeField] PopupSystem.PopupData.Style popupType;

    public List<BaseTrigger> objectives => Nodes;    

    public void Generate()
    {
        popupSystem.popups.Single(x => x.PopupType == popupType).CreatePopup(this);
    }
}
