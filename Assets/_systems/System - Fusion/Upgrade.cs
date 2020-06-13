using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Upgrade : MonoBehaviour
{
    [Header("Upgradable")]
    [SerializeField, RequireInterface(typeof(IUpgradable))] private UnityEngine.Object upgradable;
    public IUpgradable Upgradable => upgradable as IUpgradable;

    public bool IsReadyForUpgrade { get { if (IsMaterialReady && IsUpgradableReady) { return true; } else { return false; } } }
    public bool IsMaterialReady { get { if (upgradeProgress >= 100) { return true; } else { return false; } } }
    public bool IsUpgradableReady { get { if (upgradable == null) { return false; } else { return true; } } }

    [Header("Upgrade Events")]
    [SerializeField] private BoolScriptableEvent OnUpgradableSelectChange;
    [SerializeField] private BoolScriptableEvent OnUpgradeReady;
    [SerializeField] private BoolScriptableEvent OnUpgradeSuccessful;

    [Header("Material")]
    [SerializeField, Range(0,100)] private int upgradeProgress;
    public float UpgradeProgress => upgradeProgress;
    [SerializeField] private List<MaterialData> upgradeMaterials = new List<MaterialData>();
    public List<MaterialData> UpgradeMaterials => upgradeMaterials;

    [System.Serializable]
    public class MaterialData
    {
        [SerializeField, RequireInterface(typeof(IUpgradeMaterial))] 
        private UnityEngine.Object mat;
        public IUpgradeMaterial Mat { get => mat as IUpgradeMaterial; }
        public int quantity;

        public MaterialData(IUpgradeMaterial material)
        {
            this.mat = material as UnityEngine.Object;
            quantity = 1;
        }
    }

    [Header("Material Events")]
    [SerializeField] private BoolScriptableEvent OnMaterialSelectChange;

    private void OnEnable()
    {
        DeselectUpgradable();
        UpdateMaterialProgress(true);
    }

    private void OnDisable()
    {
        DeselectUpgradable();
        EmptyMaterial();
    }

    #region UPGRADABLE
    public void DeselectUpgradable()
    {
        this.upgradable = null;
        EmptyMaterial();

        OnUpgradableSelectChange.Raise(false);
        Debug.Log($"Upgradable has been deselected.");
    }

    public bool SelectUpgradable(IUpgradable selectedUpgradable)
    {
        if (selectedUpgradable.Amount <= 0)
        {
            Debug.Log($"Not enough upgradables.");
            return false;
        }

        if (selectedUpgradable.IsMaxLevel)
        {
            Debug.Log($"Upgradable is max level already.");
            return false;
        }

        this.upgradable = selectedUpgradable as UnityEngine.Object;
        OnUpgradableSelectChange.Raise(true);
        OnUpgradeReady.Raise(IsReadyForUpgrade);

        Debug.Log($"Player selected an upgradable.");
        return true;
    }
    #endregion

    #region MATERIAL
    public bool SelectMaterial(IUpgradeMaterial material)
    {
        if (Upgradable == null)
        {
            Debug.Log($"No upgradable selected.");
            return false;
        }

        if (!Upgradable.Materials.Contains(material))
        {
            Debug.Log($"Not a valid material for this upgradable.");
            return false;
        }

        if (UpgradeProgress >= 100)
        {
            Debug.Log($"Already at max materials.");
            return false;
        }

        // Check if material is already in the list
        var matToSelect = upgradeMaterials.SingleOrDefault(x => x.Mat == material);
        if (matToSelect != null)
        {
            if (material.Amount - matToSelect.quantity > 0)
            {
                matToSelect.quantity++;
            }
            else
            {
                Debug.Log($"Insufficient materials left.");
                return false;
            }
        }
        else if (material.Amount > 0)
        {
            upgradeMaterials.Add(new MaterialData(material));
        }
        else
        {
            Debug.Log($"Insufficient materials to create new mat.");
            return false;
        }

        UpdateMaterialProgress(true);
        Debug.Log($"Player selected a material.");
        return true;
    }

    public void DeselectMaterial(IUpgradeMaterial material)
    {
        if (upgradeMaterials.Count <= 0)
        {
            Debug.Log($"Material list is empty, unable to deselect.");
            return;
        }

        if (upgradeMaterials.SingleOrDefault(x => x.Mat == material) == null)
        {
            Debug.Log($"Material list doesn't contain {material}, unable to deselect.");
            return;
        }

        var matToDeselect = upgradeMaterials.Single(x => x.Mat == material);

        if (matToDeselect.quantity <= 0)
        {
            Debug.Log($"{material} quantity is less or equal to 0, unable to deselect.");
            return;
        }

        matToDeselect.quantity--;

        if (matToDeselect.quantity <= 0) 
        {
            upgradeMaterials.Remove(matToDeselect);
        }

        UpdateMaterialProgress(false);
        Debug.Log($"Upgradable has been deselected.");
    }

    private void EmptyMaterial()
    {
        if (upgradeMaterials.Count > 0)
        {
            for (int i = upgradeMaterials.Count -1; i>= 0; i--)
            {
                int m = upgradeMaterials[i].quantity;
                for (int v = 0; v < m; v++)
                {
                    DeselectMaterial(upgradeMaterials[i].Mat as IUpgradeMaterial);
                }
            }
        }
    }

    private void UpdateMaterialProgress(bool unused)
    {
        upgradeProgress = 0;
        foreach (var mat in upgradeMaterials)
        {
            upgradeProgress += (mat.Mat.ProgressScore * mat.quantity);

            if (upgradeProgress > 100)
            {
                upgradeProgress = 100;
            }
        }

        OnMaterialSelectChange.Raise(true);
        OnUpgradeReady.Raise(IsReadyForUpgrade);
    }
    #endregion

    #region UPGRADE_ATTEMPT
    public void UpgradeAttempt()
    {
        if (IsReadyForUpgrade && upgradable != null)
        {
            if (Upgradable.Currency.SpendCurrency(Upgradable.UpgradeCost))
            {
                UpgradeSuccessful();                
            }            
        }
        else
        {
            UpgradeFailed();
        }
    }

    private void UpgradeFailed()
    {
        Debug.Log("Upgrade Failed.");
        OnUpgradeSuccessful.Raise(false);
    }

    private void UpgradeSuccessful()
    {
        //Remove Upgradable
        Upgradable.Amount -= 1;

        //Reward Higher Level Upgradable
        // INSERT HERE

        //Delete Materials
        foreach (var mat in upgradeMaterials)
        {
            mat.Mat.RemoveAmount(mat.quantity);
        }
        upgradeMaterials.Clear();

        UpdateMaterialProgress(true);
        
        DeselectUpgradable();
        OnUpgradeSuccessful.Raise(true);
    }
    #endregion
}