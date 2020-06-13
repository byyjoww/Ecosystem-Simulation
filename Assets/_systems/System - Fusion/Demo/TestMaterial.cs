using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "DEBUG/Scriptable Object/IUpgradeMaterial")]
public class TestMaterial : ScriptableObject, IUpgradeMaterial
{
    [SerializeField] private int progressScore;
    public int ProgressScore => progressScore;

    [SerializeField] private int amount;
    public int Amount => amount;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    public void RemoveAmount(int quantityToRemove)
    {
        amount -= quantityToRemove;
    }
}
