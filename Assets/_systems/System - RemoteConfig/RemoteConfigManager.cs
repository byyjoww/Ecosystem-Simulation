using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;
using System.Linq;

[CreateAssetMenu(fileName = "RemoteConfigSystem", menuName = "Scriptable Object/Remote Config/Config Manager")]
public class RemoteConfigManager : ScriptableObject, IInitializable
{
    public struct userAttributes { }
    public struct appAttributes { }

    [SerializeField] ScriptableEvent OnSceneLoad;
    [SerializeField] ScriptableEvent OnRemoteConfigLoadComplete;
    [SerializeField] ScriptableConfig fetchConfigsEveryScene;
    [SerializeField] bool isInitialized = false;
    public bool Initialized => isInitialized;

    public List<ScriptableConfig> remoteConfigs = new List<ScriptableConfig>();

    private void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        End();
    }

    public void Init()
    {
        if (isInitialized)
        {
            return;
        }

        OnSceneLoad.OnRaise += GetConfigs;

        isInitialized = true;

        Debug.Log("RemoteConfig Ready");
    }

    public void End()
    {
        if (!isInitialized)
        {
            return;
        }

        isInitialized = false;
    }

    private void GetConfigs()
    {
        if (!fetchConfigsEveryScene)
        {
            OnSceneLoad.OnRaise -= GetConfigs;
            OnSceneLoad.OnRaise += OnRemoteConfigLoadComplete.Raise;
        }        

        foreach (var config in remoteConfigs)
        {
            ConfigManager.FetchCompleted += config.LoadConfig;
        }

        ConfigManager.FetchCompleted += ConfigFetchComplete;

        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    private void ConfigFetchComplete(ConfigResponse response)
    {
        foreach (var config in remoteConfigs)
        {
            ConfigManager.FetchCompleted -= config.LoadConfig;
        }
        
        ConfigManager.FetchCompleted -= ConfigFetchComplete;
        
        OnRemoteConfigLoadComplete.Raise();

        Debug.Log("Remote Config Fetch Complete.");
    }

    private void OnValidate()
    {
        remoteConfigs = Tools.GetAllScriptableObjects<ScriptableConfig>().ToList();
        // Debug.Log("Validated RemoteConfigManager");
    }
}