using UserNotifications;
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
            //var tcs = new TaskCompletionSource<bool>();

            //UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (approved, error) =>
            //{
            //    tcs.SetResult(approved);
            //});

            //await tcs.Task;
            try
            {
                var tcs = new TaskCompletionSource<bool>();

                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
                    (approved, error) =>
                    {
                        if (error != null)
                        {
                            tcs.SetException(new Exception(error.LocalizedDescription));
                        }
                        else
                        {
                            tcs.SetResult(approved);
                        }
                    });

                bool isApproved = await tcs.Task;

                // Optional: Notify the user of the approval status
                await Application.Current.MainPage.DisplayAlert(
                    "Notification Permission",
                    isApproved ? "Permission granted!" : "Permission denied.",
                    "OK"
                );
            }
            catch (Exception ex)
            {
                // Display the error message in an alert
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"An error occurred while requesting notification permissions: {ex.Message} + {ex.StackTrace}",
                    "OK"
                );
            }
        }

        public async Task<bool> CheckRequestNotificationPermissionAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (granted, error) =>
                {
                    tcs.SetResult(granted);
                });

            return await tcs.Task;
        }

        public async Task<bool> AreNotificationsEnabledAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
            {
                tcs.SetResult(settings.AuthorizationStatus == UNAuthorizationStatus.Authorized);
            });

            return await tcs.Task;
        }
    }
}