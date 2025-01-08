using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public interface INotificationService
    {
        Task RequestNotificationPermissionAsync();

        Task<bool> CheckRequestNotificationPermissionAsync();

        Task<bool> AreNotificationsEnabledAsync();
    }
}
