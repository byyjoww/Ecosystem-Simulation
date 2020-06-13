using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SavableHeroValue", menuName = "Scriptable Object/Savable Value/Complex/Hero", order = 1)]
public class SavableHeroValue : HeroValue, ISavable
{
    public ushort Size => 2 * sizeof(int);

    public void Load(BinaryReader reader)
    {
        Value = new HeroID(reader.ReadInt32(), reader.ReadInt32());
    }

    public void LoadDefault()
    {

    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Value.heroName);
        writer.Write(Value.heroLevel);
    }
}
