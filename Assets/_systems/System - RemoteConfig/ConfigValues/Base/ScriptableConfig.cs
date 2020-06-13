using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public abstract class ScriptableConfig : ScriptableObject
{
    public abstract void LoadConfig(ConfigResponse response);
}
