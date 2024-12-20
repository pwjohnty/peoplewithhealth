using Foundation;
using Microsoft.Azure.NotificationHubs;
using PeopleWith.Platforms.iOS.Services;
using UIKit;
using UserNotifications;

namespace PeopleWith
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                var result = base.FinishedLaunching(application, launchOptions);

                this.InvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                  //  UNUserNotificationCenter.Current.Delegate = new UserNotificaitonCenterDelegate();
                });
                //removed so the user is asked on the reg about notification , check if azure notifications later

                //var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                //UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                //{
                //    if (granted && error == null)
                //    {
                //        this.InvokeOnMainThread(() =>
                //        {
                //            UIApplication.SharedApplication.RegisterForRemoteNotifications();
                //            UNUserNotificationCenter.Current.Delegate = new UserNotificaitonCenterDelegate();
                //        });
                //    }
                //});

                return result;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try
            {
                //Lock Font Size 
                UIKit.UIFont.GetPreferredFontForTextStyle(UIKit.UIFontTextStyle.Body).WithSize(16);


                string token = null!;
                if (deviceToken.Length > 0)
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        var data = deviceToken.ToArray();
                        token = BitConverter
                            .ToString(data)
                            .Replace("-", "")
                            .Replace("\"", "");
                    }
                    else if (!string.IsNullOrEmpty(deviceToken.Description))
                    {
                        token = deviceToken.Description.Trim('<', '>');
                    }

                    Preferences.Set("token", token);
                }
                //  var hubName = "PWDevHub";
                //  var connectionString = "Endpoint=sb://PWDevelopment.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=ZiwsFi5CJVNru6prZMix/55OIDEZJvXumOSBkRjU4gM="; // Can be found in Access policy. Use Listen connection

                IList<string> tags = new List<string>();

                if (!string.IsNullOrEmpty(Helpers.Settings.UserKey))
                {
                    tags.Add(Helpers.Settings.UserKey);
                }

                if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
                {
                    tags.Add(Helpers.Settings.SignUp);
                }

                // tags.Add("MARK");

                var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.ListenConnectionString, Constants.NotificationHubName);
                var installation = new Installation
                {
                    InstallationId = token,
                    PushChannel = token,
                    Platform = NotificationPlatform.Apns,
                    Tags = tags
                };
                await hub.CreateOrUpdateInstallationAsync(installation);

            }
            catch(Exception ex)
            {

            }
        }

    }
}
