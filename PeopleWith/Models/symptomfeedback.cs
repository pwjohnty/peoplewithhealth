using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace PeopleWith
{
    public class symptomfeedback : INotifyPropertyChanged
    {
        bool deleteCheck;
        bool dateSelected;

        public string action { get; set; }
        public string timestamp { get; set; }
        public string symptomfeedbackid { get; set; }
        public string intensity { get; set; }
        public string notes { get; set; }
        public string triggers { get; set; }
        public string interventions { get; set; }
        public string duration { get; set; }

     
        [System.Text.Json.Serialization.JsonIgnore]
        public string formattedDateTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime DateTimeFormat { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool TriggerBool { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool InterventionBool { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool OtherBool { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool DeleteCheck
        {
            get => deleteCheck;
            set
            {
                if (deleteCheck != value)
                {
                    deleteCheck = value;
                    OnPropertyChanged();
                }
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool DeleteSelected
        {
            get => dateSelected;
            set
            {
                if (dateSelected != value)
                {
                    dateSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public string triggerorIntervention { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
