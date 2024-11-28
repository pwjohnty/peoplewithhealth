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
    public class userallergies
    {
        public ObservableCollection<userallergies> allallergies;
        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<userallergies> Allallergies
        {
            get => allallergies;
            set
            {
                allallergies = value;
                OnPropertyChanged(nameof(Allallergies));
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string createdAt { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string allergyid { get; set; }
        public string title { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public string Shorttitle { get; set; }



        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
