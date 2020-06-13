using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "IAPStoreItems", menuName = "Scriptable Object/IAP/IAP Store Items", order = 1)]
public class IAPStoreItems : ScriptableObject
{
    public List<StoreItemPack> itemsList;
    
    #if UNITY_EDITOR
    [ContextMenu("Export YML")]
    void ExportYML()
    {
        var str = "iaps:\n";

        foreach (var itemPack in itemsList)
        {
            str += itemPack.ToYML() + "\n";
        }

        const string path = "Assets/Metadata/iaps.yml";

        var writer = File.Exists(path) ? new StreamWriter(path, false) : File.CreateText(path);
        writer.Write(str);
        writer.Close();
        
        AssetDatabase.ImportAsset(path);
        
        Debug.Log("IAPs YML Created in Assets/Metadata");
    }
    #endif
}
