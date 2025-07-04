//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using Maui.FreakyControls.Extensions;
using System.Threading.Tasks;
using Syncfusion.Maui.DataSource.Extensions;
//using static AndroidX.Core.Text.Util.LocalePreferences.FirstDayOfWeek;

namespace PeopleWith;

public partial class AllMedications : ContentPage
{
	APICalls aPICalls = new APICalls();
    //public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> CompletedMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> PendingMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> CurrentMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> AsRequiredMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> AddinAsRequired = new ObservableCollection<usermedication>();
    private bool IsLoading = true;

    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public AllMedications()
	{
        try
        {
            InitializeComponent();
            BindingContext = this;
            getusermedications();
            //if (AllUserMedications.Count == 0 && CompletedMedications.Count == 0 && AsRequiredMedications.Count == 0)
            //{
            //    nodatastack.IsVisible = true;
            //    datastack.IsVisible = false;
            //}
            //else if (AllUserMedications.Count == 0)
            //{
            //    noActivemedlbl.IsVisible = true;
            //    AllUserMedsList.IsVisible = false; 
            //}
            //else
            //{
            //    nodatastack.IsVisible = false;
            //    datastack.IsVisible = true;

            //    AllUserMedsList.ItemsSource = AllUserMedications;

            //    WorkOutNextDue();
            //}

            WeakReferenceMessenger.Default.Register<UpdateAllMeds>(this, (r, m) =>
            {

                AllUserMedications = (ObservableCollection<usermedication>)m.Value;

            });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AllMedications(ObservableCollection<usermedication> AllUsermeds)
    {
        try
        {
            InitializeComponent();
            BindingContext = this;
            AllUserMedications.Clear();

            AllUserMedications = AllUsermeds;

            WorkOutNextDue();

            //if (AllUserMedications.Count == 0)
            //{
            //    nodatastack.IsVisible = true;
            //    datastack.IsVisible = false;
            //}
            //else
            //{
            //    nodatastack.IsVisible = false;
            //    datastack.IsVisible = true;

            //    AllUserMedsList.ItemsSource = AllUserMedications;

            //    WorkOutNextDue();
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void NovoConsentData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                var signup = Helpers.Settings.SignUp;
                if (signup.Contains("SAX"))
                { //All Novo SignupCodes 
                    NovoConsent.IsVisible = true;
                    NovoContentlbl.Text = Preferences.Default.Get("NovoContent", String.Empty);
                    NovoExitidlbl.Text = Preferences.Default.Get("NovoExitid", String.Empty);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getusermedications()
	{
		try
        {
            MedsLoading.IsVisible = true;
           
            var getMedicationsTask =  aPICalls.GetUserMedicationsAsync();

            //var delayTask = Task.Delay(500);

            //if (await Task.WhenAny(getMedicationsTask, delayTask) == delayTask)
            //{
               // await MopupService.Instance.PushAsync(new GettingReady("Loading Medications") { });
            //}
        
            AllUserMedications = await getMedicationsTask;
            
            WorkOutNextDue();
            

            
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void WorkOutNextDue()
    {
        try
        {
            //Change if taken/

            foreach (var item in AllUserMedications)
            {
                if (item.status == "Pending")
                {

                }
                else
                {
                    if (item.frequency.Contains("|"))
                    {
                        var Freq = item.frequency.Split('|');
                        if (Freq[0] == "As Required")
                        {
                            item.NextDosage = Freq[1];
                            item.NextTime = "As Required";
                        }
                        else
                        {
                            item.MedFrequency = Freq[0];
                            DateTime currentTime = DateTime.Now;
                            List<DateTime> times = new List<DateTime>();
                            var freq = item.frequency.Split('|');

                            foreach (var x in item.TimeDosage)
                            {
                                var parts = x.Split('|');
                                if (parts.Length > 0)
                                {
                                    //string[] formats = { "HH:mm", "HH:mm:ss" };
                                    if (DateTime.TryParse(parts[0], out DateTime parsedTime))
                                    {
                                        //string formattedTime = parsedTime.ToString("HH:mm");
                                        times.Add(parsedTime);
                                    }
                                }
                            }

                       
                                // Find the next time that is greater than the current time
                                var nextTimeDue = times.Where(t => t > currentTime).OrderBy(t => t).FirstOrDefault();


                                if (nextTimeDue == DateTime.MinValue)
                                {
                                    nextTimeDue = times[0];
                                }
                            

                            //Daily 
                            if (freq[0] == "Daily")
                            {


                                if (nextTimeDue == default(DateTime))
                                {
                                    times = times.OrderBy(t => t).ToList();
                                    var firstTimeForTomorrow = times.First();



                                    // Find the corresponding dosage for the first time
                                    foreach (var p in item.TimeDosage)
                                    {
                                        var split = p.Split('|');
                                        if (DateTime.TryParse(split[0], out DateTime timePart))
                                        {
                                            if (timePart == firstTimeForTomorrow)
                                            {
                                                if (split.Length == 3)
                                                {
                                                    item.NextDosage = split[1] + "|" + split[2];
                                                }
                                                else
                                                {
                                                    item.NextDosage = split[1];
                                                }

                                                break;
                                            }
                                        }
                                    }

                                    if (item.NextDosage.Contains("|"))
                                    {
                                        var split = item.NextDosage.Split('|');
                                        //Unit Split 
                                        var unitsplit = item.unit.Split(' ');
                                        var Unituno = unitsplit[0] + " " + unitsplit[1];
                                        var unitdos = unitsplit[2];

                                        item.NextTime = "Tomorrow " + firstTimeForTomorrow.ToString("HH:mm") + " - " + split[0] + " " + Unituno + " " + split[1] + " " + unitdos;
                                    }
                                    else
                                    {

                                        item.NextTime = "Tomorrow " + firstTimeForTomorrow.ToString("HH:mm") + " - " + item.NextDosage + " " + item.unit;
                                    }
                                }
                                else
                                {


                                    // Find the Correct dosage for the nextTimeDue
                                    foreach (var p in item.TimeDosage)
                                    {
                                        var split = p.Split('|');
                                        //string[] formats = { "HH:mm", "HH:mm:ss" };
                                        if (DateTime.TryParse(split[0], out DateTime timePart))
                                        {
                                            if (timePart == nextTimeDue)
                                            {
                                                if (split.Length == 3)
                                                {
                                                    item.NextDosage = split[1] + "|" + split[2];
                                                }
                                                else
                                                {
                                                    item.NextDosage = split[1];
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    // Set the NextTime 
                                    if (item.NextDosage.Contains("|"))
                                    {
                                        var split = item.NextDosage.Split('|');
                                        //Unit Split 
                                        var unitsplit = item.unit.Split(' ');
                                        var Unituno = unitsplit[0] + " " + unitsplit[1];
                                        var unitdos = unitsplit[2];
                                        item.NextTime = "Today " + nextTimeDue.ToString("HH:mm") + " - " + split[0] + " " + Unituno + " " + split[1] + " " + unitdos;
                                    }
                                    else
                                    {

                                        item.NextTime = "Today " + nextTimeDue.ToString("HH:mm") + " - " + item.NextDosage + " " + item.unit;
                                    }
                                }
                            }
                            //Weekly 
                            else if (freq[0] == "Weekly" || freq[0] == "Weekly ")
                            {
                                int index = 0;
                                var freqq = item.frequency.Split('|');
                                if (freqq[1].Contains(","))
                                {
                                    var days = freqq[1].Split(',').ToList();

                                    item.MedFrequency = Freq[0] + " - Every " + string.Join(", ", days);


                                    //check if today is the same day 

                                    var checkday = DateTime.Now.ToString("ddd");

                                    var weekdaystring = "";

                                    if (days.Contains(checkday))
                                    {

                                        weekdaystring = "Today";


                                        // Find the Correct dosage for the nextTimeDue
                                        foreach (var p in item.TimeDosage)
                                        {
                                            var split = p.Split('|');
                                            if (DateTime.TryParse(split[0], out DateTime timePart))
                                            {
                                                if (timePart == nextTimeDue)
                                                {
                                                    if (split.Count() == 4)
                                                    {
                                                        item.NextDosage = split[1] + "|" + split[2];
                                                    }
                                                    else
                                                    {
                                                        item.NextDosage = split[1];
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        var today = DateTime.Now.ToString("ddd");

                                        // Predefined order of the week starting from Monday to Sunday
                                        var weekDaysOrder = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

                                        // Find today's index in the ordered list
                                        int todayIndex = weekDaysOrder.IndexOf(today);

                                        string nextDay = null;

                                        // Loop through the ordered days of the week, starting from the day after today
                                        for (int i = 1; i <= weekDaysOrder.Count; i++)
                                        {
                                            
                                            int nextDayIndex = (todayIndex + i) % weekDaysOrder.Count;
                                            string potentialNextDay = weekDaysOrder[nextDayIndex];

                                            // Check if the potential next day is in the 'days' list
                                            if (days.Contains(potentialNextDay))
                                            {
                                                weekdaystring = potentialNextDay;
                                                break;
                                            }
                                        }

                                        foreach (var p in item.TimeDosage)
                                        {
                                            var split = p.Split('|');

                                            if (split.Count() == 4)
                                            {
                                                item.NextDosage = split[1] + "|" + split[2];
                                            }
                                            else
                                            {
                                                item.NextDosage = split[1];
                                            }
                                            break;

                                        }
                                    }



                                    // Set the NextTime 
                                    if (item.NextDosage.Contains("|"))
                                    {
                                        var split = item.NextDosage.Split('|');
                                        //Unit Split 
                                        var unitsplit = item.unit.Split(' ');
                                        var Unituno = unitsplit[0] + " " + unitsplit[1];
                                        var unitdos = unitsplit[2];

                                        item.NextTime = weekdaystring + " " + nextTimeDue.ToString("HH:mm") + " - " + split[0] + " " + Unituno + " " + split[1] + " " + unitdos;
                                    }
                                    else
                                    {

                                        item.NextTime = weekdaystring + " " + nextTimeDue.ToString("HH:mm") + " - " + item.NextDosage + " " + item.unit;
                                    }


                                }
                                else
                                {
                                    var getday = freq[1];
                                    item.MedFrequency = Freq[0] + " - Every " + getday;

                                    //check if today is the same day 
                                    //check if today is the same day 

                                    var days = new List<string>();

                                    days.Add(getday);

                                    var checkday = DateTime.Now.ToString("ddd");

                                    var weekdaystring = "";

                                    if (days.Contains(checkday))
                                    {

                                        weekdaystring = "Today";


                                        // Find the Correct dosage for the nextTimeDue
                                        foreach (var p in item.TimeDosage)
                                        {
                                            var split = p.Split('|');
                                            if (DateTime.TryParse(split[0], out DateTime timePart))
                                            {
                                                if (timePart == nextTimeDue)
                                                {
                                                    if (split.Count() == 4)
                                                    {
                                                        item.NextDosage = split[1] + "|" + split[2];
                                                    }
                                                    else
                                                    {
                                                        item.NextDosage = split[1];
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        var today = DateTime.Now.ToString("ddd");

                                        // Predefined order of the week starting from Monday to Sunday
                                        var weekDaysOrder = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

                                        // Find today's index in the ordered list
                                        int todayIndex = weekDaysOrder.IndexOf(today);

                                        string nextDay = null;

                                        // Loop through the ordered days of the week, starting from the day after today
                                        for (int i = 1; i <= weekDaysOrder.Count; i++)
                                        {

                                            int nextDayIndex = (todayIndex + i) % weekDaysOrder.Count;
                                            string potentialNextDay = weekDaysOrder[nextDayIndex];

                                            // Check if the potential next day is in the 'days' list
                                            if (days.Contains(potentialNextDay))
                                            {
                                                weekdaystring = potentialNextDay;
                                                break;
                                            }
                                        }

                                        foreach (var p in item.TimeDosage)
                                        {
                                            var split = p.Split('|');

                                            if (split.Count() == 4)
                                            {
                                                item.NextDosage = split[1] + "|" + split[2];
                                            }
                                            else
                                            {
                                                item.NextDosage = split[1];
                                            }
                                            break;

                                        }
                                    }

                                    // Set the NextTime 
                                    if (item.NextDosage.Contains("|"))
                                    {
                                        var split = item.NextDosage.Split('|');
                                        //Unit Split 
                                        var unitsplit = item.unit.Split(' ');
                                        var Unituno = unitsplit[0] + " " + unitsplit[1];
                                        var unitdos = unitsplit[2];

                                        item.NextTime = weekdaystring + " " + nextTimeDue.ToString("HH:mm") + " - " + split[0] + " " + Unituno + " " + split[1] + " " + unitdos;
                                    }
                                    else
                                    {

                                        item.NextTime = weekdaystring + " " + nextTimeDue.ToString("HH:mm") + " - " + item.NextDosage + " " + item.unit;
                                    }


                                }


                            }

                            //days Interval
                            else if (freq[0] == "Days Interval")
                            {
                                var medfreqstring = "";

                                if (freq[1] == "2")
                                {
                                    medfreqstring = "Every other day";
                                }
                                else if (freq[1] == "3")
                                {
                                    medfreqstring = "Every 3 days";
                                }
                                else if (freq[1] == "7")
                                {
                                    medfreqstring = "Every 7 days";
                                }
                                else if (freq[1] == "14")
                                {
                                    medfreqstring = "Every 14 days";
                                }
                                else if (freq[1] == "21")
                                {
                                    medfreqstring = "Every 21 days";
                                }
                                else if (freq[1] == "28")
                                {
                                    medfreqstring = "Every 28 days";
                                }
                                else if (freq[1] == "30")
                                {
                                    medfreqstring = "Every 30 days";
                                }
                                else if (freq[1] == "35")
                                {
                                    medfreqstring = "Every 5 weeks";
                                }
                                else if (freq[1] == "42")
                                {
                                    medfreqstring = "Every 6 weeks";
                                }
                                else if (freq[1] == "49")
                                {
                                    medfreqstring = "Every 7 weeks";
                                }
                                else if (freq[1] == "56")
                                {
                                    medfreqstring = "Every 8 weeks";
                                }
                                else if (freq[1] == "63")
                                {
                                    medfreqstring = "Every 9 weeks";
                                }
                                else if (freq[1] == "70")
                                {
                                    medfreqstring = "Every 10 weeks";
                                }

                                item.MedFrequency = freq[0] + " - " + medfreqstring;

                                foreach (var x in item.TimeDosage)
                                {
                                    var sd = DateTime.Parse(item.startdate);
                                    var Interval = item.frequency.Split('|');
                                    int daysInterval = int.Parse(Interval[1]);
                                    DateTime currentDate = DateTime.Now;
                                    int daysSinceStart = (currentDate - sd).Days;

                                    // Find the next due date based on the interval
                                    int daysUntilNextDue = daysInterval - (daysSinceStart % daysInterval);
                                    DateTime nextDueDate = currentDate.AddDays(daysUntilNextDue);

                                    var firstTimeDosage = x.Split('|');
                                    if (firstTimeDosage.Length >= 2 && DateTime.TryParse(firstTimeDosage[0], out DateTime parsedTime))
                                    {

                                        DateTime nextDueDateTime = new DateTime(nextDueDate.Year, nextDueDate.Month, nextDueDate.Day, parsedTime.Hour, parsedTime.Minute, 0);

                                        if (nextDueDateTime < DateTime.Now)
                                        {
                                            nextDueDateTime = nextDueDateTime.AddDays(daysInterval);
                                        }

                                        item.NextTime = nextDueDateTime.ToString("ddd HH:mm") + " - " + item.NextDosage + " " + item.unit; // Next due day and time
                                        item.NextDosage = firstTimeDosage[1]; // Set the dosage for this time
                                    }

                                }
                            }
                        }

                    }
                    else
                    {
                        item.NextDosage = "N/A";
                        item.NextTime = "As Required";
                        item.MedFrequency = "As Required";
                    }
                }
            }

                foreach (var item in AllUserMedications)
                {
                    //if (item.NextTime == "As Required")
                    //{
                    //    //Put to Bottom of list Later
                    //    AddinAsRequired.Add(item);
                    //}
                    //else if (item.NextTime.Contains("Today"))
                    //{
                    //    var Check = item.NextTime.Split(' ');
                    //    if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                    //    {
                    //        item.DatetimeOrder = DateTime.Now.Date.Add(parsedTime.TimeOfDay);
                    //    }
                    //}
                    //else if (item.NextTime.Contains("Tomorrow"))
                    //{
                    //    var Check = item.NextTime.Split(' ');
                    //    if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                    //    {
                    //        item.DatetimeOrder = DateTime.Now.Date.AddDays(1).Add(parsedTime.TimeOfDay);
                    //    }
                    //}
                    //else
                    //{
                    //    var Check = item.NextTime.Split(' ');
                    //    if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                    //    {
                    //        var GetDay = ParseDay(Check[0]);

                    //        int daysUntilTarget = ((int)GetDay - (int)DateTime.Now.DayOfWeek + 7) % 7;
                    //        DateTime nextDayOccurrence = DateTime.Now.Date.AddDays(daysUntilTarget).Add(parsedTime.TimeOfDay);

                    //        item.DatetimeOrder = nextDayOccurrence;
                    //    }
                    //}
                }

            //var sortedList = AllUserMedications.Where(md => md.NextTime != "As Required").OrderBy(md => md.DatetimeOrder).ToList();
            // Add "As Required" items to the bottom of the list
           // sortedList.AddRange(AddinAsRequired);


            //order by name
            foreach(var item in AllUserMedications)
            {
                if (item.status == "Pending")
                {
                    item.ChangedMedName = item.medicationtitle;
                    item.ChangedMed = false;
                    item.pendborderBack = "#00FFFFFF";
                    item.pendimgSource = "warningicon.png";

                }
                else
                {

                    var splitdetails = item.details.Split('|');

                    if (splitdetails[0] != "--")
                    {
                        item.ChangedMedName = splitdetails[0];
                        item.ChangedMed = true;
                    }
                    else
                    {
                        item.ChangedMedName = item.medicationtitle;
                        item.ChangedMed = false;
                    }

                    if (splitdetails[1] != "--")
                    {
                        item.ChangedNotes = true;
                        item.ChangedMedNotes = splitdetails[1];
                    }
                    else
                    {
                        item.ChangedNotes = false;
                    }
                }

            }
            //check if any medications have been completed

            foreach (var item in AllUserMedications)
            {
                if (item.status == "Pending")
                {
                    item.PendingMeds = true;
                    item.ActiveMeds = false;
                    item.pendborderBack = "#00FFFFFF";
                    item.pendimgSource = "warningicon.png";
                    CurrentMedications.Add(item);
                    
                }
                else
                {
                    item.ActiveMeds = true;
                    item.PendingMeds = false;


                    if (!string.IsNullOrEmpty(item.enddate))
                    {
                        var ed = DateTime.Parse(item.enddate);


                        if (DateTime.Now.Date >= ed.Date)
                        {
                            CompletedMedications.Add(item);
                        }
                        else if (item.frequency.Contains("As Required"))
                        {
                            AsRequiredMedications.Add(item);
                        }
                        else
                        {
                            CurrentMedications.Add(item);
                        }
                    }
                    else if (item.frequency.Contains("As Required"))
                    {
                        AsRequiredMedications.Add(item);
                    }
                    else
                    {
                        CurrentMedications.Add(item);
                    }
                }

            }

            var PendingList = new ObservableCollection<usermedication>(CurrentMedications.Where(x => x.status == "Pending").OrderBy(x => x.ChangedMedName)); 

            var sortedbyname = CurrentMedications.Where(x=> x.status != "Pending")
    .OrderBy(x => x.status == "Pending" ? 0 : 1)  // Pending first
    .ThenBy(x => x.ChangedMedName)                 // Then by medication name
    .ToList();
            var sortedbyname2 = CompletedMedications.OrderBy(x => x.ChangedMedName).ToList();
            var sortedbyname3 = AsRequiredMedications.OrderBy(x => x.ChangedMedName).ToList();

            NovoConsentData();
            bool checkonenotEmpty = true; 
            //Active Medications List 
            if (PendingList.Count == 0 && sortedbyname.Count == 0 && sortedbyname2.Count == 0 && sortedbyname3.Count == 0)
            {
                nodatastack.IsVisible = true;
                datastack.IsVisible = false;
                checkonenotEmpty = false; 
                await Task.Delay(2000);
                //NovoConsent.Margin = new Thickness(20, 300, 20, 10);
                //await MopupService.Instance.PopAllAsync(false);
            }
            else if (sortedbyname.Count == 0)
            {
                if (checkonenotEmpty)
                {
                   // SegmentDetails.Text = "Medications you have currently scheduled";
                }
                if(PendingList.Count == 0)
                {
                    noActivemedlbl.IsVisible = true;
                }
                AllUserMedsList.IsVisible = false;
                await Task.Delay(2000);
                // NovoConsent.Margin = new Thickness(20, 300, 20, 10);
                //await MopupService.Instance.PopAllAsync(false);
            }
            else
            {
                noActivemedlbl.IsVisible = false;
                nodatastack.IsVisible = false;
                datastack.IsVisible = true;
                //Set Segmentlbl
                SegmentDetails.Text = "Medications you have currently scheduled";
                AllUserMedsList.IsVisible = true;
                AllUserMedsList.ItemsSource = AllUserMedications;
                //NovoConsent.Margin = new Thickness(20, 0, 20, 10);

                //await MopupService.Instance.PopAllAsync(false);
            }

            PendingMedsList.ItemsSource = PendingList;
            PendingMedications = PendingList;
            if (PendingList.Count > 0)
            {
                PendingMedsList.IsVisible = true; 
            }
            else
            {
                PendingMedsList.IsVisible = false;
            }
          
            AllUserMedsList.ItemsSource = sortedbyname;
            CompletedMedsList.ItemsSource = sortedbyname2;
            AsRequiredList.ItemsSource = sortedbyname3;

           


            //check and update any completed meds in the db
            CheckCompletedMedications();

            

            MedsLoading.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    DayOfWeek ParseDay(string day)
    {
        return day switch
        {
            "Mon" => DayOfWeek.Monday,
            "Tues" => DayOfWeek.Tuesday,
            "Wed" => DayOfWeek.Wednesday,
            "Thurs" => DayOfWeek.Thursday,
            "Thu" => DayOfWeek.Thursday,
            "Fri" => DayOfWeek.Friday,
            "Sat" => DayOfWeek.Saturday,
            "Sun" => DayOfWeek.Sunday,
            _ => throw new ArgumentException("Invalid day format")
        };
    }

    async void CheckCompletedMedications()
    {
        try
        {


            foreach(var item in CompletedMedications)
            {
                if(item.status != "Completed")
                {
                    item.status = "Completed";

                    aPICalls.UpdateCompletedMedication(item);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                usermedication NullInstance = new usermedication();
                //RevertSelectedPending();

                await Navigation.PushAsync(new AddMedication(AllUserMedications, NullInstance), false);
                AddBtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
		}
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void AllUserMedsList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                usermedication SelectedMed = e.DataItem as usermedication;

                //RevertSelectedPending();

                if (SelectedMed.status == "Pending")
                {
                    SelectedMed.EditMedSection = "Details";
                    await Navigation.PushAsync(new AddMedication(AllUserMedications, SelectedMed), false);
                }
                else
                {
                    await Navigation.PushAsync(new SingleMedication(AllUserMedications, SelectedMed), false);
                }
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            var index = e.NewIndex;

            //RevertSelectedPendingRevertSelectedPending();

            if (index == 0)
            {
             if(IsLoading == true)
                {
                    AllUserMedsList.IsVisible = true;
                    noActivemedlbl.IsVisible = false;
                    IsLoading = false;                  
                }
                else
                {
                    AsRequiredList.IsVisible = false;
                    CompletedMedsList.IsVisible = false;
                    noARmedlbl.IsVisible = false;
                    noCompletedmedlbl.IsVisible = false;
                    bool Check = CurrentMedications.All(x => x.status == "Pending");
                    //bool hasMultipleStatuses = CurrentMedications.Select(x => x.status).Distinct().Count() > 1;
                    if (!Check)
                    {
                        SegmentDetails.Text = "Medications you have currently scheduled";
                    }

                    if (CurrentMedications.Count == 0)
                    {
                        AllUserMedsList.IsVisible = false;
                        noActivemedlbl.IsVisible = true;
                    }
                    else
                    {
                      
                        AllUserMedsList.IsVisible = true;
                        noActivemedlbl.IsVisible = false;
                    }

                    if (PendingMedications.Count > 0)
                    {
                        PendingMedsList.IsVisible = true;
                    }
                    else
                    {
                        PendingMedsList.IsVisible = false;
                    }
                }
            }
            else if(index == 1)
            {
                PendingMedsList.IsVisible = false;
                noActivemedlbl.IsVisible = false;
                AllUserMedsList.IsVisible = false;
                SegmentDetails.Text = "Medications you take as you require them";
                if (AsRequiredMedications.Count == 0)
                {
                    noARmedlbl.IsVisible = true;
                }
                else
                {
                    
                    noARmedlbl.IsVisible = false;
                    AsRequiredList.IsVisible = true;

                }
               
                CompletedMedsList.IsVisible = false;
                noCompletedmedlbl.IsVisible = false;
            }
            else if(index == 2)
            {
                PendingMedsList.IsVisible = false;
                noActivemedlbl.IsVisible = false;
                AllUserMedsList.IsVisible = false;
                AsRequiredList.IsVisible = false;
                noARmedlbl.IsVisible = false;
                SegmentDetails.Text = "Medications you are no longer taking";
                if (CompletedMedications.Count == 0)
                {
                    noCompletedmedlbl.IsVisible = true;
                }
                else
                {
                    
                    noCompletedmedlbl.IsVisible = false;
                    CompletedMedsList.IsVisible = true;
                }
            }

        }
        catch(Exception ex)
        {
            //Leave Empty
        }
    }

    private async void AsRequiredList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            usermedication SelectedMed = e.DataItem as usermedication;

            if (SelectedMed.status == "Pending")
            {
                SelectedMed.EditMedSection = "Details";
                await Navigation.PushAsync(new AddMedication(AllUserMedications, SelectedMed), false);
            }
            else
            {
                await Navigation.PushAsync(new SingleMedication(AllUserMedications, SelectedMed), false);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("med") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void PendingMedsList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            usermedication SelectedMed = e.DataItem as usermedication;

            if (SelectedMed.status == "Pending")
            {
                SelectedMed.EditMedSection = "Details";
                await Navigation.PushAsync(new AddMedication(AllUserMedications, SelectedMed), false);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void DeletePendingMed(object sender, TappedEventArgs e)
    {
        try
        {
            bool Delete = await DisplayAlert("Delete Pending Medication", "Are you sure you want to delete this Medication? Once deleted it cannot be retrieved", "Delete", "Cancel");
            if (Delete == true)
            {

                if (e.Parameter is usermedication selectedMed)
                {
                    APICalls database = new APICalls();
                    selectedMed.deleted = true; 
                    await database.DeleteMedication(selectedMed);

                    //Upadte Pendinglist 
                    if (PendingMedications.Contains(selectedMed))
                    {
                        PendingMedications.Remove(selectedMed);
                    }

                    if (CurrentMedications.Contains(selectedMed))
                    {
                        CurrentMedications.Remove(selectedMed);
                    }

                    if (PendingMedications.Count > 0)
                    {
                        PendingMedsList.IsVisible = true;
                        PendingMedsList.ItemsSource = PendingMedications;
                    }
                    else
                    {
                        PendingMedsList.IsVisible = false; 
                    }

                    if (CurrentMedications.Count == 0 && PendingMedications.Count == 0 && CompletedMedications.Count == 0 && AsRequiredMedications.Count == 0 )
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;
                    }

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Medication Deleted") { });
                    await Task.Delay(1500);
                    await MopupService.Instance.PopAllAsync(false);


                }
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
           NotasyncMethod(Ex);
        }
    }


    //Press to Hold Pending Meds Delete

    //private async void RevertSelectedPending() 
    //{
    //    try
    //    {
    //        if (DeleteFrame.IsVisible == true)
    //        {
    //            DeleteFrame.IsVisible = false;
    //            foreach (var item in AllUserMedications)
    //            {
    //                if (item.status == "Pending")
    //                {
    //                    item.pendimgSource = "warningicon.png";
    //                    item.pendborderBack = "#00FFFFFF";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    //async void ImageButton_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //delete all selected items
    //        string Borrar = null;

    //        if (sender is Button btn)
    //        {
    //            Borrar = btn.CommandParameter?.ToString();
    //        }
    //        else if (sender is ImageButton imgBtn)
    //        {
    //            Borrar = imgBtn.CommandParameter?.ToString();
    //        }

    //        if (Borrar == "Delete")
    //        {
    //            var AllSelectedMeds = new ObservableCollection<usermedication>(AllUserMedications.Where(x => x.pendborderBack == "#e5f9f4"));
    //            foreach (var item in AllSelectedMeds)
    //            {
    //                AllUserMedications.Remove(item);
    //                CurrentMedications.Remove(item);
    //                item.deleted = true;               
    //            }
    //            APICalls database = new APICalls();
    //            await database.DeletePendingMedications(AllSelectedMeds);

    //            //Update Page 
    //            await MopupService.Instance.PushAsync(new PopupPageHelper("Medication Deleted") { });
    //            await Task.Delay(1500);
    //            await MopupService.Instance.PopAllAsync(false);

    //            //Update items 

    //            var sortedbyname = CurrentMedications.OrderBy(x => x.status == "Pending" ? 0 : 1).ThenBy(x => x.ChangedMedName).ToList();

    //            NovoConsentData();

    //            //Active Medications List 
    //            if (sortedbyname.Count == 0)
    //            {
    //                nodatastack.IsVisible = true;
    //                datastack.IsVisible = false;
    //                await Task.Delay(2000);
    //            }
    //            else
    //            {
    //                noActivemedlbl.IsVisible = false;
    //                nodatastack.IsVisible = false;
    //                datastack.IsVisible = true;
    //                SegmentDetails.Text = "Medications you have currently scheduled";
    //                AllUserMedsList.IsVisible = true;
    //                AllUserMedsList.ItemsSource = sortedbyname;

    //            }         


    //        }
    //        else
    //        {
    //            //Cancel 
    //            foreach (var item in AllUserMedications)
    //            {
    //                if (item.status == "Pending")
    //                {
    //                    item.pendimgSource = "warningicon.png";
    //                    item.pendborderBack = "#00FFFFFF";
    //                }
    //            }
    //        }

    //        //DeleteFrame.IsVisible = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    //private void AllUserMedsList_ItemLongPress(object sender, Syncfusion.Maui.ListView.ItemLongPressEventArgs e)
    //{
    //    try
    //    {
    //        if (e == null) return;
    //        usermedication SelectedMed = e.DataItem as usermedication;

    //        var medication = AllUserMedications.FirstOrDefault(m => m.id == SelectedMed.id);
    //        if (medication != null && medication.PendingMeds == true)
    //        {
    //            // Toggle between two states
    //            if (medication.pendimgSource == "regcompletetick.png")
    //            {
    //                medication.pendimgSource = "warningicon.png";
    //                medication.pendborderBack = "#00FFFFFF";
    //            }
    //            else
    //            {
    //                medication.pendimgSource = "regcompletetick.png";
    //                medication.pendborderBack = "#e5f9f4";
    //            }
    //        }

    //        bool check = AllUserMedications.Any(x => x.pendborderBack != "#00FFFFFF" && x.PendingMeds == true);
    //        DeleteFrame.IsVisible = check;

    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}
}