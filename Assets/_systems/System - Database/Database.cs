using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Persistent, CreateFromPrefab("Loader"), RequireComponent(typeof(AppInitializer))]
public sealed class Database : Singleton<Database>
{
    //-----------------------CORE-----------------------//
    [SerializeField] private List<ScriptableDatabase> databases = new List<ScriptableDatabase>();
    public List<ScriptableDatabase> Databases => databases;    

    //---------------DATABASE MANAGEMENT---------------//
    public enum DatabaseType { Item = 0, Crate = 1 }

    public List<T> GetDatabaseElements<T>(DatabaseType databaseType)
    {
        var database = Databases.First(x => x.sectionType == databaseType);
        var list = database.databaseElements.Cast<T>().ToList();

        return list;
    }
}
