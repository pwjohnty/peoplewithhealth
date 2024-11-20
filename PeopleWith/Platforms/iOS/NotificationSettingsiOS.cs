using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using PeopleWith;

[assembly: Dependency(typeof(NotificationSettingsiOS))]
namespace PeopleWith
{
    public class NotificationSettingsiOS : INotificationSettings
    {
        public async Task OpenNotificationSettingsAsync()
        {
            var url = new Uri("app-settings:");
            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
        }
    }
}
