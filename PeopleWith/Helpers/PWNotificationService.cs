using Microsoft.Azure.NotificationHubs;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace PeopleWith
{
    public class PWNotificationService
    {
        private NotificationHubClient hub;
        private  string installationId;
        private string token;

        public PWNotificationService()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(Constants.ListenConnectionString, Constants.NotificationHubName);
            installationId = Helpers.Settings.DeviceID;

            string tokentag = Preferences.Default.Get("token", "Unknown");

            token = tokentag;
        }

        public async Task ClearTagsAsync()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    var installation = new Microsoft.Azure.NotificationHubs.Installation
                    {
                        InstallationId = installationId,
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
                        InstallationId = installationId,
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