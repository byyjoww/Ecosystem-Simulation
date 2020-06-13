using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "Scriptable Object/Savable Value/Save System", order = 1)]
public class SaveSystem : ScriptableObject, IInitializable
{
    [SerializeField, RequireInterface(typeof(ISavable))]
    List<ScriptableValue> savablesList = new List<ScriptableValue>();

    [SerializeField,
     Tooltip("Use Keychain for a persistent save.\nIf the user deletes the app the save won't be deleted.")]
    bool isPersistent;

    const ushort MaxSize = 16384;
    //TODO: Validate MaxSize and show warning or error if higher
    bool IsLoaded = false;

    public static event Action OnSaveComplete;
    public static event Action OnLoadComplete;

    void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        End();
    }

    public void Init()
    {
        if (IsLoaded)
        {
            return;
        }
        
        foreach (var savable in savablesList)
        {
            savable.OnValueChanged += SaveData;
        }
        
        LoadData();
        IsLoaded = true;
        
        Debug.Log("SaveSystem Ready");
    }

    public bool Initialized => IsLoaded;

    public void End()
    {
        if (!IsLoaded)
        {
            return;
        }
        
        foreach (var savable in savablesList)
        {
            savable.OnValueChanged -= SaveData;
        }
        
        IsLoaded = false;
    }

    ushort GetSavableListBufferSize()
    {
        ushort size = 0;

        foreach (ISavable savable in savablesList)
        {
            size += savable.Size;
        }
        
        if (size > MaxSize)
        {
            Debug.LogError("Saving more than MaxSize <" + MaxSize +
                           "> might result in unexpected behaviours. Current Size <" + size + ">");
        }

        return size;
    }

    public void LoadData()
    {
        var str = isPersistent ? KeychainAccess.GetKeychainString() : PlayerPrefs.GetString("SaveSystem.LocalSave");

        if (string.IsNullOrEmpty(str))
        {
            Debug.Log("No save found. Creating first save.");
            SetupInitialValues();
            SaveData();
            return;
        }
        
        var buffer = Convert.FromBase64String(str);
        Stream binaryStream = new MemoryStream(buffer);
        var reader = new BinaryReader(binaryStream);
                
        foreach (ISavable savable in savablesList)
        {
            savable.Load(reader);
        }        
        
        reader.Close();

        OnLoadComplete?.Invoke();
    }

    private void SetupInitialValues()
    {
        foreach (ISavable savable in savablesList)
        {
            savable.LoadDefault();
        }
    }

    public void SaveData()
    {
        var buffer = new byte[GetSavableListBufferSize()];
        Stream binaryStream = new MemoryStream(buffer);
        var writer = new BinaryWriter(binaryStream);
        
        foreach (ISavable savable in savablesList)
        {
            savable.Save(writer);
        }
        
        writer.Close();
        
        var str = Convert.ToBase64String(buffer);

        if (isPersistent)
        {
            KeychainAccess.SaveKeychainString(str);
        }
        else
        {
            PlayerPrefs.SetString("SaveSystem.LocalSave", str);
            PlayerPrefs.Save();
        }

        OnSaveComplete?.Invoke();
    }

    private void OnValidate()
    {
        savablesList = Tools.GetAllScriptableObjects<ScriptableValue>().Where(x => x is ISavable).ToList();
        // Debug.Log("Validated SaveSystem");
    }
}
