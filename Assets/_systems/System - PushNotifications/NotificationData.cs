using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationData", menuName = "Scriptable Object/Notification/Notification Data", order = 1)]
public class NotificationData : ScriptableObject
{
    [SerializeField] protected string message;
    [SerializeField] protected double addMinutes;
    [SerializeField] protected double addHours;
    [SerializeField] protected double addDays;
    [SerializeField] protected BoolValue shouldSchedule;

    public virtual string Message => message;

    public virtual bool ShouldSchedule()
    {
        return shouldSchedule != null ? shouldSchedule : true;
    }

    public virtual DateTime GetFireUtcDate(DateTime utcNow)
    {
        return utcNow.AddMinutes(addMinutes).AddHours(addHours).AddDays(addDays);
    }
}
