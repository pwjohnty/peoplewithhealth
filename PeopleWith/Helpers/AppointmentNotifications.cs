using Plugin.LocalNotification;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PeopleWith; 

namespace PeopleWith
{
    public class AppointmentNotifications
    {
        bool NotificationsEnabled = Helpers.Settings.NotificationsEnabled;
        CrashDetected crashHandler = new CrashDetected();

        async public void NotasyncMethod(Exception Ex)
        {
            try
            {
                await crashHandler.SentryCrashDetected(Ex);
                //SentrySdk.ConfigureScope(scope =>
                //await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
            }
            catch (Exception ex)
            {
                //Dunno 
            }
        }

        //Cant Cancel/Delete Notification
        //public async Task CancelNotification(appointment MedtoDelete)
        //{
        //    try
        //    {
        //        //foreach (var item in MedtoDelete.schedule)
        //        //{
        //        //    int NotificationID = item.id;
        //        //    LocalNotificationCenter.Current.Cancel(NotificationID);
        //        //}
        //    }
        //    catch (Exception Ex)
        //    {
        //        NotasyncMethod(Ex);
        //    }
        //}

        public async Task<appointment> AddAppointment(appointment AddAppointment)
        {
            try
            {
                var startdateandtime = DateTime.Parse(AddAppointment.datetime) - AddAppointment.Reminder;
                var Time = DateTime.Parse(AddAppointment.datetime).ToString("HH:mm");
                var nottitle = "Appointment Reminder";
                var notdescription = string.Empty; 
                if (string.IsNullOrEmpty(AddAppointment.hcpname))
                {
                    notdescription = AddAppointment.datetimeConverted.ToString("dd/MM/yy, HH:mm") + " at " + AddAppointment.location + " (" + AddAppointment.type + ")";
                }
                else
                {
                    notdescription = AddAppointment.hcpname + " - " + AddAppointment.datetimeConverted.ToString("dd/MM/yy, HH:mm") + " at " + AddAppointment.location + " (" + AddAppointment.type + ")";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = AddAppointment.Notid,
                    Title = nottitle,
                    Description = notdescription,
                    BadgeNumber = 0,
                    Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                    {
                        Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.High, // 🔥 Set priority here
                    },
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = startdateandtime,
                        RepeatType = NotificationRepeat.No,
                        NotifyRepeatInterval = null
                    }
                };


                var check = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if (check)
                {
                    await LocalNotificationCenter.Current.Show(notification);
                }

                return null; 
            }
            catch (Exception Ex)
            {
                NotasyncMethod(Ex);
                return null; 
            }
        }

        //public async Task ApponintmentFeedbackReminder(string hcpname, string location, TimeSpan time, string startdate)
        //{
        //    try
        //    {
        //        Random random = new Random();
        //        int notid = random.Next(100000, 100000001);

        //        var sd = DateTime.Parse(startdate);
        //        var startdateandtime = sd + time;
        //        var nottitle = "How was your Appointment With " + hcpname + " ?";
        //        var notdescription = "At " + location;

        //        var notification = new NotificationRequest
        //        {
        //            NotificationId = notid,
        //            Title = nottitle,
        //            Description = notdescription,
        //            BadgeNumber = 0,
        //            Schedule = new NotificationRequestSchedule
        //            {
        //                NotifyTime = startdateandtime,
        //                RepeatType = NotificationRepeat.No,
        //                NotifyRepeatInterval = null
        //            }
        //        };

        //        if (NotificationsEnabled == true)
        //        {
        //            await LocalNotificationCenter.Current.Show(notification);
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        NotasyncMethod(Ex);
        //    }
        //}
    }
}
