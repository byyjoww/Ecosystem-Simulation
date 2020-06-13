using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupTutorialPopup : MonoBehaviour, IPopup
{
    [SerializeField] TMP_Text titleTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public void Setup(Popup popup)
    {
        titleTextComponent.text = popup.title;
        descriptionTextComponent.text = popup.description;
        imageComponent.sprite = popup.sprite;
        foreach (var trigger in popup.Nodes)
        {
            buttonComponent.onClick.AddListener(delegate { trigger.TriggerAction?.Invoke(); });
        }        
    }
}
