﻿using UserNotifications;
using Foundation;
//using PeopleWith;
using Microsoft.Maui.ApplicationModel;
using PeopleWith;

[assembly: Dependency(typeof(NotificationService))]
namespace PeopleWith
{
    public class NotificationService : INotificationService
    {
        public async Task RequestNotificationPermissionAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (approved, error) =>
            {
                tcs.SetResult(approved);
            });

            await tcs.Task;
        }
    }
}