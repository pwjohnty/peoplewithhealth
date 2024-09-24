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

        public string dosageunit { get; set; }

        private string day;
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

        private bool labelvis;
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
