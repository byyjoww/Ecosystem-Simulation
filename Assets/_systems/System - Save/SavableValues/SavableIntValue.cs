using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SavableIntValue", menuName = "Scriptable Object/Savable Value/Primitive/Int", order = 1)]
public class SavableIntValue : IntValue, ISavable
{
    public ushort Size => sizeof(int);

    public void Load(BinaryReader reader)
    {
        Value = reader.ReadInt32();
    }

    public void LoadDefault()
    {
        Value = defaultValue;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Value);
    }
}
