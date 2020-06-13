using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

[CreateAssetMenu(fileName = "new Remote Config", menuName = "Scriptable Object/Remote Config/Bool")]
public class BoolRemoteConfig : GenericRemoteConfig<bool>
{
    public override void LoadConfig(ConfigResponse response)
    {
        data = ConfigManager.appConfig.GetBool(configName);
    }
}
