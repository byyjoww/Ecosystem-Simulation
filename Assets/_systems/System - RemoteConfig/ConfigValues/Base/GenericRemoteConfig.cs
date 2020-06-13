using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public abstract class GenericRemoteConfig<T> : ScriptableConfig
{
    public string configName;
    protected T data;
    public T Value => data;
}