using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum TimeToRun { START = 0, UPDATE = 1, FIXED_UPDATE = 2, LATE_UPDATE = 3 }

    [SerializeField] TimeToRun UpdateMode = TimeToRun.START;
    [SerializeField] Camera target;

    private void Start()
    {
        target = Camera.main;
        transform.LookAt(target.transform);
    }

    private void Update()
    {
        if(UpdateMode == TimeToRun.UPDATE)
            transform.LookAt(target.transform);
    }

    private void FixedUpdate()
    {
        if (UpdateMode == TimeToRun.FIXED_UPDATE)
            transform.LookAt(target.transform);
    }

    private void LateUpdate()
    {
        if (UpdateMode == TimeToRun.LATE_UPDATE)
            transform.LookAt(target.transform);
    }
}
