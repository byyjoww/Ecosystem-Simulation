using System.IO;

public interface ISavable
{
    ushort Size { get; }
    void Load(BinaryReader reader);
    void LoadDefault();
    void Save(BinaryWriter writer);
}
