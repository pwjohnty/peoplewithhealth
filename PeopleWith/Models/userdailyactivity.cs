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
        public bool activityplanner { get; set; }

        private string typeimg;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Typeimg
        {
            get { return typeimg; }
            set
            {
                typeimg = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Typeimg)));
                }
            }
        }
        //public string Typeimg { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string convertedduration { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string moodimg { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string startandduration { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string shorttitle { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime Date { get; set; }

        //New Way
        [System.Text.Json.Serialization.JsonIgnore]
        public ActivityFeedback ActivityFeedbackList { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<ActivitySymptoms> ActivitySymptomsList { get; set; }

        //Old Way 

        //[System.Text.Json.Serialization.JsonIgnore]
        //public ObservableCollection<activenotes> noteslist { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public ObservableCollection<activefrequency> activityfrequencylist { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public ObservableCollection<ActivityFeedback> activityfeedbacklist { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public string frequency { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public int SelectedTimes { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public string name { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public string displayname { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public bool displaynameAdded { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    //New Way 
    public class ActivityFeedback
    {
        public string Duration { get; set; }
        public string Outcome { get; set; }
        public string Mood { get; set; }

    }
    public class ActivitySymptoms
    {
        public string Symptomid { get; set; }
        public string Symptomtitle { get; set; }
    }

    public class ApiResponseUserActivity
    {
        public ObservableCollection<userdailyactivity> Value { get; set; }
    }

  

    //public class activefrequency : INotifyPropertyChanged
    //{
    //    public string id { get; set; }
    //    public bool deleted { get; set; }
    //    public string frequency { get; set; }
    //    public string day { get; set; }
    //    public string time { get; set; }
    //    public string duration { get; set; }

    //    private TimeSpan timespan;

    //    [System.Text.Json.Serialization.JsonIgnore]
    //    public TimeSpan Timespan
    //    {
    //        get { return timespan; }
    //        set
    //        {
    //            timespan = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Timespan)));
    //            }
    //        }
    //    }

    //    [System.Text.Json.Serialization.JsonIgnore]
    //    public string itemno { get; set; }

    //    [System.Text.Json.Serialization.JsonIgnore]
    //    public bool weeklyitem { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void OnPropertyChanged(string name)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }
    //}

    //public class ActivityFeedback
    //{
    //    public string id { get; set; }
    //    public string Recorded { get; set; }
    //    public string frequency { get; set; }
    //    public string Scheduledatetime { get; set; }
    //    public string datetimerecorded { get; set; }
    //    public string name { get; set; }
    //    public string displayname { get; set; }
    //    public string day { get; set; }

    //    [System.Text.Json.Serialization.JsonIgnore]
    //    public Color SetColour { get; set; }

    //    [System.Text.Json.Serialization.JsonIgnore]
    //    public string activityid { get; set; }
    //}

    //public class ActivityPlanner : INotifyPropertyChanged
    //{
    //    public string activityid { get; set; }
    //    public string frequencyid { get; set; }
    //    public string frequency { get; set; }
    //    public string day { get; set; }
    //    public string time { get; set; }
    //    public string Name { get; set; }
    //    public string startdate { get; set; }
    //    public string enddate { get; set; }
    //    public string duration { get; set; }
    //    public string displayname { get; set; }
    //    public string notes { get; set; }
    //    public bool FeedbackAdded { get; set; }
    //    private string datetimerecorded { get; set; }
    //    public string Datetimerecorded
    //    {
    //        get { return datetimerecorded; }
    //        set
    //        {
    //            datetimerecorded = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Datetimerecorded)));
    //            }
    //        }
    //    }
    //    private bool check { get; set; }
    //    public bool Check
    //    {
    //        get { return check; }
    //        set
    //        {
    //            check = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Check)));
    //            }
    //        }
    //    }
    //    private TextDecorations strike { get; set; }
    //    public TextDecorations Strike
    //    {
    //        get { return strike; }
    //        set
    //        {
    //            strike = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Strike)));
    //            }
    //        }
    //    }
    //    private bool showcompleted { get; set; }
    //    public bool Showcompleted
    //    {
    //        get { return showcompleted; }
    //        set
    //        {
    //            showcompleted = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Showcompleted)));
    //            }
    //        }
    //    }

    //    private bool showfrequency;
    //    public bool Showfrequency
    //    {
    //        get { return showfrequency; }
    //        set
    //        {
    //            showfrequency = value;
    //            if (PropertyChanged != null)
    //            {
    //                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Showfrequency)));
    //            }
    //        }
    //    }


    //    private bool isCheckBoxVisible;
    //    public bool IsCheckBoxVisible
    //    {
    //        get { return isCheckBoxVisible; }
    //        set
    //        {
    //            if (isCheckBoxVisible != value)
    //            {
    //                isCheckBoxVisible = value;
    //                OnPropertyChanged(nameof(IsCheckBoxVisible));
    //                OnPropertyChanged(nameof(SetOpacity));
    //                OnPropertyChanged(nameof(IsCheckBoxEnabled)); // Notify UI of enabled state
    //            }
    //        }
    //    }

    //    public double SetOpacity => IsCheckBoxVisible ? 1.0 : 0.0;
    //    public bool IsCheckBoxEnabled => IsCheckBoxVisible && SetOpacity == 1.0;

    //    public string frequencystring { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void OnPropertyChanged(string name)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }

    //}

    //public class activenotes
    //{
    //    public string displayname { get; set; }
    //    public string notes { get; set; }

    //}

}

