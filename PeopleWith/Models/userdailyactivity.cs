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
    public class userdailyactivity : INotifyPropertyChanged
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string activityid { get; set; }
        public string activitytitle { get; set; }  
        public string activityoption { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public bool active { get; set; }      
        public string feedback { get; set; }
        public string notes { get; set; }
        public string activityfrequency { get; set; }
        public string activitysymptoms { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<activenotes> noteslist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<activefrequency> activityfrequencylist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<activesymptoms> activitysymptomslist { get; set; }
        public bool activityplanner { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string frequency { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int SelectedTimes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public class activefrequency : INotifyPropertyChanged
    {
        public string id { get; set; }
        public bool deleted { get; set; }
        public string frequency { get; set; }
        public string day { get; set; }
        public string time { get; set; }

        private TimeSpan timespan;

        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan Timespan
        {
            get { return timespan; }
            set
            {
                timespan = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Timespan)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string itemno { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool weeklyitem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class activenotes
    {
        public string displayname { get; set; }
        public string notes { get; set; }

    }

    public class activesymptoms
    {
        public string symptomid { get; set; }
        public string symptomtitle { get; set; }

    }

    public class ApiResponseUserActivity
    {
        public ObservableCollection<userdailyactivity> Value { get; set; }
    }

}

