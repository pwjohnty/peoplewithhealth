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
    public class MedtimesDosages : INotifyPropertyChanged
    {

        //[JsonIgnore]

        public int id { get; set; }
        public string time { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan timeconverted { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        private string dosage;
        public string Dosage
        {
            get { return dosage; }
            set
            {
                dosage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Dosage)));
                }
            }
        }

        private string dosage2;


        [System.Text.Json.Serialization.JsonIgnore]
        public string Dosage2
        {
            get { return dosage2; }
            set
            {
                dosage2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Dosage2)));
                }
            }
        }

        public string dosageunit { get; set; }


        public string active { get; set; }

        public string dateadded { get; set; }
        public string frequency { get; set; }

        public string groupscheduleid { get; set; }

        public string TimeDosage { get; set; }
       
        private string day;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Day
        {
            get { return day; }
            set
            {
                day = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Day)));
                }
            }
        }

  
        private bool alldosage;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Alldosage
        {
            get { return alldosage; }
            set
            {
                alldosage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Alldosage)));
                }
            }
        }

       
        private bool checkboxchecked;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Checkboxchecked
        {
            get { return checkboxchecked; }
            set
            {
                checkboxchecked = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Checkboxchecked)));
                }
            }
        }


        private bool asReqlblVis;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool AsReqlblVis
        {
            get { return asReqlblVis; }
            set
            {
                asReqlblVis = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(AsReqlblVis)));
                }
            }
        }

        private bool asReqHidelbl;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool AsReqHidelbl
        {
            get { return asReqHidelbl; }
            set
            {
                asReqHidelbl = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(asReqHidelbl)));
                }
            }
        }


        private bool timepickerVis;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool TimepickerVis
        {
            get { return timepickerVis; }
            set
            {
                timepickerVis = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TimepickerVis)));
                }
            }
        }

        private bool labelvis;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Labelvis
        {
            get { return labelvis; }
            set
            {
                labelvis = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Labelvis)));
                }
            }
        }

     
        private bool entryvis;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Entryvis
        {
            get { return entryvis; }
            set
            {
                entryvis = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Entryvis)));
                }
            }
        }

        private bool labelvis2;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Labelvis2
        {
            get { return labelvis2; }
            set
            {
                labelvis2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Labelvis2)));
                }
            }
        }


        private bool entryvis2;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Entryvis2
        {
            get { return entryvis2; }
            set
            {
                entryvis2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Entryvis2)));
                }
            }
        }

        private string name;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private double buttonop;

        [System.Text.Json.Serialization.JsonIgnore]
        public double Buttonop
        {
            get { return buttonop; }
            set
            {
                buttonop = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Buttonop)));
                }
            }
        }

        private double buttonntop;

        [System.Text.Json.Serialization.JsonIgnore]
        public double Buttonntop
        {
            get { return buttonntop; }
            set
            {
                buttonntop = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Buttonntop)));
                }
            }
        }

        private bool buttonenabled;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Buttonenabled
        {
            get { return buttonenabled; }
            set
            {
                buttonenabled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Buttonenabled)));
                }
            }
        }


        private string feedbackid;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Feedbackid
        {
            get { return feedbackid; }
            set
            {
                feedbackid = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Feedbackid)));
                }
            }
        }


        private string usermedid;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Usermedid
        {
            get { return usermedid; }
            set
            {
                usermedid = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Usermedid)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Times { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Type { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public bool NormalDosage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool DoubleDosage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DoubleDosagetxt { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DisplayName { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool DisplayNameAdded { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int RowNum { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int RowSpan { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Color ListBackgroundColor { get; set; }



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


}
