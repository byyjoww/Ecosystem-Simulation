using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SavableBoolValue", menuName = "Scriptable Object/Savable Value/Primitive/Bool", order = 1)]
public class SavableBoolValue : BoolValue, ISavable
{
    public ushort Size => sizeof(bool);

    public void Load(BinaryReader reader)
    {
        Value = reader.ReadBoolean();
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
