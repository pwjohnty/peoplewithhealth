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
    public class registryDataInputs : INotifyPropertyChanged
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

        [System.Text.Json.Serialization.JsonIgnore]
        public int groupkey { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public List<CheckBoxOption> ValueInputs { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public TextOption TextInput { get; set; }
        //public ObservableCollection<CheckBoxOption> ValueInputs { get; set; }

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

        //Set Single Selection Visibility 
        private bool dropdownwother;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool DropDownwOther
        {
            get { return dropdownwother; }
            set
            {
                dropdownwother = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DropDownwOther)));
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

        //Show hide/Specify Text 
        private bool weightyearentry;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool WeightYearEntry
        {
            get { return weightyearentry; }
            set
            {
                weightyearentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightYearEntry)));
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

        //Free Text Entry 
        private string freetextentry;
        [System.Text.Json.Serialization.JsonIgnore]
        public string FreeTextEntry
        {
            get { return freetextentry; }
            set
            {
                freetextentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(FreeTextEntry)));
                }
            }
        }


        //Number freeText
        private string numberentry;
        [System.Text.Json.Serialization.JsonIgnore]
        public string NumberEntry
        {
            get { return numberentry; }
            set
            {
                numberentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NumberEntry)));
                }
            }
        }

        //Weight,Year (year Entry) 
        private string weightentrytext;
        [System.Text.Json.Serialization.JsonIgnore]
        public string WeightEntryText
        {
            get { return weightentrytext; }
            set
            {
                weightentrytext = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightEntryText)));
                }
            }
        }

        //Weight,Year (Weight Entry) 
        private string weightyearone;
        [System.Text.Json.Serialization.JsonIgnore]
        public string WeightYearOne
        {
            get { return weightyearone; }
            set
            {
                weightyearone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightYearOne)));
                }
            }
        }

        //Weight,Year (year Entry) 
        private string weightyeartwo;
        [System.Text.Json.Serialization.JsonIgnore]
        public string WeightYearTwo
        {
            get { return weightyeartwo; }
            set
            {
                weightyeartwo = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeightYearTwo)));
                }
            }
        }

        //date (Months) 
        private string dateEntry;
        [System.Text.Json.Serialization.JsonIgnore]
        public string DateEntry
        {
            get { return dateEntry; }
            set
            {
                dateEntry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateEntry)));
                }
            }
        }


        private Color numbercolour;
        [System.Text.Json.Serialization.JsonIgnore]
        public Color NumberColour
        {
            get { return numbercolour; }
            set
            {
                numbercolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NumberColour)));
                }
            }
        }

        private Color datecolour;
        [System.Text.Json.Serialization.JsonIgnore]
        public Color DateColour
        {
            get { return datecolour; }
            set
            {
                datecolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateColour)));
                }
            }
        }


        //datedate (Start) 
        private string datedatestart;
        [System.Text.Json.Serialization.JsonIgnore]
        public string DateDateStart
        {
            get { return datedatestart; }
            set
            {
                datedatestart = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateDateStart)));
                }
            }
        }

        //datedate (End) 
        private string datedateend;
        [System.Text.Json.Serialization.JsonIgnore]
        public string DateDateEnd
        {
            get { return datedateend; }
            set
            {
                datedateend = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateDateEnd)));
                }
            }
        }


        private Color datestartcolour;
        [System.Text.Json.Serialization.JsonIgnore]
        public Color DateStartColour
        {
            get { return datestartcolour; }
            set
            {
                datestartcolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateStartColour)));
                }
            }
        }


        private Color dateendcolour;
        [System.Text.Json.Serialization.JsonIgnore]
        public Color DateEndColour
        {
            get { return dateendcolour; }
            set
            {
                dateendcolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateEndColour)));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class CheckBoxOption : INotifyPropertyChanged
    {

        public string Text { get; set; }
        public string questionid { get; set; }

        private bool itemselected;
        [System.Text.Json.Serialization.JsonIgnore]
        public bool ItemSelected
        {
            get { return itemselected; }
            set
            {
                itemselected = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ItemSelected)));
                }
            }
        }

        private string datevalue;
        [System.Text.Json.Serialization.JsonIgnore]
        public string DateValue
        {
            get { return datevalue; }
            set
            {
                datevalue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateValue)));
                }
            }
        }

        private Color setdatecolour;
        [System.Text.Json.Serialization.JsonIgnore]
        public Color SetDateColour
        {
            get { return setdatecolour; }
            set
            {
                setdatecolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SetDateColour)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class TextOption : INotifyPropertyChanged
    {

        public string TextValue { get; set; }
        public string questionid { get; set; }

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
