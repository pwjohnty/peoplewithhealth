using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using Microsoft.Maui.Storage;

namespace PeopleWith
{
    public class Logout
    {
        public Logout()
        {
            ClearNotifications();
            ClearUserInfo();
        }

        async public void ClearNotifications()
        {
            try
            {
                //clear local notifcations
                LocalNotificationCenter.Current.ClearAll();
                LocalNotificationCenter.Current.CancelAll();

                //clear tags for azure notications
                var notificationService = new PWNotificationService();
                await notificationService.ClearTagsAsync();
            }
            catch (Exception ex)
            { 

            }
        }

        public async void ClearUserInfo()
        {
            try
            {
                // Clearing user information from settings
                Preferences.Set("userid", string.Empty);
                Preferences.Set("firstname", string.Empty);
                Preferences.Set("surname", string.Empty);
                Preferences.Set("gender", string.Empty);
                Preferences.Set("email", string.Empty);
                Preferences.Set("password", string.Empty);
                Preferences.Set("age", string.Empty);
                Preferences.Set("ethnicity", string.Empty);
                Preferences.Set("userpasswordhash", string.Empty);
                Preferences.Set("signupcode", string.Empty);
                Preferences.Set("pincode", string.Empty);
                Preferences.Set("launchvideo", string.Empty);
                Preferences.Set("clinicaltrial", string.Empty);
                Preferences.Set("clinicaltrial", string.Empty);
                Preferences.Set("biometrics", false);
                Preferences.Set("NotificationsEnabled", false);
                Preferences.Set("validationcode", string.Empty);
                Preferences.Set("token", string.Empty);

                await Task.Delay(300);
                //Application.Current.MainPage = new NavigationPage(new MainPage());
                await App.SetMainPage(new NavigationPage(new MainPage()));
                // await Shell.Current.GoToAsync("MainPage");


                //Application.Current.MainPage = new NavigationPage(new MainPage());

            }
            catch (Exception ex)
            {

            }
        }
    }
}