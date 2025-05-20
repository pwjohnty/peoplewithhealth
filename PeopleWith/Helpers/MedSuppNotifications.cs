using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Org.Apache.Http.Conn.Schemes;
using Plugin.LocalNotification;

namespace PeopleWith
{
  public class MedSuppNotifications
    {

 
        public async Task DailyNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate)
        {
            try
            {

                //calaulte the time to start notification


                var sd = DateTime.Parse(startdate);

                var startdateandtime = sd + time;

                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = notid,
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
                        RepeatType = NotificationRepeat.Daily,
                        NotifyRepeatInterval = null 
                    }
                     


                };

                LocalNotificationCenter.Current.Show(notification);



            }
            catch(Exception ex)
            {

            }
        }


        public async Task DailyWithEndDateNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate, string enddate)
        {
            try
            {
                var sd = DateTime.Parse(startdate);
                var ed = DateTime.Parse(enddate);
                var startdateandtime = sd + time;
                var enddatewithtime = ed + time;

                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                    {
                        NotificationId = notid, 
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
                            NotifyAutoCancelTime = enddatewithtime,
                            RepeatType = NotificationRepeat.Daily
                          
                        },
                    

                    };

                    LocalNotificationCenter.Current.Show(notification);
                
            }
            catch (Exception ex)
            {

            }
        }


        public async Task WeeklyNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate, string day)
        {
            try
            {

                //calaulte the time to start notification
                // Parse the string day
                var targetDay = ParseDay(day);

                var sd = DateTime.Parse(startdate);

                var startdateandtime = sd + time;

                if (startdateandtime.DayOfWeek == targetDay)
                {
                    // If it is the target day, check if the current time is before or after time
                    if (DateTime.Now < startdateandtime)
                    {
                        // If before time, use today
                        startdateandtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours, time.Minutes, 0);
                    }
                    else
                    {
                        // If after time, set to next target day
                        startdateandtime = startdateandtime.AddDays(7);
                    }
                }
                else
                {
                    // Find the next occurrence of the target day
                    int daysUntilTarget = ((int)targetDay - (int)startdateandtime.DayOfWeek + 7) % 7;
                    startdateandtime = startdateandtime.AddDays(daysUntilTarget);
                }


                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = notid,
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
                        RepeatType = NotificationRepeat.Weekly,
                        NotifyRepeatInterval = null
                    }



                };

                LocalNotificationCenter.Current.Show(notification);



            }
            catch (Exception ex)
            {

            }
        }

        public async Task WeeklyWithEndDateNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate, string day, string enddate)
        {
            try
            {

                //calaulte the time to start notification
                // Parse the string day
                var targetDay = ParseDay(day);

                var sd = DateTime.Parse(startdate);

                var ed = DateTime.Parse(enddate);
                var startdateandtime = sd + time;
                var enddatewithtime = ed + time;

                if (startdateandtime.DayOfWeek == targetDay)
                {
                    // If it is the target day, check if the current time is before or after time
                    if (DateTime.Now < startdateandtime)
                    {
                        // If before time, use today
                        startdateandtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours, time.Minutes, 0);
                    }
                    else
                    {
                        // If after time, set to next target day
                        startdateandtime = startdateandtime.AddDays(7);
                    }
                }
                else
                {
                    // Find the next occurrence of the target day
                    int daysUntilTarget = ((int)targetDay - (int)startdateandtime.DayOfWeek + 7) % 7;
                    startdateandtime = startdateandtime.AddDays(daysUntilTarget);
                }


                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = notid,
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
                        RepeatType = NotificationRepeat.Weekly,
                        NotifyAutoCancelTime = enddatewithtime,
                        NotifyRepeatInterval = null
                    }



                };

                LocalNotificationCenter.Current.Show(notification);



            }
            catch (Exception ex)
            {

            }
        }

        DayOfWeek ParseDay(string day)
        {
            return day switch
            {
                "Mon" => DayOfWeek.Monday,
                "Tues" => DayOfWeek.Tuesday,
                "Wed" => DayOfWeek.Wednesday,
                "Thurs" => DayOfWeek.Thursday,
                "Fri" => DayOfWeek.Friday,
                "Sat" => DayOfWeek.Saturday,
                "Sun" => DayOfWeek.Sunday,
                _ => throw new ArgumentException("Invalid day format")
            };
        }



        public async Task DaysIntervalNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate, int daycount)
        {
            try
            {

                //calaulte the time to start notification


                var sd = DateTime.Parse(startdate);

                var startdateandtime = sd + time;

                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = notid,
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
                      //  RepeatType = NotificationRepeat.TimeInterval,
                        NotifyRepeatInterval = TimeSpan.FromDays(daycount)
                    }



                };

                LocalNotificationCenter.Current.Show(notification);



            }
            catch (Exception ex)
            {

            }
        }

        public async Task DaysIntervalWithEndDateNotifications(string nottitle, int notid, string name, string dosage, string dosageunit, TimeSpan time, string startdate, string enddate, int daycount)
        {
            try
            {

                //calaulte the time to start notification


                var sd = DateTime.Parse(startdate);
                var ed = DateTime.Parse(enddate);

                var startdateandtime = sd + time;
                var endateandtime = ed + time;

                var notdescription = "";

                if (dosage.Contains("|"))
                {
                    var split = dosage.Split('|');

                    notdescription = split[0] + " " + dosageunit + " " + split[1] + " " + name + " due";
                }
                else
                {

                    notdescription = dosage + " " + dosageunit + " " + name + " due";
                }

                var notification = new NotificationRequest
                {
                    NotificationId = notid,
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
                        //  RepeatType = NotificationRepeat.TimeInterval,
                        NotifyRepeatInterval = TimeSpan.FromDays(daycount),
                        NotifyAutoCancelTime = endateandtime
                    }



                };

                LocalNotificationCenter.Current.Show(notification);



            }
            catch (Exception ex)
            {

            }
        }

    }
}
