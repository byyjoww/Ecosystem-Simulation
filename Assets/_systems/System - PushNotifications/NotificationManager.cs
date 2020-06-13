using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] NotificationSettings notificationSettings;

    void Awake()
    {
        DontDestroyOnLoad(this);
        InitializeNotifications();
    }

    void Start()
    {
        //Notifications.AskForPermission();
    }

    void InitializeNotifications()
    {
        //Notifications.RegisterLocalScheduler(() =>
        //{
        //    var list = new List<Notifications.GameNotification>();

        //    var utcNow = AppDateTime.UtcNow;

        //    foreach (var notificationData in notificationSettings.LocalNotifications)
        //    {
        //        if (!notificationData.ShouldSchedule())
        //        {
        //            continue;
        //        }
                
        //        var notification = new Notifications.GameNotification();
        //        notification.Message = notificationData.Message;
        //        notification.FireUtcDate = notificationData.GetFireUtcDate(utcNow);
        //        notification.AnalyticsId = notificationData.AnalyticsId;
        //        list.Add(notification);
        //    }
            
        //    return list.ToArray();
        //});
        
        //Notifications.RegisterCustomRetentationMessages(() =>
        //{
        //    var list = new List<Notifications.GameNotification>();
        //    foreach (var msg in notificationSettings.RetentationMessages)
        //    {
        //        list.Add(new Notifications.GameNotification {Title = notificationSettings.GameTitle, Message = msg});
        //    }

        //    return list.ToArray();
        //});
    }
}
