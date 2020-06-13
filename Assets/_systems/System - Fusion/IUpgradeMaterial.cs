using UnityEngine;

public interface IUpgradeMaterial
{
    int ProgressScore { get; }
    int Amount { get; }
    Sprite Sprite { get; }
    void RemoveAmount(int quantityToRemove);
}