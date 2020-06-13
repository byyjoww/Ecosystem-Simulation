using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RefreshGroup { START = 1, UPDATE = 2, FIXED_UPDATE = 3, LATE_UPDATE = 4 }

public class FollowPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private RefreshGroup RefreshPeriod = RefreshGroup.START;
    [SerializeField] private float smoothSpeed = 0.125f;

    [Header("Position")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {       
        if (target == null) FindTarget();        
    }

    private void FindTarget()
    {
        if (target == null)
            return;

        SetInitialPosition();
    }

    private void SetInitialPosition()
    {
        transform.position = target.position + offset;
    }

    private void Update()
    {
        if(RefreshPeriod == RefreshGroup.UPDATE)
            UpdatePosition();
    }

    private void FixedUpdate()
    {
        if (RefreshPeriod == RefreshGroup.FIXED_UPDATE)
            UpdatePosition();
    }

    private void LateUpdate()
    {
        if (RefreshPeriod == RefreshGroup.LATE_UPDATE)
            UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}