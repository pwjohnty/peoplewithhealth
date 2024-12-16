using Newtonsoft.Json;
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
    public class usersymptom : INotifyPropertyChanged
    {
        double opacity;
        string currentIntensity;
        bool enabled;
        double slidervalue;
        string currentIntensityua;
        double slidervalueua;
        bool firstadd;

        public ObservableCollection<usersymptom> allsymptoms;
        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<usersymptom> AllSymptoms
        {
            get => allsymptoms;
            set
            {
                allsymptoms = value;
                OnPropertyChanged(nameof(AllSymptoms));
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string symptomid { get; set; }

        [JsonConverter(typeof(FeedbackConverter))]
        public ObservableCollection<symptomfeedback> feedback { get; set; }
        public string symptomtitle { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Shorttitle { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string CurrentIntensity
        {
            get => currentIntensity;
            set
            {
                if (currentIntensity != value)
                {
                    currentIntensity = value;
                    OnPropertyChanged();
                }
            }
        }


        [System.Text.Json.Serialization.JsonIgnore]
        public string CurrentIntensityUA
        {
            get => currentIntensityua;
            set
            {
                if (currentIntensityua != value)
                {
                    currentIntensityua = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string LastUpdated { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string LastUpdatedTime { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string HighIntensity { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string LowIntensity { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string IntensityAverage { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Score { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string PreviousIntensity { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public double Opacity
        {
            get => opacity;
            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public double Slidervalue
        {
            get => slidervalue;
            set
            {
                if (slidervalue != value)
                {
                    slidervalue = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public double SlidervalueUA
        {
            get => slidervalueua;
            set
            {
                if (slidervalueua != value)
                {
                    slidervalueua = value;
                    OnPropertyChanged();
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DateUpdatedAll { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Firstadd
        {
            get => firstadd;
            set
            {
                if (firstadd != value)
                {
                    firstadd = value;
                    OnPropertyChanged();
                }
            }
        }


        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    public class ApiResponseUserSymptom
    {
        public ObservableCollection<usersymptom> Value { get; set; }
    }


}
