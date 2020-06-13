using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

[CreateAssetMenu(fileName = "new Remote Config", menuName = "Scriptable Object/Remote Config/Long")]
public class LongRemoteConfig : GenericRemoteConfig<long>
{
    public override void LoadConfig(ConfigResponse response)
    {
        data = ConfigManager.appConfig.GetLong(configName);
    }
}
