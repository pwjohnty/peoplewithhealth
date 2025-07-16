using Microsoft.Azure.NotificationHubs;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
#if ANDROID
using Plugin.Firebase.CloudMessaging;
using static Android.Provider.Settings;
#endif
namespace PeopleWith
{
    public class PWNotificationService
    {
        private NotificationHubClient hub;
        private string deviceid;
        private string token;
        #if ANDROID
        private static string? GetDeviceId() => Secure.GetString(Android.App.Application.Context.ContentResolver, Secure.AndroidId);
        #endif
        public PWNotificationService()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(Constants.ListenConnectionString, Constants.NotificationHubName);
            CheckTokenDevice(); 
        }

        async void CheckTokenDevice()
        {
#if ANDROID
                deviceid = Preferences.Get("Device_token", string.Empty);

                if (string.IsNullOrEmpty(deviceid))
                {
                    deviceid = GetDeviceId();
                    Preferences.Set("Device_token", deviceid);
                }

                token = Preferences.Get("fcm_token", string.Empty);

                if (string.IsNullOrEmpty(token))
                {
                    token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                    Preferences.Set("fcm_token", token);
                }
#elif IOS
            token = Preferences.Get("token", string.Empty);           
#endif
        }

        public async Task ClearTagsAsync()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    var installation = new Microsoft.Azure.NotificationHubs.Installation
                    {
                        InstallationId = deviceid,
                        PushChannel = token,
                        Platform = NotificationPlatform.FcmV1,
                        Tags = new List<string>()
                    };
                    await hub.CreateOrUpdateInstallationAsync(installation);
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    var installation = new Installation
                    {
                        InstallationId = token,
                        PushChannel = token,
                        Platform = NotificationPlatform.Apns,
                        Tags = new List<string>()
                    };

                    await hub.CreateOrUpdateInstallationAsync(installation);
                }
            }
            catch (Exception ex)
            {
                //Add AppCenter
            }
        }

        public async Task AddTag(IList<string> tags)
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    var installation = new Microsoft.Azure.NotificationHubs.Installation
                    {
                        InstallationId = deviceid,
                        PushChannel = token,
                        Platform = NotificationPlatform.FcmV1,
                        Tags = tags
                    };
                    await hub.CreateOrUpdateInstallationAsync(installation);
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    var installation = new Installation
                    {
                        InstallationId = token,
                        PushChannel = token,
                        Platform = NotificationPlatform.Apns,
                        Tags = tags
                    };

                    await hub.CreateOrUpdateInstallationAsync(installation);
                }

            }
            catch (Exception ex)
            {
                //Add AppCenter
            }
        }
    }
}