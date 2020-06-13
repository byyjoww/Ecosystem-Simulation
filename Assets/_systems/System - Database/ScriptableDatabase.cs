using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Database", menuName = "Scriptable Object/Scriptable Database")]
public class ScriptableDatabase : ScriptableObject
{
    public Database.DatabaseType sectionType = default;
    [RequireInterface(typeof(IScriptableIdentifiable<Database.DatabaseType>))]
    public List<ScriptableElement> databaseElements;

    private void OnValidate()
    {
        DatabaseTools.RefreshScriptableData<Database.DatabaseType, ScriptableElement>(ref databaseElements);
    }
}