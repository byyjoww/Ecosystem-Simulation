using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RemoteConfigTester : MonoBehaviour
{
    public IntRemoteConfig remoteConfig;
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    private void Start()
    {
       if (remoteConfig.Value > 200)
       {
            OnSuccess?.Invoke();
       }
       else
       {
            OnFail?.Invoke();
       }
    }
}
