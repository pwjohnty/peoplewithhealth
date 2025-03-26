using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace PeopleWith
{
    public class ActivityNotifications
    {

        //Crash Handler
        CrashDetected crashHandler = new CrashDetected();
        async public void NotasyncMethod(Exception Ex)
        {
            try
            {
                //await crashHandler.SentryCrashDetected(Ex);
                //await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
            }
            catch (Exception ex)
            {
                //Dunno 
            }
        }
        public async Task CreateNotifications()
        {
            try
            {

                //var sd = DateTime.Parse(startdate);

                //var startdateandtime = sd + time;
                //var NotDescription = 

                //var notification = new NotificationRequest
                //{
                //    NotificationId = notid,
                //    Title = "Activity Reminder",
                //    Description = NotDescription,
                //    BadgeNumber = 0,
                //    Schedule = new NotificationRequestSchedule
                //    {
                //        NotifyTime = startdateandtime,
                //        RepeatType = NotificationRepeat.Daily,
                //        NotifyRepeatInterval = null
                //    }



                //if Notifications are set to Disabled, Create then but don't show them so they can be fixed at later date 

                //LocalNotificationCenter.Current.Show(notification);

            }
            catch (Exception Ex)
            {
                NotasyncMethod(Ex);
            }
        }

    }
}
