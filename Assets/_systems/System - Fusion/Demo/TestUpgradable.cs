using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgradable", menuName = "DEBUG/Scriptable Object/IUpgradable")]
public class TestUpgradable : ScriptableObject, IUpgradable
{
    [SerializeField] private int amount;
    public int Amount { get => amount; set => amount = value; }

    [SerializeField] private bool isMaxLevel;
    public bool IsMaxLevel => isMaxLevel;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private Currency currency;
    public Currency Currency => currency;

    [SerializeField] private int upgradeCost;
    public int UpgradeCost => upgradeCost;

    [SerializeField, RequireInterface(typeof(IUpgradeMaterial))] private List<UnityEngine.Object> materials = new List<UnityEngine.Object>();
    public List<IUpgradeMaterial> Materials => materials.Cast<IUpgradeMaterial>().ToList();
}
