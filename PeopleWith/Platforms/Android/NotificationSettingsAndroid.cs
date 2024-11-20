using PeopleWith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(NotificationSettingsAndroid))]
namespace PeopleWith
{
    
    public class NotificationSettingsAndroid : INotificationSettings
    {
        public async Task OpenNotificationSettingsAsync()
        {
            var intent = new Android.Content.Intent(Android.Provider.Settings.ActionAppNotificationSettings);
            intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, Android.App.Application.Context.PackageName);
            intent.AddFlags(Android.Content.ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}
