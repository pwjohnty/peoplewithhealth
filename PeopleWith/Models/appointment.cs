using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class appointment
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string datetime { get; set; }
        public string reminderinterval { get; set; }
        public string hcpid { get; set; }
        public string hcpname { get; set; }
        public string reason { get; set; }
        public string type { get; set; }
        public string expectedduration { get; set; }
        public string location { get; set; }
        public string attended { get; set; }

        [JsonConverter(typeof(AppointmentFeedbackConverter))]
        public ObservableCollection<appointmentfeedback> feedback { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime datetimeConverted { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int Notid { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan Reminder { get; set; }
    }

    public class appointmentfeedback
    {
        public string ActualDuration { get; set; }
        public string DoctorsNotes { get; set; }
        public string AdditionalNotes { get; set; }
    }
    public class ApiResponeAppointment
    {
        public ObservableCollection<appointment> Value { get; set; }
    }
}
