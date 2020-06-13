using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    int Amount { get; set; }
    bool IsMaxLevel { get; }
    Sprite Sprite { get; }
    Currency Currency { get; }
    int UpgradeCost { get; }
    List<IUpgradeMaterial> Materials { get; }
}