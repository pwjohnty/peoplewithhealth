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
    public class questionanswerinfo : INotifyPropertyChanged
    {

        //[JsonIgnore]
        public string questionid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string questiontitle { get; set; }
        public string questiondirections { get; set; }
        public string questiontype { get; set; }
        public string questionorder { get; set; }
        public string questioncatergory { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string questionnum { get; set; }

        public bool questionrequired { get; set; }

        //  [JsonConverter(typeof(QuestionnaireAnswerConverter))]
        public ObservableCollection<QuestionAnswers> questionanswers { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool singleselection { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool mulitpleselection { get; set; }



        [System.Text.Json.Serialization.JsonIgnore]
        public string answerid { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<AnswerViewModel> AnswerOptions { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string SelectedAnswer { get; set; }

        private bool addfreetext;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Addfreetext
        {
            get { return addfreetext; }
            set
            {
                addfreetext = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Addfreetext)));
                }
            }
        }

        private bool addfreetextenabled;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Addfreetextenabled
        {
            get { return addfreetextenabled; }
            set
            {
                addfreetextenabled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Addfreetextenabled)));
                }
            }
        }

        private double addfreetextopacity;

        [System.Text.Json.Serialization.JsonIgnore]
        public double Addfreetextopacity
        {
            get { return addfreetextopacity; }
            set
            {
                addfreetextopacity = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Addfreetextopacity)));
                }
            }
        }

        private bool doublenumentry;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Doublenumentry
        {
            get { return doublenumentry; }
            set
            {
                doublenumentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Doublenumentry)));
                }
            }
        }

        private bool numericentry;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Numericentry
        {
            get { return numericentry; }
            set
            {
                numericentry = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Numericentry)));
                }
            }
        }

        private bool sliderscale;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Sliderscale
        {
            get { return sliderscale; }
            set
            {
                sliderscale = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Sliderscale)));
                }
            }
        }

        private bool isrequired;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Isrequired
        {
            get { return isrequired; }
            set
            {
                isrequired = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Isrequired)));
                }
            }
        }

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
        public string answertitle { get; set; }


        private bool hasanswered;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Hasanswered
        {
            get { return hasanswered; }
            set
            {
                hasanswered = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Hasanswered)));
                }
            }
        }

        private bool answerednumericentryone;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Answerednumericentryone
        {
            get { return answerednumericentryone; }
            set
            {
                answerednumericentryone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Answerednumericentryone)));
                }
            }
        }

        private bool answerednumericentrytwo;

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Answerednumericentrytwo
        {
            get { return answerednumericentrytwo; }
            set
            {
                answerednumericentrytwo = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Answerednumericentrytwo)));
                }
            }
        }

        private List<string> selectedansweridlist = new List<string>();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<string> Selectedansweridlist
        {
            get { return selectedansweridlist; }
            set
            {
                selectedansweridlist = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Selectedansweridlist)));
                }
            }
        }


        [System.Text.Json.Serialization.JsonIgnore]
        public string selectedtextvalue { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string doubleentryone { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string doubleentrytwo { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string freetextentry { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string numericentrytext { get; set; }


        public double slidervalue;
        [System.Text.Json.Serialization.JsonIgnore]
        public double SliderValue
        {
            get { return slidervalue; }
            set
            {
                slidervalue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SliderValue)));
                }
            }
        }


        [System.Text.Json.Serialization.JsonIgnore]
        public string doubleentry1 { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string doubleentry2 { get; set; }


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

    public class QuestionAnswers
    {
        public string answerid { get; set; }
        public string answertitle { get; set; }
        public string answervalue { get; set; }

        public string answerorder { get; set; }

        public string answeroptions { get; set; }

    }

    public class AnswerViewModel
    {
        public string answertitle { get; set; }
        public bool isVisible { get; set; }

        public string questionid { get; set; }
        public string answerid { get; set; }

        public string answeroptions { get; set; }

        public string groupkey { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool selectedss { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool selectedms { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool bordervis { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string ImgSource { get; set; }


    }
}
