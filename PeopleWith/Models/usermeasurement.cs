using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class usermeasurement : INotifyPropertyChanged
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string measurementid { get; set; }
        public string measurementname { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public string status { get; set; }

        public bool hcpinput { get; set; }

        public string inputmethod { get; set; }
        public string inputtype { get; set; }
        public string inputdatetime { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string datechanged { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public double numconverted { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public double numconvertedtwo { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime dateconverted { get; set; }

        private bool deleteisvis;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Deleteisvis
        {
            get { return deleteisvis; }
            set
            {
                deleteisvis = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Deleteisvis)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string BPone { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string BPtwo { get; set; }



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

    public class ApiResponseUserMeasurement
    {
        public ObservableCollection<usermeasurement> Value { get; set; }
    }
}
