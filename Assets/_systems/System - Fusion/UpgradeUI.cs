using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Upgrade))]
public class UpgradeUI : MonoBehaviour, IFillable
{
    [Header("Components")]
    [SerializeField] private Upgrade upgrade;
    [SerializeField] private Image mainUnitImageComponent;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Button upgradeButton;

    [Header("Layout Elements")]
    [SerializeField] private GameObject pfMaterialElement;
    [SerializeField] private Transform tMaterialElement;
    [SerializeField] private List<GameObject> layoutElements = new List<GameObject>();

    [Header("Upgrade Events")]
    [SerializeField] private BoolScriptableEvent OnUpgradableSelectChange;
    [SerializeField] private BoolScriptableEvent OnUpgradeSucessful;
    [SerializeField] private BoolScriptableEvent OnUpgradeReady;

    [Header("Material Events")]
    [SerializeField] private BoolScriptableEvent OnMaterialSelectChange;

    //------------------------Interface------------------------//
    public event Action OnFillValueChanged;
    public float CurrentFill => upgrade.UpgradeProgress;
    public float MaxFill => 100;

    //---------------------------------------------------------//

    private void OnEnable()
    {
        OnUpgradableSelectChange.OnRaise += CheckSelectStatus;
        OnUpgradeReady.OnRaise += ChangeUpgradeStatus;
        OnUpgradeSucessful.OnRaise += ChangeUpgradeStatus;
        OnMaterialSelectChange.OnRaise += UpdateMaterialProgress;
        OnMaterialSelectChange.OnRaise += UpdateMaterialUI;
    }

    private void Start()
    {
        UpdateMaterialProgress(true);
        UpdateMaterialUI(true);
    }

    private void OnDisable()
    {
        OnUpgradableSelectChange.OnRaise -= CheckSelectStatus;
        OnUpgradeReady.OnRaise -= ChangeUpgradeStatus;
        OnUpgradeSucessful.OnRaise -= ChangeUpgradeStatus;
        OnMaterialSelectChange.OnRaise -= UpdateMaterialProgress;
        OnMaterialSelectChange.OnRaise -= UpdateMaterialUI;
    }

    private void CheckSelectStatus(bool selected)
    {
        if (selected)
        {
            SelectUpgradable();
        }
        else
        {
            DeselectUpgradable();
        }
    }

    #region UPGRADABLES
    private void DeselectUpgradable()
    {
        if(upgrade.Upgradable == null)
        {
            mainUnitImageComponent.sprite = defaultSprite;
        }
    }

    private void SelectUpgradable()
    {
        mainUnitImageComponent.sprite = upgrade.Upgradable.Sprite;
    }

    private void UpdateMaterialProgress(bool unused)
    {
        print("OnFillValueChanged Invoked");
        OnFillValueChanged?.Invoke();
    }

    private void ChangeUpgradeStatus(bool isReady)
    {
        upgradeButton.interactable = upgrade.IsReadyForUpgrade;
    }
    #endregion

    #region MATERIALS

    private void UpdateMaterialUI(bool unused)
    {
        foreach (var element in layoutElements)
        {
            Destroy(element);
        }

        layoutElements.Clear();

        foreach (var material in upgrade.UpgradeMaterials)
        {
            var matByName = layoutElements.SingleOrDefault(x => x.name == (material.Mat as ScriptableObject).name);
            if (matByName == null)
            {
                var obj = Instantiate(pfMaterialElement, tMaterialElement);
                obj.name = (material.Mat as ScriptableObject).name;
                obj.GetComponent<SetupClickableImage>().Setup(material.Mat.Sprite, material.quantity, () => upgrade.DeselectMaterial(material.Mat));
                layoutElements.Add(obj);
            }
            else
            {
                matByName.GetComponent<SetupClickableImage>().AdjustCount(material.quantity);
            }
        }
    }
    #endregion

    private void OnValidate()
    {
        if (upgrade == null) upgrade = GetComponent<Upgrade>();
    }
}