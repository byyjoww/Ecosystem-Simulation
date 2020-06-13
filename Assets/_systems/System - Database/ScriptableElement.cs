using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableElement : ScriptableObject, IScriptableIdentifiable<Database.DatabaseType>
{
    public abstract Database.DatabaseType EnumIdentifier { get; }
}

