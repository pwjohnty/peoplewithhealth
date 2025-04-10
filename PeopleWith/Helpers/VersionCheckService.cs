using System;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace PeopleWith
{
    public class VersionCheckService
    {
        public string GetCurrentVersion()
        {
            try
            {
                var version = AppInfo.Version.ToString();
              //  var version = DependencyService.Get<IAppVersionProvider>().GetVersion();
                return version;
            }
            catch(Exception ex)
            { 
                return "0.0.0";
                var s = ex.Message;
                var ss= ex.StackTrace;
            }

        }

        public async Task<string> GetLatestVersion()
        {
            
            // Add the new app version on every update
            return await Task.FromResult("3.0.1"); // Replace with actual implementation
        }

        public async Task CheckForUpdate()
        {
            var currentVersion = GetCurrentVersion();
            var latestVersion = await GetLatestVersion();

            if (currentVersion != latestVersion)
            {
                // Prompt user to update

                await Application.Current.MainPage.Navigation.PushAsync(new UpdatePage());

                //await Application.Current.MainPage.DisplayAlert(
                //    "Update Available",
                //    "A new version of the app is available. Please update to continue.",
                //    "Update");

                //// Redirect to App Store
                //var appStoreUrl = DeviceInfo.Platform == DevicePlatform.iOS
                //    ? "https://apps.apple.com/app/1323985690" // Replace YOUR_APP_ID with your app's ID
                //    : "https://play.google.com/store/apps/details?id=com.peoplewith.peoplewith"; // Replace YOUR_PACKAGE_NAME with your app's package name

                //await Launcher.Default.OpenAsync(new Uri(appStoreUrl));
            }
        }
    }
}