using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

[CreateAssetMenu(fileName = "new Remote Config", menuName = "Scriptable Object/Remote Config/String")]
public class StringRemoteConfig : GenericRemoteConfig<string>
{
    public override void LoadConfig(ConfigResponse response)
    {
        data = ConfigManager.appConfig.GetString(configName);
    }
}
