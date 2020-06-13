using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupSystem", menuName = "Scriptable Object/Popup/PopupSystem")]
public class PopupSystem : ScriptableObject
{
    [System.Serializable]
    public class PopupData
    {
        public enum Style { TITLE_TEXT = 0, TUTORIAL_POPUP = 1 }
        public Style PopupType;
        public GameObject prefab;

        public void CreatePopup(Popup popup)
        {
            var obj = Instantiate(prefab);
            obj.GetComponent<IPopup>().Setup(popup);
        }
    }

    public List<PopupData> popups = new List<PopupData>();
}
