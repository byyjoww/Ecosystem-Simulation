using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelector : MonoBehaviour
{
    public Upgrade upgrade;

    [RequireInterface(typeof(IUpgradable))] public ScriptableObject upgradable;
    [RequireInterface(typeof(IUpgradeMaterial))] public ScriptableObject material1;
    [RequireInterface(typeof(IUpgradeMaterial))] public ScriptableObject material2;
    [RequireInterface(typeof(IUpgradeMaterial))] public ScriptableObject material3;

    public void SelectUpgradable()
    {
        upgrade.SelectUpgradable(upgradable as IUpgradable);
    }

    public void SelectMaterial1()
    {
        upgrade.SelectMaterial(material1 as IUpgradeMaterial);
    }

    public void SelectMaterial2()
    {
        upgrade.SelectMaterial(material2 as IUpgradeMaterial);
    }

    public void SelectMaterial3()
    {
        upgrade.SelectMaterial(material3 as IUpgradeMaterial);
    }
}
