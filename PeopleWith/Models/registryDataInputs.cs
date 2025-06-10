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
    public class registryDataInputs
    {

        //[JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        public bool deleted { get; set; }
        public string dataInputs { get; set; }
        public string dataTab { get; set; }
        public string label { get; set; }
        public string labelDirections { get; set; }
        public string type { get; set; }
        public string values { get; set; }
        public string upperLimit { get; set; }
        public string lowerLimit { get; set; }
        public bool active { get; set; }

        public string access { get; set; }

        public string subType { get; set; }

        public bool chart { get; set; }

        public bool patientAccess { get; set; }

        public string itemOrder { get; set; }

        public string inputGroup { get; set; }

        public string trigger { get; set; }

        public string webOnly { get; set; }

        public string apporder { get; set; }

        //Show Hide Data 

        private Color bordercolor;

        [System.Text.Json.Serialization.JsonIgnore]
        public Color Bordercolor
        {
            get { return bordercolor; }
            set
            {
                bordercolor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bordercolor)));
                }
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public int quesorder { get; set; }
        
        public List<CheckBoxOption> ValueInputs { get; set; }

        //Set Single Selection Visibility 
        private bool dropdown;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Dropdown
        {
            get { return dropdown; }
            set
            {
                dropdown = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Dropdown)));
                }
            }
        }


        //Show hide/Specify Text 
        private bool specifytext;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool SpecifyText
        {
            get { return specifytext; }
            set
            {
                specifytext = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SpecifyText)));
                }
            }
        }

        //Show hide/Specify Text 
        private bool weightentry;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool WeightEntry
        {
            get { return weightentry; }
            set
            {
                weightentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightEntry)));
                }
            }
        }

        //Set multiple Selection Visibility 
        private bool multiple;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Multiple
        {
            get { return multiple; }
            set
            {
                multiple = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Multiple)));
                }
            }
        }


        //Set multiple Selection Visibility 
        private bool weight;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Weight)));
                }
            }
        }


        //Set Weightyear Selection Visibility 
        private bool weightyear;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool WeightYear
        {
            get { return weightyear; }
            set
            {
                weightyear = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightYear)));
                }
            }
        }


        //Set Number Selection Visibility 
        private bool number;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Number
        {
            get { return number; }
            set
            {
                number = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }


        //Set MultipleDate Selection Visibility 
        private bool multipledate;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool MultipleDate
        {
            get { return multipledate; }
            set
            {
                multipledate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(MultipleDate)));
                }
            }
        }


        //Set DateDate Selection Visibility 
        private bool datedate;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool DateDate
        {
            get { return datedate; }
            set
            {
                datedate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateDate)));
                }
            }
        }


        //Set DateDate Selection Visibility 
        private bool date;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Date
        {
            get { return date; }
            set
            {
                date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        //Show hide/Specify Text 
        private string updatelabel;
        [System.Text.Json.Serialization.JsonIgnore]
        public string UpdateLabel
        {
            get { return updatelabel; }
            set
            {
                updatelabel = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(UpdateLabel)));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class CheckBoxOption
    {
        private bool isChecked;
        public string Text { get; set; }
        public string questionid { get; set; }
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ApiResponseregistyDataInputs
    {
        public ObservableCollection<registryDataInputs> Value { get; set; }
    }
}
