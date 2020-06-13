using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

[CreateAssetMenu(fileName = "new Remote Config", menuName = "Scriptable Object/Remote Config/Float")]
public class FloatRemoteConfig : GenericRemoteConfig<float>
{
    public override void LoadConfig(ConfigResponse response)
    {
        data = ConfigManager.appConfig.GetFloat(configName);
    }
}
