using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NotificationSettings", menuName = "Scriptable Object/Notification/Notification Settings", order = 1)]
public class NotificationSettings : ScriptableObject, IInitializable
{
    public string GameTitle;
    public List<NotificationData> LocalNotifications;
    public List<string> RetentationMessages;

    public void Init()
    {
       AskForPermission();

       RegisterLocalScheduler(() =>
        {
            var list = new List<GameNotification>();

            var utcNow = DateTime.UtcNow;

            foreach (var notificationData in LocalNotifications)
            {
                if (!notificationData.ShouldSchedule())
                {
                    continue;
                }

                var notification = new GameNotification();
                notification.Message = notificationData.Message;
                notification.FireUtcDate = notificationData.GetFireUtcDate(utcNow);
                list.Add(notification);
            }

            return list.ToArray();
        });

        RegisterCustomRetentationMessages(() =>
        {
            var list = new List<GameNotification>();
            foreach (var msg in RetentationMessages)
            {
                list.Add(new GameNotification { Title = GameTitle, Message = msg });
            }

            return list.ToArray();
        });
    }

    [Serializable]
    public class GameNotification
    {
        public string Title;
        public string Message;

        public string TitleColor; //Hexadecimal color
        public string MessageColor; //Hexadecimal color

        public string PictureTitle;
        public string PictureText;

        public string BackgroundImage;
        public string BackgroundIcon;

        public string NotificationIcon;

        [NonSerialized] public DateTime FireUtcDate;
    }

    /// <summary>
    /// Ask for notification permission.
    /// Call this method in a relevant time for the user.
    /// </summary>
    private void AskForPermission()
    {
        Debug.Log("Asking User for Permission");
    }

    /// <summary>
    /// Expects a function that return the list of desired notifications for this session.
    /// This function is called when the notifications are being scheduled, usually in the end of a session.
    /// All notification are canceled in the beggining of the session and are supposed to be re-scheduled at the end.
    /// </summary>
    private void RegisterLocalScheduler(Func<GameNotification[]> p)
    {
        Debug.Log("??");
    }

    /// <summary>
    /// Expects a function that returns a list of retention messages (type of Game Notification).
    /// This can be used to provide localization to retentation messages.
    /// This function is called when retentions messages are being scheduled.
    /// If nothing is registered, than the we use the default retentation messages defined in the module data.
    /// </summary>
    void RegisterCustomRetentationMessages(Func<IEnumerable<GameNotification>> scheduler)
    {
        Debug.Log("Registering Notifications");
    }

    public bool Initialized => true;
}