using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class appointmentcalendar
    {
        public string Appointid { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAllDay { get; set; }
        public string EventName { get; set; }
        //public TimeZoneInfo StartTimeZone { get; set; }
        //public TimeZoneInfo EndTimeZone { get; set; }
        public Brush Background { get; set; }
        public Color TextColor { get; set; }
    }
}
