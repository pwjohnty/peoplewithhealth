using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class usersupplement : INotifyPropertyChanged
    {

        //[JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string supplementid { get; set; }
        public string supplementtitle { get; set; }
        public string preparation { get; set; }
        public string formulation { get; set; }
        public string unit { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string frequency { get; set; }

        [JsonConverter(typeof(MedSuppTimesConverter))]
        public ObservableCollection<MedtimesDosages> schedule { get; set; }
        public string diagnosis { get; set; }
        public string status { get; set; }

        [JsonConverter(typeof(MedSuppFeedbackCoventer))]
        public ObservableCollection<MedSuppFeedback> feedback { get; set; }
        public string details { get; set; }

        public string supplementquestions { get; set; }

        public string groupscheduleid { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public List<string> TimeDosage { get; set; } = new List<string>();

        private string nexttime;

        [System.Text.Json.Serialization.JsonIgnore]
        public string NextTime
        {
            get { return nexttime; }
            set
            {
                nexttime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NextTime)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string NextDosage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime DatetimeOrder { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Dosage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string MedDateTime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Action { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Color Colour { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string EditMedSection { get; set; }

        private string medfrequency;

        [System.Text.Json.Serialization.JsonIgnore]
        public string MedFrequency
        {
            get { return medfrequency; }
            set
            {
                medfrequency = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(MedFrequency)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool EndingSoon { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public string ChangedMedName { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool ChangedMed { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string ChangedMedNotes { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool ChangedNotes { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool PendingMeds { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool ActiveMeds { get; set; }

        //Used For Double dosage Inside of Listview 

        [System.Text.Json.Serialization.JsonIgnore]
        public string UnitOne { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string UnitTwo { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DosageOne { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DosageTwo { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool SingleDosage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool DoubleDosage { get; set; }

        // event handler for updating the list views
        public event PropertyChangedEventHandler PropertyChanged;
        //  public void OnPropertyChanged()
        //{
        //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs());
        // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        // }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    public class ApiResponseUserSupplement
    {
        public ObservableCollection<usersupplement> Value { get; set; }
    }
}
