using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SavableDateTimeValue", menuName = "Scriptable Object/Savable Value/Complex/DateTime", order = 1)]
public class SavableDateTimeValue : DateTimeValue, ISavable
{
    public ushort Size => sizeof(long);

    public void Load(BinaryReader reader)
    {
        Value = DateTime.FromBinary(reader.ReadInt64());
    }

    public void LoadDefault()
    {
        Value = defaultValue;
    }

    public void Save(BinaryWriter writer)
    {
       
        writer.Write(Value.ToBinary());
    }
}
