using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

[CreateAssetMenu(fileName = "new Remote Config", menuName = "Scriptable Object/Remote Config/Int")]
public class IntRemoteConfig : GenericRemoteConfig<int>
{
    public override void LoadConfig(ConfigResponse response)
    {
        data = ConfigManager.appConfig.GetInt(configName);
    }
}
