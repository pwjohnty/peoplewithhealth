using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using Microsoft.Maui.Controls;
using PeopleWith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace PeopleWith
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]

    public class FirebaseService : FirebaseMessagingService
    {
        const string CHANNEL_ID = "PeopleWithLocalNotifications";
        ObservableCollection<pushdata> NotificationData = new();

        //New Code
        public async override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            NotificationData.Clear();

            string title = "Notification";
            string body = "You have a new message.";

            if (message.GetNotification() != null)
            {
                title = message.GetNotification().Title ?? title;
                body = message.GetNotification().Body ?? body;
            }

            if (message.Data != null && message.Data.Count > 0)
            {
                if (message.Data.TryGetValue("title", out var dataTitle)) title = dataTitle;

                if (message.Data.TryGetValue("body", out var dataBody)) body = dataBody;

                var internalIntent = new Intent("MyPushNotification");
                foreach (var item in message.Data)
                {
                    internalIntent.PutExtra(item.Key, item.Value);
                    var NewValue = new pushdata
                    {
                        key = item.Key.ToString(),
                        Data = item.Value.ToString()
                    };
                    NotificationData.Add(NewValue);
                }
                //Android.App.Application.Context.SendBroadcast(internalIntent);

                var filePath = Path.Combine(FileSystem.AppDataDirectory, "PWPushNotification.json");
                var json = JsonSerializer.Serialize(NotificationData);
                // Save/update
                await File.WriteAllTextAsync(filePath, json);

                //var context = Android.App.Application.Context;
                //var prefs = context.GetSharedPreferences("MyAppPrefs", FileCreationMode.Private);
                //var editor = prefs.Edit();

                //// Serialize the NotificationData to JSON and save it
                //var json = JsonSerializer.Serialize(NotificationData);
                //editor.PutString("PWPushNotification", json);
                //editor.Apply(); // Apply changes asynchronously

                await ShowNotification(title, body, NotificationData);
            }
            else
            {
                await ShowNotification(title, body, NotificationData);
            }
        }

        private async Task ShowNotification(string title, string body, ObservableCollection<pushdata> ExtraData)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

            foreach (var item in ExtraData)
            {
                intent.PutExtra(item.key, item.Data);
            }

            var pendingIntent = PendingIntent.GetActivity(
                this, 0, intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.OneShot
            );

            var soundUri = Android.Net.Uri.Parse($"{ContentResolver.SchemeAndroidResource}://{PackageName}/{Resource.Raw.pwjingo}");


            var notificationBuilder = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.pwicon) 
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetSound(soundUri)
                .SetPriority((int)NotificationPriority.High)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(new Random().Next(), notificationBuilder.Build());
        }
    }
}





        //public const string PRIMARY_CHANNEL = "default";

        //    public override void OnMessageReceived(RemoteMessage message)
        //    {
        //        base.OnMessageReceived(message);
        //        string messageBody = string.Empty;

        //        if (message.GetNotification() != null)
        //        {
        //            messageBody = message.GetNotification().Body;
        //        }

        //        // NOTE: test messages sent via the Azure portal will be received here
        //        else
        //        {
        //            if (message.Data.TryGetValue("message", out var messageString))
        //            {
        //                messageBody = messageString;
        //            }
        //        }

        //        try
        //        {
        //            // convert the incoming message to a local notification
        //            MainThread.BeginInvokeOnMainThread(() =>
        //            {
        //                SendLocalNotification(messageBody);
        //            });
        //        }
        //        catch
        //        {
        //        }
        //    }

        //    public bool IsForeground()
        //    {
        //        ActivityManager.RunningAppProcessInfo appProcessInfo = new ActivityManager.RunningAppProcessInfo();
        //        ActivityManager.GetMyMemoryState(appProcessInfo);
        //        return (appProcessInfo.Importance == Importance.Foreground || appProcessInfo.Importance == Importance.Visible);
        //    }

        //    public override void HandleIntent(Intent intent)
        //    {
        //        try
        //        {
        //            MainThread.BeginInvokeOnMainThread(() =>
        //            {

        //                base.HandleIntent(intent);
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //    public override async void OnNewToken(string token)
        //    {
        //        // Set the token name to secure storage
        //        await SecureStorage.SetAsync("FireBaseToken", token);
        //    }

        //    private void SendLocalNotification(string body)
        //    {
        //        if (string.IsNullOrEmpty(body))
        //        {
        //            return;
        //        }
        //        NotificationManager notMan = (NotificationManager)GetSystemService(Context.NotificationService);

        //        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        //        {
        //            var channelDescription = string.Empty;
        //            var channel = new NotificationChannel(PRIMARY_CHANNEL, PRIMARY_CHANNEL, NotificationImportance.High);
        //            notMan.CreateNotificationChannel(channel);
        //        }

        //        var intent = new Intent(this, typeof(MainActivity));
        //        intent.AddFlags(ActivityFlags.ClearTop);
        //        intent.PutExtra("message", body);
        //        var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);

        //        var notificationBuilder = new NotificationCompat.Builder(this, PRIMARY_CHANNEL)
        //            .SetSmallIcon(Resource.Drawable.material_ic_calendar_black_24dp)
        //            .SetContentText(body)
        //            .SetAutoCancel(true)
        //            .SetShowWhen(false)
        //            .SetContentIntent(pendingIntent)
        //            .SetNumber(6)
        //            .SetContentInfo("info");

        //        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        //        {
        //            //notificationBuilder.SetSmallIcon(Resource.Drawable.);
        //            notificationBuilder.SetColor(GetColor(_Microsoft.Android.Resource.Designer.Resource.Color.colorPrimary));
        //        }
        //        else
        //        {
        //            notificationBuilder.SetSmallIcon(Resource.Drawable.material_ic_menu_arrow_down_black_24dp);
        //        }

        //        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        //        {
        //            notificationBuilder.SetChannelId(PRIMARY_CHANNEL);
        //        }

        //        var notificationManager = NotificationManager.FromContext(this);
        //        notificationManager.Notify(new Random().Next(), notificationBuilder.Build());
        //    }
    //}
//}