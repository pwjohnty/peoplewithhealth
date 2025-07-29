using System;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace PeopleWith
{
    public class VersionCheckService
    {
        public async Task<bool> CheckForUpdate()
        {
            var currentVersion = AppInfo.Current.VersionString;
            var latestVersion = string.Empty;
            //Ensure the following is changed
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                latestVersion = "13.0.4";
            }
            else
            {
                latestVersion = "97.1";
            }

            if (string.IsNullOrEmpty(currentVersion) || string.IsNullOrEmpty(latestVersion)) return false;

            if (currentVersion != latestVersion)
            {
                return true; 
                //Application.Current.Windows[0].Page = new NavigationPage(new UpdatePage());
                //await App.Current.MainPage.Navigation.PushAsync(new UpdatePage());
                //await Navigation.PushAsync(new UpdatePage(), false);

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
            else
            {
                return false; 
            }
        }
    }
}