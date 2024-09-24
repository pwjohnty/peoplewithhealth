using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq.Expressions;
using Plugin.LocalNotification;
using Mopups.Services;
//using Windows.Foundation.Metadata;

namespace PeopleWith;

public partial class AddMedication : ContentPage
{
    HttpClient client = new HttpClient();
    ObservableCollection<medication> allmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<medication> FilterResults = new ObservableCollection<medication>();
    usermedication newusermedication = new usermedication();
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

    ObservableCollection<MedtimesDosages> medtimesanddosages = new ObservableCollection<MedtimesDosages>();
    ObservableCollection<usermedication> UserMedications = new ObservableCollection<usermedication>();
    public AddMedication()
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
         new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       { "Night", new List<MedtimesDosages> {
         new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
          { "own time", new List<MedtimesDosages> {
         new MedtimesDosages { time = DateTime.Now.ToString("HH:mm"), timeconverted = DateTime.Now.TimeOfDay, Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Twice Daily", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Three Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       {
    "Four Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};

        frequencyOptionsWDI = new Dictionary<string, List<MedtimesDosages>>()
{
    { "One", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Two", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Three", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       {
    "Four", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
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

    public AddMedication(ObservableCollection<usermedication> AllUsermedications)
    {
        InitializeComponent();


        UserMedications = AllUsermedications;

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
         new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       { "Night", new List<MedtimesDosages> {
         new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
          { "own time", new List<MedtimesDosages> {
         new MedtimesDosages { time = DateTime.Now.ToString("HH:mm"), timeconverted = DateTime.Now.TimeOfDay, Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Twice Daily", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Three Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       {
    "Four Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight Times Daily", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    // Add the other options similarly
};

        frequencyOptionsWDI = new Dictionary<string, List<MedtimesDosages>>()
{
    { "One", new List<MedtimesDosages> {
        new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Two", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
    {
    "Three", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
       {
    "Four", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
            {
    "Five", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "11:00", timeconverted = new TimeSpan(11, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "17:00", timeconverted = new TimeSpan(17, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                        {
    "Six", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:30", timeconverted = new TimeSpan(10, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:00", timeconverted = new TimeSpan(13, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "15:30", timeconverted = new TimeSpan(15, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                {
    "Seven", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "10:00", timeconverted = new TimeSpan(10, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "12:00", timeconverted = new TimeSpan(12, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "14:00", timeconverted = new TimeSpan(14, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "16:00", timeconverted = new TimeSpan(16, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:00", timeconverted = new TimeSpan(18, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
    }},
                                                                                                {
    "Eight", new List<MedtimesDosages> {
      new MedtimesDosages { time = "08:00", timeconverted = new TimeSpan(8, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
       new MedtimesDosages { time = "09:45", timeconverted = new TimeSpan(9, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "11:30", timeconverted = new TimeSpan(11, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "13:15", timeconverted = new TimeSpan(13, 15, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "15:00", timeconverted = new TimeSpan(15, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
             new MedtimesDosages { time = "16:45", timeconverted = new TimeSpan(16, 45, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
         new MedtimesDosages { time = "18:30", timeconverted = new TimeSpan(18, 30, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit},
        new MedtimesDosages { time = "20:00", timeconverted = new TimeSpan(20, 0, 0), Dosage = string.Empty,  dosageunit = newusermedication.unit}
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

    async void Getmedications()
	{
		try
		{
            //get all medications
           // medsloading.IsVisible = true;

            var urlmedications = APICalls.GetMedications;

            HttpResponseMessage responsemeds = await client.GetAsync(urlmedications);

            if (responsemeds.IsSuccessStatusCode)
            {
                string contentmeds = await responsemeds.Content.ReadAsStringAsync();
                var userResponsemed = JsonConvert.DeserializeObject<ApiResponseMedication>(contentmeds);
                var meds = userResponsemed.Value;

                allmedicationlist = meds;

                Medicationslistview.ItemsSource = allmedicationlist;

               // medsloading.IsVisible = false;

               

            }


            //get the preparations
            MedPreparations = await aPICalls.GetMedPreparation();
            


        }
		catch(Exception ex)
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

                if(countofcharacters > 2)
                {
                    var Characters = e.NewTextValue;
                    var filteredmeds = new ObservableCollection<medication>(allmedicationlist.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
                   
                    Medicationslistview.ItemsSource = filteredmeds;
                    Medicationslistview.IsVisible = true;
                   // Medicationslistview.HeightRequest = filteredmeds.Count * 50;
                }
         
            }

        }
        catch(Exception ex)
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

            var item = e.DataItem as medication;


            newusermedication.medicationid = item.medicationid;
            newusermedication.medicationtitle = item.title;

            mednamelbl.Text = item.title;
            medname2lbl.Text = item.title;
            medname3lbl.Text = item.title;
            medname4lbl.Text = item.title;
            medname5lbl.Text = item.title;


        }
        catch(Exception ex)
        { 
        }
    }

  
    private async void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (firststack.IsVisible == true)
            {
                //check if they have selected a medication
                if (Medicationslistview.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    await DisplayAlert("Select Medication", "Please select a medication from this list", "Ok");
                    return;
                }
                else
                {
                    medpreparationslistview.ItemsSource = MedPreparations;
                    firststack.IsVisible = false;
                    secondstack.IsVisible = true;
                    backbtn.IsVisible = true;
                    topprogress.Progress = 40;
                    backbtn.Text = "Back";
                }
            }

            else if(secondstack.IsVisible == true)
            {
                if(medpreparationslistview.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    await DisplayAlert("Select Medication Preparation", "Please select a medication preparation from this list", "Ok");
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
                    topprogress.Progress = 60;
                }
            }
            else if(thirdstack.IsVisible == true)
            {

                if(enddatecheck.IsChecked)
                {
                    newusermedication.enddate = enddatepicker.Date.ToString("dd/MM/yyyy");
                }

                newusermedication.startdate = startdatepicker.Date.ToString("dd/MM/yyyy");


                thirdstack.IsVisible = false;
                fourthstack.IsVisible = true;
                topprogress.Progress = 80;
                medfreqlistview.IsVisible = true;

            }

            else if(fourthstack.IsVisible == true)
            {
                //add validation here


                fourthstack.IsVisible = false;
                detailsstack.IsVisible = true;
            }
            else if(detailsstack.IsVisible == true)
            {
                var newlist = new List<string>();

                if(string.IsNullOrEmpty(displaynameentry.Text))
                {
                    newlist.Add("--");
                    confirmdisplaynamelbl.Text = newusermedication.medicationtitle;
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

                if(string.IsNullOrEmpty(newusermedication.formulation))
                {
                    confirmformlbl.Text = "--";
                }
                else
                {
                    confirmformlbl.Text = newusermedication.formulation;
                }

                confirmdulbl.Text = newusermedication.unit;
                confirmsdlbl.Text = newusermedication.startdate;

                if(string.IsNullOrEmpty(newusermedication.enddate))
                {
                    confirmedlbl.Text = "--";
                }
                else
                {
                    confirmedlbl.Text = newusermedication.enddate;
                }

                confirmfreqlbl.Text = freqstring;

                
                confirmtimesanddosageslistview.ItemsSource = selectedDosages;
                confirmtimesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
              //  confirmtimesanddosageslistview.IsEnabled = false;

                detailsstack.IsVisible = false;
                confirmstack.IsVisible = true;
                nextbtn.IsVisible = false;

            }



        }
        catch (Exception ex)
        {

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
            
        }
        catch(Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            if(!e.Value)
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
        catch(Exception ex)
        {

        }
    }

    private void medfreqlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as preparation;

            freqstring = item.title;

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
            else if(item.title == "Specfic Days of the Week")
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
            else if(item.title == "Days Interval")
            {
                freqlbl.Text = "How often";
                dailytimeslistview.IsVisible = false;
                weeklydayslist.IsVisible= false;
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

                dailytimeslistview.SelectedItems.Clear();
                oncedailylistview.SelectedItems.Clear();
                daysintervallistview.SelectedItems.Clear();
                weeklydayslist.SelectedItems.Clear();
                weekfreqlistview.SelectedItems.Clear();
            }



        }
        catch(Exception ex)
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

                    selectedDosages[0].Alldosage = false;

                    foreach(var itemm in selectedDosages)
                    {
                        itemm.Dosage = string.Empty;
                    }

                    // Bind the MedtimesDosages list to the ListView
                    timesanddosageslistview.ItemsSource = selectedDosages;
                   // timesanddosageslistview.IsVisible = true;
                   // timeanddosagelbl.IsVisible = true;
                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;
                }
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void weeklydayslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

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
        catch(Exception ex)
        {

        }
    }

    private void meddosageunitlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;


            newusermedication.unit = item;

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
        catch(Exception ex)
        {

        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            // Get the Entry that triggered the event
            var entry = (Entry)sender;

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
                    selectedDosages[0].Alldosage = false;
                }
            }



        }
        catch(Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if(e.Value)
            {
                var firstdosage = selectedDosages[0].Dosage;

                foreach (var item in selectedDosages)
                {
                    item.Dosage = firstdosage;
                }

                selectedDosages[0].Alldosage = false;
                selectedDosages[0].Checkboxchecked = false;

                timesanddosageslistview.HeightRequest = timesanddosageslistview.Height + 100;


            }
            
        }
        catch(Exception ex)
        {

        }
    }

    private void weekfreqlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //weekly how many times per day

            var item = e.DataItem as string;

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
                    }
                        itemm.Dosage = string.Empty;
                }

                if(weeklynumperday == "One" && weeklydayslist.SelectedItems.Count == 1)
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
                else if(weeklynumperday == "One" && weeklydayslist.SelectedItems.Count > 1)
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
        catch(Exception ex)
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

            if(item == "Yes")
            {

                if (weeklynumperday == "One")
                {
                    //dont show second list as only one 
                    //show the label and hide the entry

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

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
                        sd.Entryvis = true;
                        sd.Labelvis = false;
                    }

                    adddosagelbl.IsVisible = false;
                    adddosage2lbl.IsVisible = false;
                    adddosageframe.IsVisible = false;
                }

                    sameweeklydosage2.IsVisible = false;
                samedosageweeklylist2.IsVisible = false;


                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptionsWDI[weeklynumperday];

                selectedDosages[0].Alldosage = false;

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
                }

                timesanddosageslistview.ItemsSource = selectedDosages;
                timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;

                adddosagelbl.IsVisible = false;
                adddosage2lbl.IsVisible = false;
                adddosageframe.IsVisible = false;
                samedosageentry.Text = string.Empty;

                timesanddosageslistview.IsVisible = true;
                timeanddosagelbl.IsVisible = true;
                timeanddosagelbl2.IsVisible = true;

            }


        }
        catch(Exception ex)
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
                foreach(var item in selectedDosages)
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
        catch(Exception ex)
        {

        }
    }

    private void samedosageweeklylist2_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            samedosageentry.Text = string.Empty;

            if (item == "Yes")
            {

                //check was freq it is

                if(freqstring == "Daily")
                {
                    //show the label and hide the entry

                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

                    timesanddosageslistview.HeightRequest = selectedDosages.Count * 130;
                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;

                }
                else if(freqstring == "Specfic Days of the Week")
                {
                    foreach (var sd in selectedDosages)
                    {
                        sd.Dosage = string.Empty;
                        sd.Entryvis = false;
                        sd.Labelvis = true;
                    }

                    adddosagelbl.IsVisible = true;
                    adddosage2lbl.IsVisible = true;
                    adddosageframe.IsVisible = true;

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
                        sd.Entryvis = true;
                        sd.Labelvis = false;
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
                        sd.Entryvis = true;
                        sd.Labelvis = false;
                    }


                    timesanddosageslistview.IsVisible = true;
                    timeanddosagelbl.IsVisible = true;
                    timeanddosagelbl2.IsVisible = true;
                }
            }



            
        }
        catch(Exception ex)
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
        catch(Exception ex)
        {

        }
    }

    private void oncedailylistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {


            var item = e.DataItem as string;


            var splititem = item.Split('\n');
            // Check if the selected option exists in the dictionary
            if (frequencyOptions.ContainsKey(splititem[1]))
            {
                // Get the corresponding MedtimesDosages list for the selected option
                selectedDosages = frequencyOptions[splititem[1]];

                selectedDosages[0].Alldosage = false;

                foreach (var itemm in selectedDosages)
                {
                    itemm.Labelvis = false;
                    itemm.Entryvis = true;
                    itemm.Dosage = string.Empty;
                }

                // Bind the MedtimesDosages list to the ListView
                timesanddosageslistview.ItemsSource = selectedDosages;
                 timesanddosageslistview.IsVisible = true;
                 timeanddosagelbl.IsVisible = true;
                timeanddosagelbl2.IsVisible = true;
                timesanddosageslistview.HeightRequest = selectedDosages.Count * 100;
            }

        }
        catch(Exception ex)
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
        catch(Exception ex)
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

                selectedDosages[0].Alldosage = false;

                foreach (var itemm in selectedDosages)
                {
                    itemm.Dosage = string.Empty;
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
        catch(Exception ex)
        {

        }
    }

    private void disamedosagequestionlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            samedosageentry.Text = string.Empty;


            if(item == "Yes")
            {
                adddosagelbl.IsVisible = true;
                adddosage2lbl.IsVisible = true;
                adddosageframe.IsVisible = true;

                foreach(var sd in selectedDosages)
                {
                    sd.Dosage = string.Empty;
                    sd.Entryvis = false;
                    sd.Labelvis = true;
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
                    sd.Entryvis = true;
                    sd.Labelvis = false;
                }
            }

            timesanddosageslistview.IsVisible = true;
            timeanddosagelbl.IsVisible = true;
            timeanddosagelbl2.IsVisible = true;



        }
        catch(Exception ex)
        {

        }
    }

    private void medformulationslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;


            newusermedication.formulation = item;
        }
        catch(Exception ex)
        {

        }
    }

    private void startdatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);
        }
        catch(Exception ex)
        {

        }
    }

    private void backbtn_Clicked(object sender, EventArgs e)
    {
        //back button clicked
        try
        {

            if(backbtn.Text == "Cancel")
            {
                Navigation.RemovePage(this);
            }
            else if(confirmstack.IsVisible == true)
            {
                confirmstack.IsVisible = false;
                detailsstack.IsVisible = true;
                backbtn.Text = "Back";
                nextbtn.IsVisible = true;
            }
            else if(detailsstack.IsVisible == true)
            {
                detailsstack.IsVisible = false;
                fourthstack.IsVisible = true;
            }
            else if (fourthstack.IsVisible == true)
            {
                fourthstack.IsVisible = false;
                thirdstack.IsVisible = true;
            }
            else if(thirdstack.IsVisible == true)
            {
                thirdstack.IsVisible = false;
                secondstack.IsVisible = true;
            }
            else if(secondstack.IsVisible == true)
            {
                secondstack.IsVisible = false;
                firststack.IsVisible = true;
                backbtn.Text = "Cancel";
                    
            }

        }
        catch(Exception ex)
        {

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {


            //schedule notifications


            var daycount = 0;
            var mednottitle = "Medication Reminder";

            //used for daily and days interval notifications
            foreach(var item in selectedDosages)
            {
                //add an id so we can cancel it at anytime
                Random random = new Random();
                int randomNumber = random.Next(100000, 100000001);

                item.id = randomNumber;


                if(freqstring == "Daily")
                {
                    if(string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        await ScheduleNotifications.DailyNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate);
                    }
                    else
                    {
                        await ScheduleNotifications.DailyWithEndDateNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, newusermedication.enddate);
                    }


                }
                else if(freqstring == "Days Interval")
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
                        await ScheduleNotifications.DaysIntervalNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, daycount);
                    }
                    else
                    {
                        await ScheduleNotifications.DaysIntervalWithEndDateNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, newusermedication.enddate, daycount);
                    }
                }

            }



            //get the days for weekly
            var listofdays = weeklydayslist.SelectedItems.Cast<string>().ToList();

            //weekly are done differnet so it is below
            if(freqstring == "Specfic Days of the Week")
            {
                //means their is multiple days in list
                if (selectedDosages[0].Day.Contains(','))
                {

                    foreach (var md in selectedDosages)
                    {
                        for (int i = 0; i < listofdays.Count; i++)
                        {
                            var d = listofdays[i];
                            Random random = new Random();
                            int randomNumber = random.Next(100000, 100000001);
                            // Create a new instance of MedtimesDosages (assuming md is of this type) and copy the relevant properties
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

                foreach(var item in selectedDosages)
                {
                    if (string.IsNullOrEmpty(newusermedication.enddate))
                    {
                        await ScheduleNotifications.WeeklyNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, item.Day);
                    }
                    else
                    {
                        await ScheduleNotifications.WeeklyWithEndDateNotifications(mednottitle, item.id, newusermedication.medicationtitle, item.Dosage, item.dosageunit, item.timeconverted, newusermedication.startdate, item.Day, newusermedication.enddate);
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
                
                if(weeklynumperday == "One")
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
            }
            else if (freqstring == "Days Interval")
            {
                newusermedication.frequency = "Days Interval" + "|" + daycount.ToString() + "|" + selectedDosages.Count.ToString();
            }
            else
            {
                newusermedication.frequency = "As Required";
            }

            // Convert list to ObservableCollection
            ObservableCollection<MedtimesDosages> observableItemList = new ObservableCollection<MedtimesDosages>(selectedDosages);

            newusermedication.schedule = observableItemList;

            APICalls database = new APICalls();
            //insert to db
            string userid = Preferences.Default.Get("userid", "Unknown");

            newusermedication.userid = userid;
            var returnedsymptom = await database.PostMedicationAsync(newusermedication);

            UserMedications.Add(newusermedication);

            await MopupService.Instance.PushAsync(new PopupPageHelper("Medication Added") { });
            await Task.Delay(1500);

            //await Navigation.PushAsync(new AllSymptoms(SymptomsPassed));
            await Navigation.PushAsync(new AllMedications(UserMedications));


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
}