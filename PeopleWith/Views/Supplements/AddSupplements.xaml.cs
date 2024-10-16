using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq.Expressions;
using Plugin.LocalNotification;
using Mopups.Services;
using Microsoft.Maui;
using Color = Microsoft.Maui.Graphics.Color;
using Syncfusion.Maui.ListView.Helpers;
using System.Linq;
//using Android.App;
//using Xamarin.Google.Crypto.Tink.Proto;
//using Windows.Foundation.Metadata;

namespace PeopleWith;

public partial class AddSupplement : ContentPage
{
    HttpClient client = new HttpClient();
    ObservableCollection<supplement> allmedicationlist = new ObservableCollection<supplement>();
    ObservableCollection<supplement> FilterResults = new ObservableCollection<supplement>();
    usersupplement newusermedication = new usersupplement();
    APICalls aPICalls = new APICalls();
    MedSuppNotifications ScheduleNotifications = new MedSuppNotifications();
    ObservableCollection<preparation> MedPreparations = new ObservableCollection<preparation>();
    List<string> medformulations = new List<string>();
    List<string> meddosages = new List<string>();
    ObservableCollection<preparation> medfreq = new ObservableCollection<preparation>();
    List<string> timesperdaylist = new List<string>();
    List<string> weekdayslist = new List<string>();
    List<string> daysintervallist = new List<string>();
    List<string> ODlist = new List<string>();
    List<string> weekfreqlist = new List<string>();
    usersupplement SelectedMed = new usersupplement();
    bool IsEdit = false;

    List<string> timesfordailylist = new List<string>();
    public string usermedunit;

    public Dictionary<string, List<MedtimesDosages>> frequencyOptions;
    public Dictionary<string, List<MedtimesDosages>> frequencyOptionsWDI;
    public Dictionary<string, int> dayOrder;


    public List<MedtimesDosages> selectedDosages = new List<MedtimesDosages>();
    public List<MedtimesDosages> WeeklyChangedselectedDosages = new List<MedtimesDosages>();
    List<string> weekdaycountlist = new List<string>();
    string weeklynumperday;
    List<string> weekdaynamelist = new List<string>();
    List<string> weeklydosagesame = new List<string>();
    string freqstring;
    bool TimeOfDaySelected = false;
    string DailysameDosageSelected;
    string WeeklyDaysSelected;
    string WeeklyHowManyTimes;
    string WeeklysameDosageSelected;
    private int clickCount = 0;

    ObservableCollection<MedtimesDosages> medtimesanddosages = new ObservableCollection<MedtimesDosages>();
    ObservableCollection<usersupplement> UserMedications = new ObservableCollection<usersupplement>();
    public AddSupplement()
    {
        InitializeComponent();


        Getmedications();

        var new1 = new preparation();
        new1.title = "Daily";
        medfreq.Add(new1);

        var new2 = new preparation();
        new2.title = "Specfic Days of the Week";
        medfreq.Add(new2);

        var new3 = new preparation();
        new3.title = "Days Interval";
        medfreq.Add(new3);

        var new4 = new preparation();
        new4.title = "As Required";
        medfreq.Add(new4);


        medfreqlistview.ItemsSource = medfreq;


        timesperdaylist.Add("OD\nOnce Daily");
        timesperdaylist.Add("BD\nTwice Daily");
        timesperdaylist.Add("TDS\nThree Times Daily");
        timesperdaylist.Add("QDS\nFour Times Daily");
        timesperdaylist.Add("5x\nFive Times Daily");
        timesperdaylist.Add("6x\nSix Times Daily");
        timesperdaylist.Add("7x\nSeven Times Daily");
        timesperdaylist.Add("8x\nEight Times Daily");

        dailytimeslistview.ItemsSource = timesperdaylist;


        ODlist.Add("MANE\nMorning");
        ODlist.Add("NOCTE\nNight");
        ODlist.Add("Set\nown time");

        oncedailylistview.ItemsSource = ODlist;



        weekdayslist.Add("Sun");
        weekdayslist.Add("Mon");
        weekdayslist.Add("Tues");
        weekdayslist.Add("Wed");
        weekdayslist.Add("Thrus");
        weekdayslist.Add("Fri");
        weekdayslist.Add("Sat");

        weeklydayslist.ItemsSource = weekdayslist;

        weekfreqlist.Add("One");
        weekfreqlist.Add("Two");
        weekfreqlist.Add("Three");
        weekfreqlist.Add("Four");
        weekfreqlist.Add("Five");
        weekfreqlist.Add("Six");
        weekfreqlist.Add("Seven");
        weekfreqlist.Add("Eight");

        weekfreqlistview.ItemsSource = weekfreqlist;
        ditimesperdaylist.ItemsSource = weekfreqlist;


        daysintervallist.Add("Every other day");
        daysintervallist.Add("Every 3 days");
        daysintervallist.Add("Every 7 days");
        daysintervallist.Add("Every 14 days");
        daysintervallist.Add("Every 21 days");
        daysintervallist.Add("Every 28 days");
        daysintervallist.Add("Every 30 days");
        daysintervallist.Add("Every 5 weeks");
        daysintervallist.Add("Every 6 weeks");
        daysintervallist.Add("Every 7 weeks");
        daysintervallist.Add("Every 8 weeks");
        daysintervallist.Add("Every 9 weeks");
        daysintervallist.Add("Every 10 weeks");

        daysintervallistview.ItemsSource = daysintervallist;

        weeklydosagesame.Add("Yes");
        weeklydosagesame.Add("No");

        samedosageweeklylist.ItemsSource = weeklydosagesame;
        samedosageweeklylist2.ItemsSource = weeklydosagesame;
        disamedosagequestionlist.ItemsSource = weeklydosagesame;

        // dosageunitlbl.Text = newusermedication.unit;



        // Define a dictionary to store the options and their corresponding MedtimesDosages
        frequencyOptions = new Dictionary<string, List<MedtimesDosages>>()
{
    { "Morning", new List<MedtimesDosages> {
         new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
       { "Night", new List<MedtimesDosages> {
         new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
          { "own time", new List<MedtimesDosages> {
         new MedtimesDosages { time = DateTime.Now.ToString("HH:mm"), timeconverted = DateTime.Now.TimeOfDay, Dosage2 = string.Empty, Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Twice Daily", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
    {
    "Three Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
       {
    "Four Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
                        {
    "Six Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};

        frequencyOptionsWDI = new Dictionary<string, List<MedtimesDosages>>()
{
    { "One", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
    {
    "Two", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Three", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
       {
    "Four", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
                        {
    "Six", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};


        dayOrder = new Dictionary<string, int>
{
    { "Sun", 1 },
    { "Mon", 2 },
    { "Tues", 3 },
    { "Wed", 4 },
    { "Thurs", 5 },
    { "Fri", 6 },
    { "Sat", 7 }
};


    }


    public AddSupplement(ObservableCollection<usersupplement> AllUsermedications, usersupplement MedSelected)
    {
        InitializeComponent();


        UserMedications = AllUsermedications;
        if (string.IsNullOrEmpty(MedSelected.id))
        {

        }
        else
        {

            // newusermedication = MedSelected;
            SelectedMed = MedSelected;
            IsEdit = true;
            editframe.IsVisible = true;
            topprogress.IsVisible = true;
            //loadingstack.IsVisible = false;
            //datastack.IsVisible = true;

        }

        Getmedications();

        var new1 = new preparation();
        new1.title = "Daily";
        medfreq.Add(new1);

        var new2 = new preparation();
        new2.title = "Specfic Days of the Week";
        medfreq.Add(new2);

        var new3 = new preparation();
        new3.title = "Days Interval";
        medfreq.Add(new3);

        var new4 = new preparation();
        new4.title = "As Required";
        medfreq.Add(new4);


        medfreqlistview.ItemsSource = medfreq;


        timesperdaylist.Add("OD\nOnce Daily");
        timesperdaylist.Add("BD\nTwice Daily");
        timesperdaylist.Add("TDS\nThree Times Daily");
        timesperdaylist.Add("QDS\nFour Times Daily");
        timesperdaylist.Add("5x\nFive Times Daily");
        timesperdaylist.Add("6x\nSix Times Daily");
        timesperdaylist.Add("7x\nSeven Times Daily");
        timesperdaylist.Add("8x\nEight Times Daily");

        dailytimeslistview.ItemsSource = timesperdaylist;


        ODlist.Add("MANE\nMorning");
        ODlist.Add("NOCTE\nNight");
        ODlist.Add("Set\nown time");

        oncedailylistview.ItemsSource = ODlist;



        weekdayslist.Add("Sun");
        weekdayslist.Add("Mon");
        weekdayslist.Add("Tues");
        weekdayslist.Add("Wed");
        weekdayslist.Add("Thrus");
        weekdayslist.Add("Fri");
        weekdayslist.Add("Sat");

        weeklydayslist.ItemsSource = weekdayslist;

        weekfreqlist.Add("One");
        weekfreqlist.Add("Two");
        weekfreqlist.Add("Three");
        weekfreqlist.Add("Four");
        weekfreqlist.Add("Five");
        weekfreqlist.Add("Six");
        weekfreqlist.Add("Seven");
        weekfreqlist.Add("Eight");

        weekfreqlistview.ItemsSource = weekfreqlist;
        ditimesperdaylist.ItemsSource = weekfreqlist;


        daysintervallist.Add("Every other day");
        daysintervallist.Add("Every 3 days");
        daysintervallist.Add("Every 7 days");
        daysintervallist.Add("Every 14 days");
        daysintervallist.Add("Every 21 days");
        daysintervallist.Add("Every 28 days");
        daysintervallist.Add("Every 30 days");
        daysintervallist.Add("Every 5 weeks");
        daysintervallist.Add("Every 6 weeks");
        daysintervallist.Add("Every 7 weeks");
        daysintervallist.Add("Every 8 weeks");
        daysintervallist.Add("Every 9 weeks");
        daysintervallist.Add("Every 10 weeks");

        daysintervallistview.ItemsSource = daysintervallist;

        weeklydosagesame.Add("Yes");
        weeklydosagesame.Add("No");

        samedosageweeklylist.ItemsSource = weeklydosagesame;
        samedosageweeklylist2.ItemsSource = weeklydosagesame;
        disamedosagequestionlist.ItemsSource = weeklydosagesame;

        // dosageunitlbl.Text = newusermedication.unit;

        if (IsEdit)
        {
            newusermedication.unit = MedSelected.unit;
        }

        // Define a dictionary to store the options and their corresponding MedtimesDosages
        frequencyOptions = new Dictionary<string, List<MedtimesDosages>>()
{
    { "Morning", new List<MedtimesDosages> {
         new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
       { "Night", new List<MedtimesDosages> {
         new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
          { "own time", new List<MedtimesDosages> {
         new MedtimesDosages { time = DateTime.Now.ToString("HH:mm"), timeconverted = DateTime.Now.TimeOfDay, Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Twice Daily", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
    {
    "Three Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
       {
    "Four Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
            {
    "Five Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};

        frequencyOptionsWDI = new Dictionary<string, List<MedtimesDosages>>()
{
    { "One", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Two", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
    {
    "Three", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
       {
    "Four", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,   Dosage2 = string.Empty, dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  Dosage2 = string.Empty, dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty, Dosage2 = string.Empty,  dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};


        dayOrder = new Dictionary<string, int>
{
    { "Sun", 1 },
    { "Mon", 2 },
    { "Tues", 3 },
    { "Wed", 4 },
    { "Thurs", 5 },
    { "Fri", 6 },
    { "Sat", 7 }
};


    }

    async void PopulateEditMed()
    {

        try
        {
            //Details 
            if (SelectedMed.EditMedSection == "Details")
            {
                // nextbtn.Text = "Update Details";
                // topprogress.IsVisible = false; 
                firststack.IsVisible = false;
                secondstack.IsVisible = true;
                mednamelbl.Text = SelectedMed.supplementtitle;
                medpreparationslistview.ItemsSource = MedPreparations;

                if (SelectedMed.status == "Pending")
                {
                    IsEdit = false;
                    newusermedication = SelectedMed;
                    mednamelbl.Text = SelectedMed.supplementtitle;
                    medname2lbl.Text = SelectedMed.supplementtitle;
                    medname3lbl.Text = SelectedMed.supplementtitle;
                    medname4lbl.Text = SelectedMed.supplementtitle;
                    medname5lbl.Text = SelectedMed.supplementtitle;
                    return;
                }

                for (int i = 0; i < MedPreparations.Count; i++)
                {
                    if (MedPreparations[i].title == SelectedMed.preparation)
                    {
                        medformulations = MedPreparations[i].formulation.Split(',').ToList();
                        meddosages = MedPreparations[i].unit.Split(',').ToList();
                    }
                }

                medformulationslistview.ItemsSource = medformulations;
                mflbl.IsVisible = true;
                mf2lbl.IsVisible = true;
                medformulationslistview.IsVisible = true;

                meddosageunitlistview.ItemsSource = meddosages;
                dflbl.IsVisible = true;
                df2lbl.IsVisible = true;
                meddosageunitlistview.IsVisible = true;

                //newusermedication.preparation = item.title;


                //Preperation 
                var selectedPreparation = MedPreparations.FirstOrDefault(p => p.title == SelectedMed.preparation);


                if (selectedPreparation != null)
                {
                    medpreparationslistview.SelectedItem = selectedPreparation;
                }

                //Medication Formulation 
                if (SelectedMed.formulation != null)
                {
                    medformulationslistview.SelectedItem = SelectedMed.formulation;
                }


                //Dosageunit 
                if (SelectedMed.unit != null)
                {
                    meddosageunitlistview.SelectedItem = SelectedMed.unit;
                }

            }
            //Edit Schedule 
            else
            {
                topprogress.SegmentCount = 4;
                topprogress.IsVisible = true;
                topprogress.Progress = 25;
                firststack.IsVisible = false;
                thirdstack.IsVisible = true;
                medname2lbl.Text = SelectedMed.supplementtitle;

                //Start and EndDate
                startdatepicker.Date = DateTime.Parse(SelectedMed.startdate);

                if (string.IsNullOrEmpty(SelectedMed.enddate))
                {
                    enddatepicker.Date = DateTime.Parse(SelectedMed.enddate);
                    enddatecheck.IsChecked = true;
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

    async void Getmedications()
    {
        try
        {
            await RetrieveallMedications();

        }
        catch (Exception ex)
        {

        }
    }

    async Task RetrieveallMedications()
    {
        try
        {

            //get all medications
            // medsloading.IsVisible = true;
            if (!IsEdit)
            {

                loadingstack.IsVisible = true;
                nextbtn.IsVisible = false;
                backbtn.IsVisible = false;

                var urlmedications = APICalls.GetSupplements;

                HttpResponseMessage responsemeds = await client.GetAsync(urlmedications);

                if (responsemeds.IsSuccessStatusCode)
                {
                    string contentmeds = await responsemeds.Content.ReadAsStringAsync();
                    var userResponsemed = JsonConvert.DeserializeObject<ApiResponseSupplement>(contentmeds);
                    var meds = userResponsemed.Value;

                    allmedicationlist = meds;




                    foreach (var item in UserMedications)
                    {
                        var med = allmedicationlist.Where(x => x.supplementid == item.supplementid).FirstOrDefault();

                        med.AlreadySelected = true;
                    }

                    // medsloading.IsVisible = false;
                    Medicationslistview.ItemsSource = allmedicationlist;


                    loadingstack.IsVisible = false;
                    nextbtn.IsVisible = true;
                    backbtn.IsVisible = true;
                }
            }

            //get the preparations
            var getMedPreperation = aPICalls.GetMedPreparation();

            MedPreparations = await getMedPreperation;

            if (IsEdit == true)
            {
                firststack.IsVisible = false;
                PopulateEditMed();
            }
            else
            {
                firststack.IsVisible = true;
                topprogress.IsVisible = true;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                FilterResults.Clear();
                Medicationslistview.IsVisible = false;
            }
            else
            {
                var countofcharacters = e.NewTextValue.Length;

                if (countofcharacters > 2)
                {
                    var Characters = e.NewTextValue;
                    var filteredmeds = new ObservableCollection<supplement>(allmedicationlist.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);

                    Medicationslistview.ItemsSource = filteredmeds;
                    Medicationslistview.IsVisible = true;
                    // Medicationslistview.HeightRequest = filteredmeds.Count * 50;
                }

            }

        }
        catch (Exception ex)
        {

        }
    }

    private async void Medicationslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            searchbar.IsEnabled = false;
            searchbar.IsEnabled = true;

            if (searchbar.IsSoftInputShowing())
                await searchbar.HideSoftInputAsync(System.Threading.CancellationToken.None);

            var item = e.DataItem as supplement;

            if (item.AlreadySelected)
            {
                Medicationslistview.SelectedItem = null;
                Medicationslistview.SelectedItems.Clear();
                return;
            }


            newusermedication.supplementid = item.supplementid;
            newusermedication.supplementtitle = item.title;

            mednamelbl.Text = item.title;
            medname2lbl.Text = item.title;
            medname3lbl.Text = item.title;
            medname4lbl.Text = item.title;
            medname5lbl.Text = item.title;


        }
        catch (Exception ex)
        {
        }
    }


    private async void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (IsEdit == true)
            {

                if (secondstack.IsVisible == true)
                {

                    //Preperation 
                    if (medpreparationslistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        return;
                        //SelectedMed.preparation = newusermedication.preparation;
                        // SelectedMed.preparation = medpreparationslistview.SelectedItem.ToString();
                    }


                    //Medication Formulation 
                    if (medformulationslistview.SelectedItem != null)
                    {

                        // SelectedMed.formulation = medformulationslistview.SelectedItem.ToString();
                    }


                    //Dosageunit 
                    if (meddosageunitlistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        return;
                        //SelectedMed.unit = meddosageunitlistview.SelectedItem.ToString();

                    }

                    //Update items 
                    var id = SelectedMed.id;
                    // APICalls database = new APICalls();
                    //   await database.UpdateMedicationDetails(SelectedMed);

                    //Update Schedule unit 

                    //update Notifications to include new unit 

                    secondstack.IsVisible = false;
                    thirdstack.IsVisible = true;
                    medname2lbl.IsVisible = true;
                    medname2lbl.Text = SelectedMed.supplementtitle;
                    backbtn.Text = "Back";


                    //Start and EndDate
                    startdatepicker.Date = DateTime.Parse(SelectedMed.startdate);

                    if (!string.IsNullOrEmpty(SelectedMed.enddate))
                    {
                        enddatepicker.Date = DateTime.Parse(SelectedMed.enddate);
                        enddatecheck.IsChecked = true;
                    }

                }
                else if (thirdstack.IsVisible == true)
                {
                    medname3lbl.Text = SelectedMed.supplementtitle;
                    backbtn.Text = "Back";
                    thirdstack.IsVisible = false;
                    fourthstack.IsVisible = true;
                    topprogress.Progress = 50;
                    medfreqlistview.IsVisible = true;


                    if (enddatecheck.IsChecked)
                    {
                        SelectedMed.enddate = enddatepicker.Date.ToString("dd/MM/yyyy");
                    }

                    SelectedMed.startdate = startdatepicker.Date.ToString("dd/MM/yyyy");

                    string SelectedFreq;
                    if (SelectedMed.frequency.Contains("|"))
                    {
                        var freq = SelectedMed.frequency.Split('|');
                        if (freq[0] == "Weekly" || freq[0] == "Weekly ")
                        {
                            var selectedFrequency = medfreq.FirstOrDefault(p => p.title == "Specfic Days of the Week");
                            medfreqlistview.SelectedItem = selectedFrequency;
                            SelectedFreq = "Specfic Days of the Week";
                        }
                        else
                        {
                            var selectedFrequency = medfreq.FirstOrDefault(p => p.title == freq[0]);
                            medfreqlistview.SelectedItem = selectedFrequency;
                            SelectedFreq = freq[0];
                        }
                    }
                    else
                    {
                        var selectedFrequency = medfreq.FirstOrDefault(p => p.title == "As Required");
                        medfreqlistview.SelectedItem = selectedFrequency;
                        SelectedFreq = "As Required";
                    }

                    freqstring = SelectedFreq;
                    timeanddosagelbl2.Text = "Select your supplement times and dosages from the list below. To adjust the time, simply tap on it.";

                    if (SelectedFreq == "As Required")
                    {

                        //timeanddosagelbl2.Text = "Select your dosage from the item below";

                    }
                    else if (SelectedFreq == "Specfic Days of the Week")
                    {
                        samedosageweeklylist.SelectedItems.Clear();
                        samedosageweeklylist2.SelectedItems.Clear();
                        weeklydayslist.IsVisible = true;
                        weekfreqlbl.IsVisible = true;
                        weekfreqlistview.IsVisible = true;
                        freqlbl.Text = "Which Days";

                        //Set Days 
                        var freq = SelectedMed.frequency.Split('|');
                        var getdays = freq[1];
                        if (getdays.Contains(","))
                        {
                            var days = freq[1].Split(',').ToList();
                            for (int i = 0; i < days.Count; i++)
                            {
                                weeklydayslist.SelectedItem = days[i];
                            }

                        }
                        else
                        {
                            weeklydayslist.SelectedItem = getdays;
                        }
                        var gettimes = freq[2];
                        string numberAsText = NumberToText(Int32.Parse(gettimes));
                        weekfreqlistview.SelectedItem = weekfreqlist.FirstOrDefault(p => p == numberAsText);

                        var daycount = Convert.ToInt32(freq[2]);

                        //check if dosage is same across all days 
                        if (daycount > 1)
                        {
                            sameweeklydosage.IsVisible = true;
                            samedosageweeklylist.IsVisible = true;

                            if (SelectedMed.supplementquestions.Contains("|"))
                            {
                                //means both questions answered

                                samedosageweeklylist.SelectedItems.Add("Yes");

                                var split = SelectedMed.supplementquestions.Split('|');

                                if (split[1] == "Yes")
                                {
                                    samedosageweeklylist2.SelectedItems.Add("Yes");

                                    adddosagelbl.IsVisible = true;
                                    adddosage2lbl.IsVisible = true;

                                    adddosageframe.IsVisible = true;

                                    samedosageentry.Text = SelectedMed.schedule[0].Dosage;
                                    dosageunitlbl.Text = SelectedMed.schedule[0].dosageunit;
                                }
                                else
                                {

                                    samedosageweeklylist2.SelectedItems.Add("No");
                                }



                                sameweeklydosage2.IsVisible = true;
                                samedosageweeklylist2.IsVisible = true;

                                weeklynumperday = weekfreqlistview.SelectedItem.ToString();

                                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                                listofdays = listofdays.OrderBy(x => dayOrder[x]).ToList();

                                string joinedDays = String.Join(", ", listofdays);


                                if (split[1] == "Yes")
                                {
                                    var i = 0;

                                    foreach (var itemm in selectedDosages)
                                    {
                                        itemm.Labelvis = true;
                                        itemm.Entryvis = false;
                                        itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                                        itemm.Dosage = SelectedMed.schedule[0].Dosage;
                                        itemm.TimepickerVis = true;
                                        itemm.AsReqlblVis = false;
                                        itemm.time = SelectedMed.schedule[i].time;
                                        var timeconverted = TimeSpan.Parse(SelectedMed.schedule[i].time);
                                        itemm.timeconverted = timeconverted;
                                        itemm.Day = joinedDays;
                                        i++;
                                    }
                                }
                                else
                                {
                                    var i = 0;

                                    foreach (var itemm in selectedDosages)
                                    {
                                        itemm.Labelvis = true;
                                        itemm.Entryvis = false;
                                        itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                                        itemm.Dosage = SelectedMed.schedule[i].Dosage;
                                        itemm.TimepickerVis = true;
                                        itemm.AsReqlblVis = false;
                                        itemm.time = SelectedMed.schedule[i].time;
                                        var timeconverted = TimeSpan.Parse(SelectedMed.schedule[i].time);
                                        itemm.timeconverted = timeconverted;
                                        itemm.Day = joinedDays;
                                        i++;
                                    }
                                }


                                //selectedDosages = selectedDosages.OrderBy(md => dayOrder[md.Day]).ToList();

                            }
                            else
                            {
                                samedosageweeklylist.SelectedItems.Add("No");

                                weeklynumperday = weekfreqlistview.SelectedItem.ToString();

                                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                                listofdays = listofdays.OrderBy(x => dayOrder[x]).ToList();

                                var ii = 0;

                                foreach (var d in listofdays) // Iterate over the list of days first
                                {
                                    for (int i = 0; i < selectedDosages.Count; i++) // Then iterate over selectedDosages for each day
                                    {
                                        var md = selectedDosages[i];
                                        Random random = new Random();
                                        int randomNumber = random.Next(100000, 100000001);

                                        var timeconverted = TimeSpan.Parse(SelectedMed.schedule[ii].time);
                                        // Create a new instance of MedtimesDosages and copy the relevant properties
                                        var newMd = new MedtimesDosages
                                        {
                                            id = randomNumber,
                                            Dosage = SelectedMed.schedule[ii].Dosage,
                                            dosageunit = md.dosageunit,
                                            time = SelectedMed.schedule[ii].time,
                                            timeconverted = timeconverted,
                                            Day = d // Assign the current day
                                        };

                                        WeeklyChangedselectedDosages.Add(newMd); // Add the new instance to the collection
                                        ii++;
                                    }
                                }

                                selectedDosages = WeeklyChangedselectedDosages;

                                foreach (var itemm in selectedDosages)
                                {
                                    itemm.Labelvis = false;
                                    itemm.Entryvis = true;

                                    itemm.TimepickerVis = true;
                                    itemm.AsReqlblVis = false;

                                }


                            }



                        }
                        else
                        {
                            //one time
                            sameweeklydosage.IsVisible = true;
                            samedosageweeklylist.IsVisible = true;
                            samedosageweeklylist.SelectedItems.Add(SelectedMed.supplementquestions);

                            if (SelectedMed.supplementquestions == "Yes")
                            {
                                adddosagelbl.IsVisible = true;
                                adddosage2lbl.IsVisible = true;

                                adddosageframe.IsVisible = true;

                                samedosageentry.Text = SelectedMed.schedule[0].Dosage;
                                dosageunitlbl.Text = SelectedMed.schedule[0].dosageunit;


                                weeklynumperday = weekfreqlistview.SelectedItem.ToString();

                                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                                listofdays = listofdays.OrderBy(x => dayOrder[x]).ToList();

                                string joinedDays = String.Join(", ", listofdays);

                                var ii = 0;

                                foreach (var d in listofdays) // Iterate over the list of days first
                                {
                                    for (int i = 0; i < selectedDosages.Count; i++) // Then iterate over selectedDosages for each day
                                    {
                                        var md = selectedDosages[i];
                                        Random random = new Random();
                                        int randomNumber = random.Next(100000, 100000001);

                                        var timeconverted = TimeSpan.Parse(SelectedMed.schedule[ii].time);
                                        // Create a new instance of MedtimesDosages and copy the relevant properties
                                        var newMd = new MedtimesDosages
                                        {
                                            id = randomNumber,
                                            Dosage = SelectedMed.schedule[0].Dosage,
                                            dosageunit = md.dosageunit,
                                            time = SelectedMed.schedule[ii].time,
                                            timeconverted = timeconverted,
                                            Day = d // Assign the current day
                                        };

                                        WeeklyChangedselectedDosages.Add(newMd); // Add the new instance to the collection
                                        ii++;
                                    }
                                }

                                selectedDosages = WeeklyChangedselectedDosages;

                                foreach (var itemm in selectedDosages)
                                {
                                    itemm.Labelvis = true;
                                    itemm.Entryvis = false;

                                    itemm.TimepickerVis = true;
                                    itemm.AsReqlblVis = false;
                                }

                            }
                            else
                            {
                                weeklynumperday = weekfreqlistview.SelectedItem.ToString();

                                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                                listofdays = listofdays.OrderBy(x => dayOrder[x]).ToList();

                                string joinedDays = String.Join(", ", listofdays);

                                var ii = 0;

                                foreach (var d in listofdays) // Iterate over the list of days first
                                {
                                    for (int i = 0; i < selectedDosages.Count; i++) // Then iterate over selectedDosages for each day
                                    {
                                        var md = selectedDosages[i];
                                        Random random = new Random();
                                        int randomNumber = random.Next(100000, 100000001);

                                        var timeconverted = TimeSpan.Parse(SelectedMed.schedule[ii].time);
                                        // Create a new instance of MedtimesDosages and copy the relevant properties
                                        var newMd = new MedtimesDosages
                                        {
                                            id = randomNumber,
                                            Dosage = SelectedMed.schedule[ii].Dosage,
                                            dosageunit = md.dosageunit,
                                            time = SelectedMed.schedule[ii].time,
                                            timeconverted = timeconverted,
                                            Day = d // Assign the current day
                                        };

                                        WeeklyChangedselectedDosages.Add(newMd); // Add the new instance to the collection
                                        ii++;
                                    }
                                }

                                selectedDosages = WeeklyChangedselectedDosages;

                                foreach (var itemm in selectedDosages)
                                {
                                    itemm.Labelvis = false;
                                    itemm.Entryvis = true;

                                    itemm.TimepickerVis = true;
                                    itemm.AsReqlblVis = false;
                                }




                            }


                        }


                        timeanddosagelbl.IsVisible = true;
                        timeanddosagelbl2.IsVisible = true;
                        timesanddosageslistview.IsVisible = true;

                        foreach (var itemm in selectedDosages)
                        {

                            if (itemm.dosageunit.Contains("per"))
                            {
                                itemm.DoubleDosage = true;
                                itemm.NormalDosage = false;
                                var splitdosage = itemm.Dosage.Split('|');
                                itemm.Dosage = splitdosage[0];
                                itemm.Dosage2 = splitdosage[1];

                            }
                            else
                            {
                                itemm.DoubleDosage = false;
                                itemm.NormalDosage = true;

                            }

                        }

                        if (selectedDosages[0].dosageunit.Contains("per"))
                        {
                            samedosageentry2.IsVisible = true;
                        }
                        else
                        {
                            samedosageentry2.IsVisible = false;
                        }


                        timesanddosageslistview.ItemsSource = selectedDosages;




                    }
                    else if (SelectedFreq == "Daily")
                    {
                        dailytimeslistview.IsVisible = true;
                        onedailylbl.IsVisible = true;
                        oncedailylistview.IsVisible = true;

                        freqlbl.Text = "How many times per day";

                        var freq = SelectedMed.frequency.Split('|');

                        var checkdailycount = Convert.ToInt32(freq[1]);

                        checkdailycount = checkdailycount - 1;

                        dailytimeslistview.SelectedItems.Clear();

                        dailytimeslistview.SelectedItems.Add(timesperdaylist[checkdailycount]);



                        //check the time
                        if (checkdailycount == 0)
                        {
                            // Get the corresponding MedtimesDosages list for the selected option




                            //means there is only one time
                            onedailylbl.IsVisible = true;
                            oncedailylistview.IsVisible = true;

                            oncedailylistview.SelectedItems.Clear();

                            if (SelectedMed.schedule[0].time == "08:00")
                            {
                                oncedailylistview.SelectedItems.Add(ODlist[0]);
                            }
                            else if (SelectedMed.schedule[0].time == "20:00")
                            {
                                oncedailylistview.SelectedItems.Add(ODlist[1]);
                            }
                            else
                            {
                                oncedailylistview.SelectedItems.Add(ODlist[2]);
                            }

                            var itemoflist = oncedailylistview.SelectedItem.ToString();

                            var splititem = itemoflist.Split('\n');
                            selectedDosages = frequencyOptions[splititem[1]];

                            //selectedDosages[0].Alldosage = false;

                            foreach (var itemm in selectedDosages)
                            {
                                itemm.Labelvis = false;
                                itemm.Entryvis = true;
                                itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                                itemm.Dosage = SelectedMed.schedule[0].Dosage;
                                itemm.TimepickerVis = true;
                                itemm.AsReqlblVis = false;
                                itemm.time = SelectedMed.schedule[0].time;
                                var timeconverted = TimeSpan.Parse(SelectedMed.schedule[0].time);
                                itemm.timeconverted = timeconverted;

                            }


                        }
                        else
                        {
                            // Get the corresponding MedtimesDosages list for the selected option
                            var itemoflist = dailytimeslistview.SelectedItem.ToString();

                            var splititem = itemoflist.Split('\n');
                            selectedDosages = frequencyOptions[splititem[1]];

                            // selectedDosages[0].Alldosage = false;

                            onedailylbl.IsVisible = false;
                            oncedailylistview.IsVisible = false;

                            sameweeklydosage2.IsVisible = true;
                            samedosageweeklylist2.IsVisible = true;

                            //check if dosages are same
                            bool areAllDosagesSame = SelectedMed.schedule.All(m => m.Dosage == SelectedMed.schedule[0].Dosage);

                            if (areAllDosagesSame)
                            {

                                samedosageweeklylist2.SelectedItem = "Yes";

                                adddosagelbl.IsVisible = true;
                                adddosage2lbl.IsVisible = true;
                                adddosageframe.IsVisible = true;

                                samedosageentry.Text = SelectedMed.schedule[0].Dosage;
                                dosageunitlbl.Text = SelectedMed.schedule[0].dosageunit;

                                var ii = 0;

                                foreach (var itemm in selectedDosages)
                                {
                                    itemm.Labelvis = true;
                                    itemm.Entryvis = false;
                                    itemm.Dosage = SelectedMed.schedule[0].Dosage;
                                    itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                                    itemm.TimepickerVis = true;
                                    itemm.AsReqlblVis = false;
                                    itemm.time = SelectedMed.schedule[ii].time;
                                    var timeconverted = TimeSpan.Parse(SelectedMed.schedule[ii].time);
                                    itemm.timeconverted = timeconverted;
                                    ii++;
                                }


                            }
                            else
                            {
                                samedosageweeklylist2.SelectedItem = "No";

                                var i = 0;

                                foreach (var itemm in selectedDosages)
                                {
                                    itemm.Labelvis = true;
                                    itemm.Entryvis = false;
                                    itemm.Dosage = SelectedMed.schedule[i].Dosage;
                                    itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                                    itemm.TimepickerVis = true;
                                    itemm.AsReqlblVis = false;
                                    itemm.time = SelectedMed.schedule[i].time;
                                    var timeconverted = TimeSpan.Parse(SelectedMed.schedule[i].time);
                                    itemm.timeconverted = timeconverted;

                                    i++;
                                }


                            }

                        }




                        foreach (var itemm in selectedDosages)
                        {

                            if (itemm.dosageunit.Contains("per"))
                            {
                                itemm.DoubleDosage = true;
                                itemm.NormalDosage = false;
                                var splitdosage = itemm.Dosage.Split('|');
                                itemm.Dosage = splitdosage[0];
                                itemm.Dosage2 = splitdosage[1];

                            }
                            else
                            {
                                itemm.DoubleDosage = false;
                                itemm.NormalDosage = true;

                            }

                        }

                        if (selectedDosages[0].dosageunit.Contains("per"))
                        {
                            samedosageentry2.IsVisible = true;
                        }
                        else
                        {
                            samedosageentry2.IsVisible = false;
                        }

                        // Bind the MedtimesDosages list to the ListView
                        timesanddosageslistview.ItemsSource = selectedDosages;
                        timesanddosageslistview.IsVisible = true;
                        timeanddosagelbl.IsVisible = true;
                        timeanddosagelbl2.IsVisible = true;
                        timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;



                    }
                    else if (SelectedFreq == "Days Interval")
                    {

                        ditimesperdaylist.SelectedItems.Clear();
                        daysintervallistview.SelectedItems.Clear();
                        freqlbl.Text = "How often";
                        daysintervallistview.IsVisible = true;

                        var split = SelectedMed.frequency.Split('|');

                        if (split[1] == "2")
                        {
                            daysintervallistview.SelectedItems.Add("Every other day");
                        }
                        else if (split[1] == "3")
                        {
                            daysintervallistview.SelectedItems.Add("Every 3 days");
                        }
                        else if (split[1] == "7")
                        {
                            daysintervallistview.SelectedItems.Add("Every 7 days");
                        }
                        else if (split[1] == "14")
                        {
                            daysintervallistview.SelectedItems.Add("Every 14 days");
                        }
                        else if (split[1] == "21")
                        {
                            daysintervallistview.SelectedItems.Add("Every 21 days");
                        }
                        else if (split[1] == "28")
                        {
                            daysintervallistview.SelectedItems.Add("Every 28 days");
                        }
                        else if (split[1] == "30")
                        {
                            daysintervallistview.SelectedItems.Add("Every 30 days");
                        }
                        else if (split[1] == "35")
                        {
                            daysintervallistview.SelectedItems.Add("Every 5 weeks");
                        }
                        else if (split[1] == "42")
                        {
                            daysintervallistview.SelectedItems.Add("Every 6 weeks");
                        }
                        else if (split[1] == "49")
                        {
                            daysintervallistview.SelectedItems.Add("Every 7 weeks");
                        }
                        else if (split[1] == "56")
                        {
                            daysintervallistview.SelectedItems.Add("Every 8 weeks");
                        }
                        else if (split[1] == "63")
                        {
                            daysintervallistview.SelectedItems.Add("Every 9 weeks");
                        }
                        else if (split[1] == "70")
                        {
                            daysintervallistview.SelectedItems.Add("Every 10 weeks");
                        }


                        difreqlbl.IsVisible = true;
                        ditimesperdaylist.IsVisible = true;

                        var convertint = Convert.ToInt32(split[2]);

                        var num = convertint - 1;

                        ditimesperdaylist.SelectedItems.Add((weekfreqlist[num]));


                        if (convertint > 1)
                        {
                            disamedosagelbl.IsVisible = true;
                            disamedosagequestionlist.IsVisible = true;

                            if (!string.IsNullOrEmpty(SelectedMed.supplementquestions))
                            {




                                disamedosagequestionlist.SelectedItems.Add(SelectedMed.supplementquestions);

                                if (SelectedMed.supplementquestions == "Yes")
                                {
                                    adddosagelbl.IsVisible = true;
                                    adddosage2lbl.IsVisible = true;

                                    adddosageframe.IsVisible = true;

                                    samedosageentry.Text = SelectedMed.schedule[0].Dosage;
                                    dosageunitlbl.Text = SelectedMed.schedule[0].dosageunit;

                                }
                            }

                        }
                        else
                        {
                            disamedosagequestionlist.IsVisible = false;
                        }



                        weeklynumperday = ditimesperdaylist.SelectedItem.ToString();

                        selectedDosages = frequencyOptionsWDI[weeklynumperday];

                        var iii = 0;

                        foreach (var itemm in selectedDosages)
                        {
                            if (SelectedMed.supplementquestions == "Yes")
                            {
                                itemm.Labelvis = true;
                                itemm.Entryvis = false;
                                itemm.Dosage = SelectedMed.schedule[0].Dosage;
                            }
                            else
                            {
                                itemm.Labelvis = false;
                                itemm.Entryvis = true;
                                itemm.Dosage = SelectedMed.schedule[iii].Dosage;
                            }


                            itemm.dosageunit = SelectedMed.schedule[0].dosageunit;
                            itemm.TimepickerVis = true;
                            itemm.AsReqlblVis = false;
                            itemm.time = SelectedMed.schedule[iii].time;
                            var timeconverted = TimeSpan.Parse(SelectedMed.schedule[iii].time);
                            itemm.timeconverted = timeconverted;

                            iii++;
                        }

                        foreach (var itemm in selectedDosages)
                        {

                            if (itemm.dosageunit.Contains("per"))
                            {
                                var splitdosage = itemm.Dosage.Split('|');
                                itemm.Dosage = splitdosage[0];
                                itemm.Dosage2 = splitdosage[1];
                                itemm.DoubleDosage = true;
                                itemm.NormalDosage = false;

                            }
                            else
                            {
                                itemm.DoubleDosage = false;
                                itemm.NormalDosage = true;

                            }

                        }

                        if (selectedDosages[0].dosageunit.Contains("per"))
                        {
                            samedosageentry2.IsVisible = true;
                        }
                        else
                        {
                            samedosageentry2.IsVisible = false;
                        }

                        timesanddosageslistview.ItemsSource = selectedDosages;

                        timeanddosagelbl.IsVisible = true;
                        timeanddosagelbl2.IsVisible = true;
                        timesanddosageslistview.IsVisible = true;
                    }



                }
                else if (fourthstack.IsVisible == true)
                {
                    topprogress.Progress = 75;
                    fourthstack.IsVisible = false;
                    detailsstack.IsVisible = true;
                    medname4lbl.Text = SelectedMed.supplementtitle;

                    if (SelectedMed.details != "--|--")
                    {
                        var split = SelectedMed.details.Split('|');
                        if (!string.IsNullOrEmpty(split[0]))
                        {
                            displaynameentry.Text = split[0];

                        }
                        if (!string.IsNullOrEmpty(split[1]))
                        {
                            notesentry.Text = split[1];
                        }
                    }
                }
                else if (detailsstack.IsVisible == true)
                {

                    medname5lbl.Text = SelectedMed.supplementtitle;
                    ConfirmBtn.Text = "Update Medication";
                    var newlist = new List<string>();

                    if (string.IsNullOrEmpty(displaynameentry.Text))
                    {
                        newlist.Add("--");
                        confirmdisplaynamelbl.Text = SelectedMed.supplementtitle;
                    }
                    else
                    {
                        newlist.Add(displaynameentry.Text);
                        confirmdisplaynamelbl.Text = displaynameentry.Text;
                    }


                    if (string.IsNullOrEmpty(notesentry.Text))
                    {
                        newlist.Add("--");
                        confirmnoteslbl.Text = "--";
                    }
                    else
                    {
                        newlist.Add(notesentry.Text);
                        confirmnoteslbl.Text = notesentry.Text;
                    }


                    var joinlist = string.Join('|', newlist);

                    SelectedMed.details = joinlist;

                    //add all the details to the confirm page
                    confirmpreplbl.Text = SelectedMed.preparation;

                    if (string.IsNullOrEmpty(SelectedMed.formulation))
                    {
                        confirmformlbl.Text = "--";
                    }
                    else
                    {
                        confirmformlbl.Text = SelectedMed.formulation;
                    }

                    confirmdulbl.Text = SelectedMed.unit;
                    confirmsdlbl.Text = SelectedMed.startdate;

                    if (string.IsNullOrEmpty(SelectedMed.enddate))
                    {
                        confirmedlbl.Text = "--";
                    }
                    else
                    {
                        confirmedlbl.Text = SelectedMed.enddate;
                    }

                    confirmfreqlbl.Text = freqstring;


                    foreach (var item in selectedDosages)
                    {
                        item.Labelvis = true;
                        item.Entryvis = false;

                        if (item.dosageunit.Contains("per"))
                        {
                            if (!string.IsNullOrEmpty(item.Dosage2))
                            {
                                item.Dosage = item.Dosage + "|" + item.Dosage2;
                            }
                        }
                    }

                    confirmtimesanddosageslistview.ItemsSource = selectedDosages;
                    confirmtimesanddosageslistview.HeightRequest = selectedDosages.Count * 110;
                    //  confirmtimesanddosageslistview.IsEnabled = false;

                    detailsstack.IsVisible = false;
                    confirmstack.IsVisible = true;
                    topprogress.Progress = 100;
                    nextbtn.IsVisible = false;
                }


            }
            else
            {
                //Normal Add Medication Click through
                if (firststack.IsVisible == true)
                {
                    //check if they have selected a medication
                    if (Medicationslistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Supplement", "Please select a supplement from this list", "Ok");
                        return;
                    }

                    else
                    {
                        var med = Medicationslistview.SelectedItem as supplement;

                        if (med.AlreadySelected == true)
                        {
                            Vibration.Vibrate();
                            await DisplayAlert("Supplement Already Added", "Please select another supplement from this list", "Ok");
                            return;
                        }

                        medpreparationslistview.ItemsSource = MedPreparations;
                        firststack.IsVisible = false;
                        secondstack.IsVisible = true;
                        backbtn.IsVisible = true;
                        topprogress.Progress = 33.34;
                        backbtn.Text = "Back";
                    }
                }

                else if (secondstack.IsVisible == true)
                {
                    if (medpreparationslistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Supplement Preparation", "Please select a supplement preparation from this list", "Ok");
                        return;
                    }

                    else if (meddosageunitlistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Dosage Unit", "Please select a dosage unit from this list", "Ok");
                        return;
                    }
                    else
                    {
                        secondstack.IsVisible = false;
                        thirdstack.IsVisible = true;
                        topprogress.Progress = 50.01;
                    }

                    if (newusermedication.status == "Pending")
                    {
                        backbtn.Text = "Back";
                    }
                }
                else if (thirdstack.IsVisible == true)
                {

                    if (enddatecheck.IsChecked)
                    {
                        newusermedication.enddate = enddatepicker.Date.ToString("dd/MM/yyyy");
                    }

                    newusermedication.startdate = startdatepicker.Date.ToString("dd/MM/yyyy");


                    thirdstack.IsVisible = false;
                    fourthstack.IsVisible = true;
                    topprogress.Progress = 66.68;
                    medfreqlistview.IsVisible = true;

                }

                else if (fourthstack.IsVisible == true)
                {

                    if (medfreqlistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Supplement Frequency", "Please select a supplement Frequency from this list", "Ok");
                        return;
                    }
                    else
                    {
                        var Selecteditem = medfreqlistview.SelectedItem as preparation;
                        //Daily Validation 
                        if (Selecteditem.title == "Daily")
                        {

                            //How Many times per Day 
                            if (dailytimeslistview.SelectedItem == null)
                            {
                                Vibration.Vibrate();
                                await DisplayAlert("Select how many Times", "Please select how many Times from this list", "Ok");
                                return;
                            }
                            else
                            {
                                //Once Daily Selected
                                if (dailytimeslistview.SelectedItem.ToString() == "OD\nOnce Daily")
                                {
                                    if (TimeOfDaySelected == false)
                                    {
                                        Vibration.Vibrate();
                                        await DisplayAlert("Select Time of Day", "Please select Time of Day from this list", "Ok");
                                        return;
                                    }
                                }
                                //More than Once Daily Selected 
                                else
                                {
                                    if (string.IsNullOrEmpty(DailysameDosageSelected))
                                    {
                                        Vibration.Vibrate();
                                        await DisplayAlert("Select Is Dosage the same", "Please select Is Dosage the same from this list", "Ok");
                                        return;
                                    }
                                    else if (DailysameDosageSelected == "Yes")
                                    {
                                        if (String.IsNullOrEmpty(samedosageentry.Text))
                                        {
                                            Vibration.Vibrate();
                                            await DisplayAlert("Enter a Dosage for all Times", "Please Enter a Dosage for all Times", "Ok");
                                            return;
                                        }
                                    }
                                    else if (DailysameDosageSelected == "No")
                                    {
                                        // Do Nothing 
                                    }
                                }
                            }
                        }

                        //Days of Week Validation 
                        else if (Selecteditem.title == "Specfic Days of the Week")
                        {
                            //Which Days 
                            if (weeklydayslist.SelectedItem == null)
                            {
                                Vibration.Vibrate();
                                await DisplayAlert("Select Which Days", "Please Select Which Days for your Medication", "Ok");
                                return;
                            }
                            //How many times
                            if (weekfreqlistview.SelectedItem == null)
                            {
                                Vibration.Vibrate();
                                await DisplayAlert("Select How many Times", "Please Select How many Times for your Medication", "Ok");
                                return;
                            }
                            else
                            {
                                //Once Weekly Selected
                                if (weekfreqlistview.SelectedItem.ToString() == "One")
                                {
                                    //do nothing
                                }
                                //More than Once Weekly Selected 
                                else
                                {
                                    if (samedosageweeklylist2.IsVisible)
                                    {
                                        if (samedosageweeklylist2.SelectedItem == null)
                                        {
                                            Vibration.Vibrate();
                                            await DisplayAlert("Select Is Dosage the same", "Please select Is Dosage the same from this list", "Ok");
                                            return;
                                        }
                                        else if (samedosageweeklylist2.SelectedItem.ToString() == "Yes")
                                        {
                                            if (String.IsNullOrEmpty(samedosageentry.Text))
                                            {
                                                Vibration.Vibrate();
                                                await DisplayAlert("Enter a Dosage for all Times", "Please Enter a Dosage for all Times", "Ok");
                                                return;
                                            }
                                        }
                                        else if (samedosageweeklylist2.SelectedItem.ToString() == "No")
                                        {
                                            // Do Nothing 
                                        }
                                    }
                                }
                            }
                        }

                        //Days Interval Validation 
                        else if (Selecteditem.title == "Days Interval")
                        {
                            //How Often 
                            if (daysintervallistview.SelectedItem == null)
                            {
                                Vibration.Vibrate();
                                await DisplayAlert("Select How Often", "Please Select How often you take your Medication", "Ok");
                                return;
                            }

                            //How many times per day 
                            if (ditimesperdaylist.SelectedItem == null)
                            {
                                Vibration.Vibrate();
                                await DisplayAlert("Select How many Times", "Please Select How many Times for your Medication", "Ok");
                                return;
                            }
                            else
                            {
                                //Once Weekly Selected
                                if (ditimesperdaylist.SelectedItem.ToString() == "One")
                                {
                                    //do nothing
                                }
                                //More than Once Weekly Selected 
                                else
                                {
                                    if (disamedosagequestionlist.SelectedItem == null)
                                    {
                                        Vibration.Vibrate();
                                        await DisplayAlert("Select Is Dosage the same", "Please select Is Dosage the same from this list", "Ok");
                                        return;
                                    }
                                    else if (disamedosagequestionlist.SelectedItem.ToString() == "Yes")
                                    {
                                        if (String.IsNullOrEmpty(samedosageentry.Text))
                                        {
                                            Vibration.Vibrate();
                                            await DisplayAlert("Enter a Dosage for all Times", "Please Enter a Dosage for all Times", "Ok");
                                            return;
                                        }
                                    }
                                    else if (disamedosagequestionlist.SelectedItem.ToString() == "No")
                                    {
                                        // Do Nothing 
                                    }
                                }
                            }

                        }
                        //As Required Validation 
                        else if (Selecteditem.title == "As Required")
                        {
                            // if (string.IsNullOrEmpty(newusermedication.Dosage))
                            // {
                            // Vibration.Vibrate();
                            // await DisplayAlert("Enter Dosage Unit", "Please Enter a dosage unit from this list", "Ok");
                            //   return;
                            // }
                        }
                    }

                    foreach (var item in selectedDosages)
                    {
                        if (string.IsNullOrEmpty(item.Dosage))
                        {
                            Vibration.Vibrate();
                            await DisplayAlert("Enter Dosage Unit", "Please Enter a dosage unit from this list", "Ok");
                            return;
                        }
                    }
                    topprogress.Progress = 83.35;
                    fourthstack.IsVisible = false;
                    detailsstack.IsVisible = true;

                }
                else if (detailsstack.IsVisible == true)
                {
                    var newlist = new List<string>();

                    if (string.IsNullOrEmpty(displaynameentry.Text))
                    {
                        newlist.Add("--");
                        confirmdisplaynamelbl.Text = newusermedication.supplementtitle;
                    }
                    else
                    {
                        newlist.Add(displaynameentry.Text);
                        confirmdisplaynamelbl.Text = displaynameentry.Text;
                    }


                    if (string.IsNullOrEmpty(notesentry.Text))
                    {
                        newlist.Add("--");
                        confirmnoteslbl.Text = "--";
                    }
                    else
                    {
                        newlist.Add(notesentry.Text);
                        confirmnoteslbl.Text = notesentry.Text;
                    }


                    var joinlist = string.Join('|', newlist);

                    newusermedication.details = joinlist;

                    //add all the details to the confirm page
                    confirmpreplbl.Text = newusermedication.preparation;

                    if (string.IsNullOrEmpty(newusermedication.formulation))
                    {
                        confirmformlbl.Text = "--";
                    }
                    else
                    {
                        confirmformlbl.Text = newusermedication.formulation;
                    }

                    confirmdulbl.Text = newusermedication.unit;
                    confirmsdlbl.Text = newusermedication.startdate;

                    if (string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        confirmedlbl.Text = "--";
                    }
                    else
                    {
                        confirmedlbl.Text = newusermedication.enddate;
                    }

                    confirmfreqlbl.Text = freqstring;

                    foreach (var item in selectedDosages)
                    {
                        item.Labelvis = true;
                        item.Entryvis = false;


                        if (item.dosageunit.Contains("per"))
                        {
                            if (!string.IsNullOrEmpty(item.Dosage2))
                            {
                                item.Dosage = item.Dosage + "|" + item.Dosage2;
                            }
                        }
                    }


                    confirmtimesanddosageslistview.ItemsSource = selectedDosages;
                    confirmtimesanddosageslistview.HeightRequest = selectedDosages.Count * 110;
                    //  confirmtimesanddosageslistview.IsEnabled = false;

                    detailsstack.IsVisible = false;
                    confirmstack.IsVisible = true;
                    topprogress.Progress = 100;
                    nextbtn.IsVisible = false;

                }


            }
        }
        catch (Exception ex)
        {

        }
    }

    public string NumberToText(int number)
    {
        switch (number)
        {
            case 1: return "One";
            case 2: return "Two";
            case 3: return "Three";
            case 4: return "Four";
            case 5: return "Five";
            case 6: return "Six";
            case 7: return "Seven";
            case 8: return "Eight";
            default: return "Invalid number";
        }
    }

    private void medpreparationslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

        try
        {
            var item = e.DataItem as preparation;


            medformulations = item.formulation.Split(',').ToList();
            meddosages = item.unit.Split(',').ToList();

            medformulationslistview.ItemsSource = medformulations;
            mflbl.IsVisible = true;
            mf2lbl.IsVisible = true;
            medformulationslistview.IsVisible = true;

            meddosageunitlistview.ItemsSource = meddosages;
            dflbl.IsVisible = true;
            df2lbl.IsVisible = true;
            meddosageunitlistview.IsVisible = true;

            newusermedication.preparation = item.title;

            if (IsEdit)
            {
                SelectedMed.preparation = item.title;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            if (!e.Value)
            {
                //ischecked
                enddategrid.Opacity = 0.2;
                enddatepicker.IsEnabled = false;
                newusermedication.enddate = null;
            }
            else
            {
                //ischecked
                enddategrid.Opacity = 1;
                enddatepicker.IsEnabled = true;
                enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void medfreqlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as preparation;

            freqstring = item.title;
            newusermedication.frequency = item.title;
            btnyes.BackgroundColor = Colors.White;
            btnyes.TextColor = Colors.Teal;

            btnno.BackgroundColor = Colors.White;
            btnno.TextColor = Colors.Teal;

            selectedDosages.Clear();
            timeanddosagelbl.IsVisible = false;
            timeanddosagelbl2.IsVisible = false;
            timesanddosageslistview.IsVisible = false;
            sameweeklydosage2.IsVisible = false;
            samedosageweeklylist2.IsVisible = false;
            samedosageweeklylist2.SelectedItem = null;
            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;
            sameweeklydosage.IsVisible = false;
            samedosageweeklylist.IsVisible = false;
            samedosageweeklylist.SelectedItem = null;

            difreqlbl.IsVisible = false;
            ditimesperdaylist.IsVisible = false;


            ditimesperdaylist.SelectedItem = null;

            disamedosagelbl.IsVisible = false;
            disamedosagequestionlist.IsVisible = false;
            disamedosagequestionlist.SelectedItem = null;

            timeanddosagelbl.Text = "Times and Dosages";
            timeanddosagelbl2.Text = "Select your supplement times and dosages from the list below. To adjust the time, simply tap on it.";


            if (item.title == "Daily")
            {
                freqlbl.Text = "How many times per day";
                dailytimeslistview.IsVisible = true;

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    dailytimeslistview.HeightRequest = 200;
                }
                weeklydayslist.IsVisible = false;
                daysintervallistview.IsVisible = false;


                weekfreqlbl.IsVisible = false;
                weekfreqlistview.IsVisible = false;
                samedosageweeklylist.IsVisible = false;


                daysintervallistview.SelectedItems.Clear();
                weeklydayslist.SelectedItems.Clear();
                weekfreqlistview.SelectedItems.Clear();
            }
            else if (item.title == "Specfic Days of the Week")
            {
                freqlbl.Text = "Which days";
                weeklydayslist.IsVisible = true;
                // weekfreqlbl.IsVisible = true;
                // weekfreqlistview.IsVisible = true;
                dailytimeslistview.IsVisible = false;
                daysintervallistview.IsVisible = false;

                onedailylbl.IsVisible = false;
                oncedailylistview.IsVisible = false;

                samedosageweeklylist.IsVisible = false;
                sameweeklydosage.IsVisible = false;

                dailytimeslistview.SelectedItems.Clear();
                oncedailylistview.SelectedItems.Clear();
                daysintervallistview.SelectedItems.Clear();

            }
            else if (item.title == "Days Interval")
            {
                freqlbl.Text = "How often";
                dailytimeslistview.IsVisible = false;
                weeklydayslist.IsVisible = false;
                daysintervallistview.IsVisible = true;

                onedailylbl.IsVisible = false;
                oncedailylistview.IsVisible = false;
                weekfreqlbl.IsVisible = false;
                weekfreqlistview.IsVisible = false;
                samedosageweeklylist.IsVisible = false;
                sameweeklydosage.IsVisible = false;

                dailytimeslistview.SelectedItems.Clear();
                oncedailylistview.SelectedItems.Clear();

                weeklydayslist.SelectedItems.Clear();
                weekfreqlistview.SelectedItems.Clear();
            }
            else
            {
                //  timeanddosagelbl.Text = "Add Dosage";
                //  timeanddosagelbl2.Text = "Select your dosage from the item below";
                freqlbl.Text = string.Empty;
                dailytimeslistview.IsVisible = false;
                weeklydayslist.IsVisible = false;
                daysintervallistview.IsVisible = false;

                onedailylbl.IsVisible = false;
                oncedailylistview.IsVisible = false;
                weekfreqlbl.IsVisible = false;
                weekfreqlistview.IsVisible = false;
                samedosageweeklylist.IsVisible = false;
                sameweeklydosage.IsVisible = false;

                //   var medDosagesList = new ObservableCollection<MedtimesDosages>();

                // Create a new MedtimesDosages object
                //  var newMd = new MedtimesDosages()
                //  {
                //      Dosage = newusermedication.preparation,
                //      dosageunit = newusermedication.unit,
                //  };

                // Add the new object to the collection
                //  medDosagesList.Add(newMd);

                // Iterate over the collection (if it has multiple items) or directly manipulate `newMd`
                //   foreach (var itemm in medDosagesList)
                //   {
                //      itemm.Labelvis = false;
                //      itemm.Entryvis = true;
                //      itemm.Dosage = string.Empty;
                //      itemm.TimepickerVis = false;
                //      itemm.AsReqlblVis = true;
                //  }

                // Bind the MedtimesDosages list to the ListView
                //  timesanddosageslistview.ItemsSource = medDosagesList;
                //  timesanddosageslistview.IsVisible = true;
                //  timeanddosagelbl.IsVisible = true;
                //  timeanddosagelbl2.IsVisible = true;
                //  timesanddosageslistview.HeightRequest = medDosagesList.Count * 100;

                dailytimeslistview.SelectedItems.Clear();
                oncedailylistview.SelectedItems.Clear();
                daysintervallistview.SelectedItems.Clear();
                weeklydayslist.SelectedItems.Clear();
                weekfreqlistview.SelectedItems.Clear();
            }



        }
        catch (Exception ex)
        {

        }
    }

    private void dailytimeslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            samedosageweeklylist2.SelectedItem = null;
            samedosageweeklylist2.IsVisible = false;
            sameweeklydosage.IsVisible = false;
            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;
            timeanddosagelbl.IsVisible = false;
            timeanddosagelbl2.IsVisible = false;
            timesanddosageslistview.IsVisible = false;
            sameweeklydosage2.IsVisible = false;

            // selectedDosages.Clear();

            if (item.Contains("Once"))
            {
                onedailylbl.IsVisible = true;
                oncedailylistview.IsVisible = true;
            }
            else
            {
                onedailylbl.IsVisible = false;
                oncedailylistview.IsVisible = false;

                sameweeklydosage2.IsVisible = true;
                samedosageweeklylist2.IsVisible = true;

                var splititem = item.Split('\n');
                // Check if the selected option exists in the dictionary
                if (frequencyOptions.ContainsKey(splititem[1]))
                {
                    // Get the corresponding MedtimesDosages list for the selected option
                    selectedDosages = frequencyOptions[splititem[1]];

                    //selectedDosages[0].Alldosage = false;

                    foreach (var itemm in selectedDosages)
                    {
                        itemm.Dosage = string.Empty;
                        itemm.Dosage2 = string.Empty;
                        itemm.TimepickerVis = true;
                        itemm.AsReqlblVis = false;

                        if (itemm.dosageunit.Contains("per"))
                        {
                            itemm.DoubleDosage = true;
                            itemm.NormalDosage = false;

                        }
                        else
                        {
                            itemm.DoubleDosage = false;
                            itemm.NormalDosage = true;

                        }

                    }


                    if (selectedDosages[0].dosageunit.Contains("per"))
                    {
                        samedosageentry2.IsVisible = true;
                    }
                    else
                    {
                        samedosageentry2.IsVisible = false;
                    }

                    // Bind the MedtimesDosages list to the ListView
                    timesanddosageslistview.ItemsSource = selectedDosages;
                    // timesanddosageslistview.IsVisible = true;
                    // timeanddosagelbl.IsVisible = true;
                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void weeklydayslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;
            WeeklyDaysSelected = item;
            timeanddosagelbl.IsVisible = false;
            timeanddosagelbl2.IsVisible = false;
            timesanddosageslistview.IsVisible = false;

            sameweeklydosage.IsVisible = false;
            samedosageweeklylist.IsVisible = false;
            samedosageweeklylist.SelectedItem = null;

            sameweeklydosage2.IsVisible = false;
            samedosageweeklylist2.IsVisible = false;
            samedosageweeklylist2.SelectedItem = null;

            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;
            samedosageentry.Text = string.Empty;
            samedosageentry2.Text = string.Empty;


            if (weekdaynamelist.Contains(item))
            {
                weekdaycountlist.Remove(item);
                weekdaynamelist.Remove(item);
            }
            else
            {
                weekdaycountlist.Add(item);
                weekdaynamelist.Add(item);
            }

            // Check if SelectedItems is not null and contains any items
            if (weekdaynamelist.Count > 0)
            {
                // If there are selected items, show the frequency label and list view
                weekfreqlbl.IsVisible = true;
                weekfreqlistview.IsVisible = true;

                weekfreqlistview.SelectedItem = null;

                samedosageweeklylist.IsVisible = false;
                sameweeklydosage.IsVisible = false;

                btnyes.BackgroundColor = Colors.White;
                btnyes.TextColor = Colors.Teal;

                btnno.BackgroundColor = Colors.White;
                btnno.TextColor = Colors.Teal;


            }
            else
            {
                // If no items are selected, hide the frequency label and list view
                weekfreqlbl.IsVisible = false;
                weekfreqlistview.IsVisible = false;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void meddosageunitlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;


            newusermedication.unit = item;

            if (IsEdit)
            {
                SelectedMed.unit = item;
            }

            dosageunitlbl.Text = item;


            foreach (var freq in frequencyOptions)
            {
                foreach (var medtime in freq.Value)
                {
                    medtime.dosageunit = item; // Update the dosageunit
                }
            }

            foreach (var freq in frequencyOptionsWDI)
            {
                foreach (var medtime in freq.Value)
                {
                    medtime.dosageunit = item; // Update the dosageunit
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            // Get the Entry that triggered the event
            var entry = (Entry)sender;


            if (newusermedication.frequency == "As Required")
            {
                newusermedication.Dosage = e.NewTextValue;
            }
            else
            {
                // Check if the BindingContext of the Entry is the first item in the list
                if (entry.BindingContext == selectedDosages[0])
                {
                    if (!string.IsNullOrEmpty(e.NewTextValue))
                    {
                        // Check if the list has more than one entry
                        if (selectedDosages.Count > 1)
                        {
                            //  selectedDosages[0].Alldosage = true;
                        }
                    }
                    else
                    {
                        //selectedDosages[0].Alldosage = false;
                    }
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (e.Value)
            {
                var firstdosage = selectedDosages[0].Dosage;

                foreach (var item in selectedDosages)
                {
                    item.Dosage = firstdosage;
                }

                //selectedDosages[0].Alldosage = false;
                selectedDosages[0].Checkboxchecked = false;

                timesanddosageslistview.HeightRequest = timesanddosageslistview.Height + 100;


            }

        }
        catch (Exception ex)
        {

        }
    }

    private void weekfreqlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //weekly how many times per day

            var item = e.DataItem as string;
            WeeklyHowManyTimes = item;
            samedosageweeklylist.SelectedItem = null;
            samedosageweeklylist2.IsVisible = false;
            samedosageweeklylist2.SelectedItem = null;
            sameweeklydosage2.IsVisible = false;
            sameweeklydosage.IsVisible = false;
            samedosageweeklylist.IsVisible = false;
            samedosageweeklylist.SelectedItem = null;
            timesanddosageslistview.IsVisible = false;
            timeanddosagelbl.IsVisible = false;
            timeanddosagelbl2.IsVisible = false;
            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;
            samedosageentry.Text = string.Empty;
            samedosageentry2.Text = string.Empty;


            //check if there is more than one day, if not dont show list
            if (weeklydayslist.SelectedItems.Count > 1)
            {
                samedosageweeklylist.IsVisible = true;
                sameweeklydosage.IsVisible = true;
            }
            else
            {
                samedosageweeklylist.IsVisible = false;
                samedosageweeklylist.SelectedItem = null;
                sameweeklydosage.IsVisible = false;
                samedosageweeklylist2.IsVisible = false;
                samedosageweeklylist2.SelectedItem = null;
                sameweeklydosage2.IsVisible = false;
            }

            weeklynumperday = item;


            if (frequencyOptionsWDI.ContainsKey(weeklynumperday))
            {
                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                //selectedDosages[0].Alldosage = false;

                var newcollection = new List<MedtimesDosages>();

                //add the days 
                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                foreach (var md in selectedDosages)
                {
                    for (int i = 0; i < listofdays.Count; i++)
                    {
                        var d = listofdays[i];

                        // Create a new instance of MedtimesDosages (assuming md is of this type) and copy the relevant properties
                        var newMd = new MedtimesDosages
                        {
                            Dosage = md.Dosage,
                            dosageunit = md.dosageunit,
                            time = md.time,
                            timeconverted = md.timeconverted,
                            Day = d // Assign the current day
                        };

                        newcollection.Add(newMd); // Add the new instance to the collection
                    }
                }


                // Sort the list based on the day order defined earlier
                var sortedList = newcollection.OrderBy(md => dayOrder[md.Day]).ToList();

                selectedDosages = sortedList;

                foreach (var itemm in selectedDosages)
                {
                    if (weekdaynamelist.Count == 1)
                    {
                        itemm.Labelvis = false;
                        itemm.Entryvis = true;
                        itemm.TimepickerVis = true;
                        itemm.AsReqlblVis = false;
                    }
                    itemm.Dosage = string.Empty;
                    itemm.Dosage2 = string.Empty;

                    if (itemm.dosageunit.Contains("per"))
                    {
                        itemm.DoubleDosage = true;
                        itemm.NormalDosage = false;

                    }
                    else
                    {
                        itemm.DoubleDosage = false;
                        itemm.NormalDosage = true;

                    }
                }

                if (weeklynumperday == "One" && weeklydayslist.SelectedItems.Count == 1)
                {
                    samedosageweeklylist.IsVisible = false;
                    sameweeklydosage.IsVisible = false;
                    samedosageweeklylist.SelectedItem = null;
                    samedosageweeklylist2.IsVisible = false;
                    samedosageweeklylist2.SelectedItem = null;
                    sameweeklydosage2.IsVisible = false;
                    timesanddosageslistview.ItemsSource = selectedDosages;
                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
                }
                else if (weeklynumperday == "One" && weeklydayslist.SelectedItems.Count > 1)
                {
                    samedosageweeklylist.IsVisible = true;
                    sameweeklydosage.IsVisible = true;
                    samedosageweeklylist2.IsVisible = false;
                    sameweeklydosage2.IsVisible = false;
                    timesanddosageslistview.IsVisible = false;
                    timeanddosagelbl.IsVisible = false;
                    timeanddosagelbl2.IsVisible = false;
                }
                else if (weeklydayslist.SelectedItems.Count > 1)
                {

                }
                else
                {
                    samedosageweeklylist2.IsVisible = true;
                    sameweeklydosage2.IsVisible = true;
                    timesanddosageslistview.IsVisible = false;
                    timeanddosagelbl.IsVisible = false;
                    timeanddosagelbl2.IsVisible = false;
                }





            }



            // weeklynumperday = item;

            //  timesanddosageslistview.IsVisible = false;
            //  timeanddosagelbl.IsVisible = false;
            // timeanddosagelbl2.IsVisible = false;


        }
        catch (Exception ex)
        {

        }
    }

    private void btnyes_Clicked(object sender, EventArgs e)
    {

    }

    private void btnno_Clicked(object sender, EventArgs e)
    {

    }

    private void samedosageweeklylist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            if (item == "Yes")
            {

                if (weeklynumperday == "One")
                {
                    //dont show second list as only one 
                    //show the label and hide the entry

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

                    foreach (var itemm in selectedDosages)
                    {

                        if (itemm.dosageunit.Contains("per"))
                        {
                            itemm.DoubleDosage = true;
                            itemm.NormalDosage = false;

                        }
                        else
                        {
                            itemm.DoubleDosage = false;
                            itemm.NormalDosage = true;

                        }

                    }

                    if (selectedDosages[0].dosageunit.Contains("per"))
                    {
                        samedosageentry2.IsVisible = true;
                    }
                    else
                    {
                        samedosageentry2.IsVisible = false;
                    }

                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
                    timesanddosageslistview.ItemsSource = selectedDosages;
                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                }
                else
                {

                    sameweeklydosage2.IsVisible = true;
                    samedosageweeklylist2.IsVisible = true;

                    samedosageweeklylist2.SelectedItem = null;

                    //group the listview so all days are the same
                    var groupedCollection = selectedDosages
    .GroupBy(md => new { md.time, md.Dosage, md.dosageunit, md.timeconverted })  // Group by time and other properties
    .Select(g => new MedtimesDosages
    {
        Dosage = g.Key.Dosage,
        dosageunit = g.Key.dosageunit,
        time = g.Key.time,                      // Keep the grouped time
        timeconverted = g.Key.timeconverted,    // Keep the grouped time converted
        Day = string.Join(", ", g.Select(md => md.Day))  // Join the days into a single string
    })
    .ToList();

                    selectedDosages = groupedCollection;

                    foreach (var itemm in selectedDosages)
                    {

                        if (itemm.dosageunit.Contains("per"))
                        {
                            itemm.DoubleDosage = true;
                            itemm.NormalDosage = false;

                        }
                        else
                        {
                            itemm.DoubleDosage = false;
                            itemm.NormalDosage = true;

                        }

                    }

                    if (selectedDosages[0].dosageunit.Contains("per"))
                    {
                        samedosageentry2.IsVisible = true;
                    }
                    else
                    {
                        samedosageentry2.IsVisible = false;
                    }

                    timesanddosageslistview.ItemsSource = selectedDosages;
                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;

                    timesanddosageslistview.IsVisible = false;
                    timeanddosagelbl.IsVisible = false;
                    timeanddosagelbl2.IsVisible = false;

                }
            }
            else
            {

                if (weeklynumperday == "One")
                {
                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = true;
                        sd.Labelvis = false;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }

                    adddosagelbl.IsVisible = false;
                    adddosage2lbl.IsVisible = false;
                    adddosageframe.IsVisible = false;
                }

                sameweeklydosage2.IsVisible = false;
                samedosageweeklylist2.IsVisible = false;


                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                //selectedDosages[0].Alldosage = false;

                var newcollection = new List<MedtimesDosages>();

                //add the days 
                var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

                foreach (var md in selectedDosages)
                {
                    for (int i = 0; i < listofdays.Count; i++)
                    {
                        var d = listofdays[i];

                        // Create a new instance of MedtimesDosages (assuming md is of this type) and copy the relevant properties
                        var newMd = new MedtimesDosages
                        {
                            Dosage = md.Dosage,
                            dosageunit = md.dosageunit,
                            time = md.time,
                            timeconverted = md.timeconverted,
                            Day = d // Assign the current day
                        };

                        newcollection.Add(newMd); // Add the new instance to the collection
                    }
                }


                // Sort the list based on the day order defined earlier
                var sortedList = newcollection.OrderBy(md => dayOrder[md.Day]).ToList();

                selectedDosages = sortedList;

                foreach (var itemm in selectedDosages)
                {


                    itemm.Labelvis = false;
                    itemm.Entryvis = true;
                    itemm.Dosage = string.Empty;
                    itemm.Dosage2 = string.Empty;
                    itemm.TimepickerVis = true;
                    itemm.AsReqlblVis = false;
                }

                foreach (var itemm in selectedDosages)
                {

                    if (itemm.dosageunit.Contains("per"))
                    {
                        itemm.DoubleDosage = true;
                        itemm.NormalDosage = false;

                    }
                    else
                    {
                        itemm.DoubleDosage = false;
                        itemm.NormalDosage = true;

                    }

                }

                if (selectedDosages[0].dosageunit.Contains("per"))
                {
                    samedosageentry2.IsVisible = true;
                }
                else
                {
                    samedosageentry2.IsVisible = false;
                }

                timesanddosageslistview.ItemsSource = selectedDosages;
                timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;

                adddosagelbl.IsVisible = false;
                adddosage2lbl.IsVisible = false;
                adddosageframe.IsVisible = false;
                samedosageentry.Text = string.Empty;
                samedosageentry2.Text = string.Empty;

                timesanddosageslistview.IsVisible = true;
                timeanddosagelbl.IsVisible = true;
                timeanddosagelbl2.IsVisible = true;

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void Entry_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {

            }
            else
            {
                foreach (var item in selectedDosages)
                {
                    item.Dosage = e.NewTextValue;
                }
            }


            //if (!string.IsNullOrEmpty(e.NewTextValue))
            //{
            //    // Get the current text without the unit
            //    string currentTextWithoutUnit = samedosageentry.Text.Replace($" {newusermedication.unit}", "");

            //    // Set the new text with the unit appended only once
            //    samedosageentry.Text = e.NewTextValue + " " + newusermedication.unit;
            //}


        }
        catch (Exception ex)
        {

        }
    }

    private void samedosageweeklylist2_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;
            DailysameDosageSelected = item;

            samedosageentry.Text = string.Empty;
            samedosageentry2.Text = string.Empty;

            if (IsEdit)
            {
                dosageunitlbl.Text = SelectedMed.schedule[0].dosageunit;
            }

            if (item == "Yes")
            {

                //check was freq it is

                if (freqstring == "Daily")
                {
                    //show the label and hide the entry

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;

                }
                else if (freqstring == "Specfic Days of the Week")
                {
                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

                    foreach (var itemm in selectedDosages)
                    {

                        if (itemm.dosageunit.Contains("per"))
                        {
                            itemm.DoubleDosage = true;
                            itemm.NormalDosage = false;

                        }
                        else
                        {
                            itemm.DoubleDosage = false;
                            itemm.NormalDosage = true;

                        }

                    }

                    if (selectedDosages[0].dosageunit.Contains("per"))
                    {
                        samedosageentry2.IsVisible = true;
                    }
                    else
                    {
                        samedosageentry2.IsVisible = false;
                    }

                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
                    timesanddosageslistview.ItemsSource = selectedDosages;
                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                }


            }
            else
            {
                if (freqstring == "Daily")
                {
                    adddosagelbl.IsVisible = false;
                    adddosage2lbl.IsVisible = false;
                    adddosageframe.IsVisible = false;

                    //show the entry and hide the label

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = true;
                        sd.Labelvis = false;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }


                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                }
                else if (freqstring == "Specfic Days of the Week")
                {
                    adddosagelbl.IsVisible = false;
                    adddosage2lbl.IsVisible = false;
                    adddosageframe.IsVisible = false;

                    //show the entry and hide the label

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Dosage2 = string.Empty;
                        sd.Entryvis = true;
                        sd.Labelvis = false;
                        sd.TimepickerVis = true;
                        sd.AsReqlblVis = false;
                    }


                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                }
            }




        }
        catch (Exception ex)
        {

        }
    }

    private void samedosageentry_Completed(object sender, EventArgs e)
    {
        try
        {
            samedosageentry.IsEnabled = false;
            samedosageentry.IsEnabled = true;
        }
        catch (Exception ex)
        {

        }
    }

    private void oncedailylistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {


            var item = e.DataItem as string;

            TimeOfDaySelected = true;
            var splititem = item.Split('\n');
            // Check if the selected option exists in the dictionary
            if (frequencyOptions.ContainsKey(splititem[1]))
            {
                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptions[splititem[1]];

                // selectedDosages[0].Alldosage = false;

                foreach (var itemm in selectedDosages)
                {
                    itemm.Labelvis = false;
                    itemm.Entryvis = true;
                    itemm.Dosage = string.Empty;
                    itemm.Dosage2 = string.Empty;
                    itemm.TimepickerVis = true;
                    itemm.AsReqlblVis = false;
                }

                foreach (var itemm in selectedDosages)
                {

                    if (itemm.dosageunit.Contains("per"))
                    {
                        itemm.DoubleDosage = true;
                        itemm.NormalDosage = false;

                    }
                    else
                    {
                        itemm.DoubleDosage = false;
                        itemm.NormalDosage = true;

                    }

                }

                if (selectedDosages[0].dosageunit.Contains("per"))
                {
                    samedosageentry2.IsVisible = true;
                }
                else
                {
                    samedosageentry2.IsVisible = false;
                }

                // Bind the MedtimesDosages list to the ListView
                timesanddosageslistview.ItemsSource = selectedDosages;
                timesanddosageslistview.IsVisible = true;
                timeanddosagelbl.IsVisible = true;
                timeanddosagelbl2.IsVisible = true;
                timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void daysintervallistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            difreqlbl.IsVisible = true;
            ditimesperdaylist.IsVisible = true;

            ditimesperdaylist.SelectedItem = null;

            disamedosagelbl.IsVisible = false;
            disamedosagequestionlist.IsVisible = false;
            disamedosagequestionlist.SelectedItem = null;

            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;

            timesanddosageslistview.IsVisible = false;
            timeanddosagelbl.IsVisible = false;
            timeanddosagelbl2.IsVisible = false;

        }
        catch (Exception ex)
        {

        }
    }

    private void ditimesperdaylist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            disamedosagequestionlist.SelectedItem = null;
            adddosagelbl.IsVisible = false;
            adddosage2lbl.IsVisible = false;
            adddosageframe.IsVisible = false;


            if (frequencyOptionsWDI.ContainsKey(item))
            {
                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptionsWDI[item];

                // selectedDosages[0].Alldosage = false;

                foreach (var itemm in selectedDosages)
                {
                    itemm.Dosage = string.Empty;
                    itemm.Dosage2 = string.Empty;
                }

                foreach (var itemm in selectedDosages)
                {

                    if (itemm.dosageunit.Contains("per"))
                    {
                        itemm.DoubleDosage = true;
                        itemm.NormalDosage = false;

                    }
                    else
                    {
                        itemm.DoubleDosage = false;
                        itemm.NormalDosage = true;

                    }

                }

                if (selectedDosages[0].dosageunit.Contains("per"))
                {
                    samedosageentry2.IsVisible = true;
                }
                else
                {
                    samedosageentry2.IsVisible = false;
                }

                // Bind the MedtimesDosages list to the ListView
                timesanddosageslistview.ItemsSource = selectedDosages;
                //   timesanddosageslistview.IsVisible = true;
                //   timeanddosagelbl.IsVisible = true;
                //   timeanddosagelbl2.IsVisible = true;
                timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;
            }

            if (item == "One")
            {
                disamedosagelbl.IsVisible = false;
                disamedosagequestionlist.IsVisible = false;

                foreach (var itemm in selectedDosages)
                {
                    itemm.Entryvis = true;
                    itemm.Labelvis = false;
                    itemm.TimepickerVis = true;
                    itemm.AsReqlblVis = false;
                }

                timesanddosageslistview.IsVisible = true;
                timeanddosagelbl.IsVisible = true;
                timeanddosagelbl2.IsVisible = true;
            }
            else
            {
                disamedosagelbl.IsVisible = true;
                disamedosagequestionlist.IsVisible = true;

                timesanddosageslistview.IsVisible = false;
                timeanddosagelbl.IsVisible = false;
                timeanddosagelbl2.IsVisible = false;
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void disamedosagequestionlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            samedosageentry.Text = string.Empty;
            samedosageentry2.Text = string.Empty;


            if (item == "Yes")
            {
                adddosagelbl.IsVisible = true;
                adddosage2lbl.IsVisible = true;
                adddosageframe.IsVisible = true;

                foreach (var sd in selectedDosages)
                {
                    sd.Dosage = string.Empty;
                    sd.Dosage2 = string.Empty;
                    sd.Entryvis = false;
                    sd.Labelvis = true;
                    sd.TimepickerVis = true;
                    sd.AsReqlblVis = false;
                }
            }
            else
            {
                adddosagelbl.IsVisible = false;
                adddosage2lbl.IsVisible = false;
                adddosageframe.IsVisible = false;

                foreach (var sd in selectedDosages)
                {
                    sd.Dosage = string.Empty;
                    sd.Dosage2 = string.Empty;
                    sd.Entryvis = true;
                    sd.Labelvis = false;
                    sd.TimepickerVis = true;
                    sd.AsReqlblVis = false;
                }
            }

            timesanddosageslistview.IsVisible = true;
            timeanddosagelbl.IsVisible = true;
            timeanddosagelbl2.IsVisible = true;



        }
        catch (Exception ex)
        {

        }
    }

    private void medformulationslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;


            newusermedication.formulation = item;

            if (IsEdit)
            {
                SelectedMed.formulation = item;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void startdatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);
        }
        catch (Exception ex)
        {

        }
    }

    private void backbtn_Clicked(object sender, EventArgs e)
    {
        //back button clicked
        try
        {
            if (IsEdit)
            {
                if (backbtn.Text == "Cancel")
                {
                    Navigation.RemovePage(this);
                }
                else if (confirmstack.IsVisible == true)
                {
                    confirmstack.IsVisible = false;
                    detailsstack.IsVisible = true;
                    backbtn.Text = "Back";
                    nextbtn.IsVisible = true;
                    topprogress.Progress = 75;
                }
                else if (detailsstack.IsVisible == true)
                {
                    detailsstack.IsVisible = false;
                    fourthstack.IsVisible = true;
                    topprogress.Progress = 50;
                }
                else if (fourthstack.IsVisible == true)
                {
                    fourthstack.IsVisible = false;
                    thirdstack.IsVisible = true;
                    topprogress.Progress = 25;
                    // backbtn.Text = "Cancel";
                }
                else if (thirdstack.IsVisible == true)
                {

                    thirdstack.IsVisible = false;
                    secondstack.IsVisible = true;
                    topprogress.Progress = 0;
                    backbtn.Text = "Cancel";
                }
            }
            else
            {
                if (backbtn.Text == "Cancel")
                {
                    Navigation.RemovePage(this);
                }
                else if (confirmstack.IsVisible == true)
                {
                    confirmstack.IsVisible = false;
                    detailsstack.IsVisible = true;
                    backbtn.Text = "Back";
                    nextbtn.IsVisible = true;
                    topprogress.Progress = 83.35;
                }
                else if (detailsstack.IsVisible == true)
                {
                    detailsstack.IsVisible = false;
                    fourthstack.IsVisible = true;
                    topprogress.Progress = 66.68;
                }
                else if (fourthstack.IsVisible == true)
                {
                    fourthstack.IsVisible = false;
                    thirdstack.IsVisible = true;
                    topprogress.Progress = 50.01;
                }
                else if (thirdstack.IsVisible == true)
                {
                    thirdstack.IsVisible = false;
                    secondstack.IsVisible = true;
                    topprogress.Progress = 33.34;

                    if (newusermedication.status == "Pending")
                    {
                        backbtn.Text = "Cancel";
                    }
                }
                else if (secondstack.IsVisible == true)
                {
                    secondstack.IsVisible = false;
                    firststack.IsVisible = true;
                    backbtn.Text = "Cancel";
                    topprogress.Progress = 16.67;

                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //delete any notifications
            if (IsEdit)
            {
                newusermedication = SelectedMed;

                foreach (var item in newusermedication.schedule)
                {
                    LocalNotificationCenter.Current.Cancel(item.id);
                }

                //add the details
                //  SelectedMed.preparation = newusermedication.preparation;
                //  SelectedMed.formulation = medformulationslistview.SelectedItem.ToString();
                //  SelectedMed.unit = meddosageunitlistview.SelectedItem.ToString();
                //  SelectedMed.startdate = startdatepicker.Date.ToString("dd/MM/yyyy");

                //  if(enddatecheck.IsChecked)
                //  {
                //      SelectedMed.enddate = enddatepicker.Date.ToString("dd/MM/yyyy");
                //  }

            }

            //schedule notifications


            var daycount = 0;
            var mednottitle = "Supplement Reminder";

            //used for daily and days interval notifications
            foreach (var item in selectedDosages)
            {
                //add an id so we can cancel it at anytime
                Random random = new Random();
                int randomNumber = random.Next(100000, 100000001);

                item.id = randomNumber;

                item.time = item.timeconverted.ToString(@"hh\:mm");


                if (freqstring == "Daily")
                {
                    if (string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        await ScheduleNotifications.DailyNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate);
                    }
                    else
                    {
                        await ScheduleNotifications.DailyWithEndDateNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, newusermedication.enddate);
                    }


                }
                else if (freqstring == "Days Interval")
                {

                    var DIdaycount = daysintervallistview.SelectedItem.ToString();



                    if (DIdaycount == "Every other day")
                    {
                        daycount = 2;
                    }
                    else if (DIdaycount == "Every 3 days")
                    {
                        daycount = 3;
                    }
                    else if (DIdaycount == "Every 7 days")
                    {
                        daycount = 7;
                    }
                    else if (DIdaycount == "Every 14 days")
                    {
                        daycount = 14;
                    }
                    else if (DIdaycount == "Every 21 days")
                    {
                        daycount = 21;
                    }
                    else if (DIdaycount == "Every 28 days")
                    {
                        daycount = 28;
                    }
                    else if (DIdaycount == "Every 30 days")
                    {
                        daycount = 30;
                    }
                    else if (DIdaycount == "Every 5 weeks")
                    {
                        daycount = 35; // 5 weeks * 7 days
                    }
                    else if (DIdaycount == "Every 6 weeks")
                    {
                        daycount = 42; // 6 weeks * 7 days
                    }
                    else if (DIdaycount == "Every 7 weeks")
                    {
                        daycount = 49; // 7 weeks * 7 days
                    }
                    else if (DIdaycount == "Every 8 weeks")
                    {
                        daycount = 56; // 8 weeks * 7 days
                    }
                    else if (DIdaycount == "Every 9 weeks")
                    {
                        daycount = 63; // 9 weeks * 7 days
                    }
                    else if (DIdaycount == "Every 10 weeks")
                    {
                        daycount = 70; // 10 weeks * 7 days
                    }

                    if (string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        await ScheduleNotifications.DaysIntervalNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, daycount);
                    }
                    else
                    {
                        await ScheduleNotifications.DaysIntervalWithEndDateNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, newusermedication.enddate, daycount);
                    }
                }

            }



            //get the days for weekly
            var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

            //weekly are done differnet so it is below
            if (freqstring == "Specfic Days of the Week")
            {
                //means their is multiple days in list
                if (selectedDosages[0].Day.Contains(','))
                {

                    foreach (var d in listofdays) // Iterate over the list of days first
                    {
                        for (int i = 0; i < selectedDosages.Count; i++) // Then iterate over selectedDosages for each day
                        {
                            var md = selectedDosages[i];
                            Random random = new Random();
                            int randomNumber = random.Next(100000, 100000001);

                            // Create a new instance of MedtimesDosages and copy the relevant properties
                            var newMd = new MedtimesDosages
                            {
                                id = randomNumber,
                                Dosage = md.Dosage,
                                dosageunit = md.dosageunit,
                                time = md.time,
                                timeconverted = md.timeconverted,
                                Day = d // Assign the current day
                            };

                            WeeklyChangedselectedDosages.Add(newMd); // Add the new instance to the collection
                        }
                    }

                    selectedDosages = WeeklyChangedselectedDosages;

                }
                else
                {
                    //means the days are already in there own record


                }


                //schdeule notifications using new collection now

                foreach (var item in selectedDosages)
                {
                    if (string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        await ScheduleNotifications.WeeklyNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, item.Day);
                    }
                    else
                    {
                        await ScheduleNotifications.WeeklyWithEndDateNotifications(mednottitle, item.id, newusermedication.supplementtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, item.Day, newusermedication.enddate);
                    }

                }

            }




            //medication add 

            if (freqstring == "Daily")
            {
                newusermedication.frequency = "Daily" + "|" + selectedDosages.Count.ToString();
            }
            else if (freqstring == "Specfic Days of the Week")
            {
                var joindays = string.Join(',', listofdays);

                //count how many times per day
                var newweeknum = "";

                if (weeklynumperday == "One")
                {
                    newweeknum = "1";
                }
                else if (weeklynumperday == "Two")
                {
                    newweeknum = "2";
                }
                else if (weeklynumperday == "Three")
                {
                    newweeknum = "3";
                }
                else if (weeklynumperday == "Four")
                {
                    newweeknum = "4";
                }
                else if (weeklynumperday == "Five")
                {
                    newweeknum = "5";
                }
                else if (weeklynumperday == "Six")
                {
                    newweeknum = "6";
                }
                else if (weeklynumperday == "Seven")
                {
                    newweeknum = "7";
                }
                else if (weeklynumperday == "Eight")
                {
                    newweeknum = "8";
                }

                newusermedication.frequency = "Weekly" + "|" + joindays + "|" + newweeknum;

                //add the questions
                if (sameweeklydosage2.IsVisible == true && sameweeklydosage.IsVisible == true)
                {
                    newusermedication.supplementquestions = samedosageweeklylist.SelectedItem.ToString() + "|" + samedosageweeklylist2.SelectedItem.ToString();
                }
                else if (sameweeklydosage2.IsVisible == true && sameweeklydosage.IsVisible == false)
                {
                    newusermedication.supplementquestions = samedosageweeklylist2.SelectedItem.ToString();
                }
                else if (sameweeklydosage2.IsVisible == false && sameweeklydosage.IsVisible == false)
                {
                    //do nothing
                }
                else
                {
                    newusermedication.supplementquestions = samedosageweeklylist.SelectedItem.ToString();
                }

            }
            else if (freqstring == "Days Interval")
            {
                newusermedication.frequency = "Days Interval" + "|" + daycount.ToString() + "|" + selectedDosages.Count.ToString();

                if (disamedosagequestionlist.IsVisible)
                {
                    newusermedication.supplementquestions = disamedosagequestionlist.SelectedItem.ToString();
                }
            }
            else
            {
                newusermedication.frequency = "As Required" + "|" + "0";
            }

            // Convert list to ObservableCollection
            ObservableCollection<MedtimesDosages> observableItemList = new ObservableCollection<MedtimesDosages>(selectedDosages);

            newusermedication.schedule = observableItemList;

            APICalls database = new APICalls();
            //insert to db
            string userid = Preferences.Default.Get("userid", "Unknown");

            newusermedication.status = "Active";
            newusermedication.userid = userid;

            //add timedosages so i can update ok locally 
            var Split = newusermedication.frequency.Split('|');

            if (newusermedication.TimeDosage.Count == 0)
            {
                foreach (var feedback in newusermedication.schedule)
                {
                    var dosage = feedback.Dosage;
                    var time = feedback.time;
                    //Daily
                    var getfreq = newusermedication.frequency.Split('|');
                    if (getfreq[0] == "Daily" || getfreq[0] == "Days Interval")
                    {
                        var DosageTime = time + "|" + dosage;
                        newusermedication.TimeDosage.Add(DosageTime);
                    }
                    //Weekly
                    else if (getfreq[0] == "Weekly" || getfreq[0] == "Weekly ")
                    {
                        var freq = newusermedication.frequency.Split('|');
                        if (freq[1].Contains(","))
                        {
                            var days = freq[1].Split(',').ToList();
                            for (int i = 0; i < days.Count; i++)
                            {
                                var day = days[i];
                                var DosageTime = time + "|" + dosage + "|" + day;
                                newusermedication.TimeDosage.Add(DosageTime);
                            }
                        }
                        else
                        {
                            var day = freq[1];
                            var DosageTime = time + "|" + dosage + "|" + day;
                            newusermedication.TimeDosage.Add(DosageTime);
                        }


                    }
                }
            }


            if (!string.IsNullOrEmpty(newusermedication.id))
            {
                var findmed = UserMedications.Where(med => med.id == newusermedication.id).First();

                UserMedications.Remove(findmed);

                await database.UpdateSupplementDetails(newusermedication);



                newusermedication.TimeDosage.Clear();

                foreach (var feedback in newusermedication.schedule)
                {

                    var dosage = feedback.Dosage;
                    var time = feedback.time;
                    //Daily
                    var getfreq = newusermedication.frequency.Split('|');
                    if (getfreq[0] == "Daily" || getfreq[0] == "Days Interval")
                    {
                        var DosageTime = time + "|" + dosage;
                        newusermedication.TimeDosage.Add(DosageTime);
                    }
                    //Weekly
                    else if (getfreq[0] == "Weekly" || getfreq[0] == "Weekly ")
                    {
                        var day = feedback.Day;
                        var DosageTime = time + "|" + dosage + "|" + day;
                        newusermedication.TimeDosage.Add(DosageTime);
                    }

                }


                UserMedications.Add(newusermedication);
            }
            else
            {
                var returnedsymptom = await database.PostSupplementAsync(newusermedication);
                UserMedications.Add(newusermedication);
            }

            if (IsEdit)
            {
                await MopupService.Instance.PushAsync(new PopupPageHelper("Supplement Updated") { });
                await Task.Delay(1500);
            }
            else
            {
                await MopupService.Instance.PushAsync(new PopupPageHelper("Supplement Added") { });
                await Task.Delay(1500);
            }



            //await Navigation.PushAsync(new AllSymptoms(SymptomsPassed));
            await Navigation.PushAsync(new AllSupplements(UserMedications));


            await MopupService.Instance.PopAllAsync(false);

            var pageToRemoves = Navigation.NavigationStack.ToList();

            int ii = 0;

            foreach (var page in pageToRemoves)
            {
                if (ii == 0)
                {
                }
                else if (ii == 1 || ii == 2 || ii == 3)
                {
                    Navigation.RemovePage(page);
                }
                else
                {
                    //Navigation.RemovePage(page);
                }
                ii++;
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void timesanddosageslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            if (IsEdit)
            {
                var item = e.DataItem as MedtimesDosages;

                //var finditem = selectedDosages.Where(x => x.time == item.time).Where(x => x.Dosage == item.Dosage).FirstOrDefault();



            }

        }
        catch (Exception ex)
        {

        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {

            //taken (tick) tapped on schedule

            // Get the tapped Image
            ExtendedLabel label = (ExtendedLabel)sender;

            var getitem = selectedDosages.Where(x => x.time == label.Time).Where(x => x.Dosage == label.Dosage).FirstOrDefault();


            getitem.Labelvis = false;
            getitem.Dosage = string.Empty;
            getitem.Dosage2 = string.Empty;
            getitem.Entryvis = true;


        }
        catch (Exception ex)
        {

        }
    }


    private void samedosageentry2_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {

            }
            else
            {
                foreach (var item in selectedDosages)
                {
                    item.Dosage2 = e.NewTextValue;
                }
            }


            //if (!string.IsNullOrEmpty(e.NewTextValue))
            //{
            //    // Get the current text without the unit
            //    string currentTextWithoutUnit = samedosageentry.Text.Replace($" {newusermedication.unit}", "");

            //    // Set the new text with the unit appended only once
            //    samedosageentry.Text = e.NewTextValue + " " + newusermedication.unit;
            //}


        }
        catch (Exception ex)
        {

        }
    }
}