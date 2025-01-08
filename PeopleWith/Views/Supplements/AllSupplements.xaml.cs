//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Linq;
//using static AndroidX.Core.Text.Util.LocalePreferences.FirstDayOfWeek;

namespace PeopleWith;

public partial class AllSupplements : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<usersupplement> AllUserMedications = new ObservableCollection<usersupplement>();
    public ObservableCollection<usersupplement> CompletedMedications = new ObservableCollection<usersupplement>();
    public ObservableCollection<usersupplement> CurrentMedications = new ObservableCollection<usersupplement>();
    public ObservableCollection<usersupplement> AsRequiredMedications = new ObservableCollection<usersupplement>();
    public ObservableCollection<usersupplement> AddinAsRequired = new ObservableCollection<usersupplement>();
    private bool IsLoading = true;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public AllSupplements()
    {
        try
        {
            InitializeComponent();
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

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AllSupplements(ObservableCollection<usersupplement> AllUsermeds)
    {
        try
        {
            InitializeComponent();

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


    async void getusermedications()
    {
        try
        {
            MedsLoading.IsVisible = true;
            var getMedicationsTask = aPICalls.GetUserSupplementsAsync();

            //var delayTask = Task.Delay(500);

            //if (await Task.WhenAny(getMedicationsTask, delayTask) == delayTask)
            //{
                //await MopupService.Instance.PushAsync(new GettingReady("Loading Supplements") { });
            //}

            AllUserMedications = await getMedicationsTask;

            WorkOutNextDue();
            MedsLoading.IsVisible = false;

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
                                    if (DateTime.TryParse(parts[0], out DateTime parsedTime))
                                    {
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

                                            if(split.Count() == 4)
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
            foreach (var item in AllUserMedications)
            {
                if (item.status == "Pending")
                {
                    item.ChangedMedName = item.supplementtitle;
                    item.ChangedMed = false;

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
                        item.ChangedMedName = item.supplementtitle;
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




            var sortedbyname = CurrentMedications
    .OrderBy(x => x.status == "Pending" ? 0 : 1)  // Pending first
    .ThenBy(x => x.ChangedMedName)                 // Then by medication name
    .ToList();
            var sortedbyname2 = CompletedMedications.OrderBy(x => x.ChangedMedName).ToList();
            var sortedbyname3 = AsRequiredMedications.OrderBy(x => x.ChangedMedName).ToList();


            //Active Medications List 
            if (sortedbyname.Count == 0 && sortedbyname2.Count == 0 && sortedbyname3.Count == 0)
            {
                nodatastack.IsVisible = true;
                datastack.IsVisible = false;
                await Task.Delay(2000);
                //await MopupService.Instance.PopAllAsync(false);
            }
            else if (sortedbyname.Count == 0)
            {
                noActivemedlbl.IsVisible = true;
                AllUserMedsList.IsVisible = false;
                await Task.Delay(2000);
                //await MopupService.Instance.PopAllAsync(false);
            }
            else
            {
                noActivemedlbl.IsVisible = false;
                nodatastack.IsVisible = false;
                datastack.IsVisible = true;
                AllUserMedsList.IsVisible = true;
                AllUserMedsList.ItemsSource = AllUserMedications;
                //Set Segmentlbl
                SegmentDetails.Text = "Supplements you have currently scheduled";

                //await MopupService.Instance.PopAllAsync(false);
            }

            AllUserMedsList.ItemsSource = sortedbyname;
            CompletedMedsList.ItemsSource = sortedbyname2;
            AsRequiredList.ItemsSource = sortedbyname3;


            //check and update any completed meds in the db
            CheckCompletedMedications();

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
            foreach (var item in CompletedMedications)
            {
                if (item.status != "Completed")
                {
                    item.status = "Completed";

                    aPICalls.UpdateCompletedSupplement(item);
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
                usersupplement NullInstance = new usersupplement();
                await Navigation.PushAsync(new AddSupplement(AllUserMedications, NullInstance), false);
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
            usersupplement SelectedMed = e.DataItem as usersupplement;

            if (SelectedMed.status == "Pending")
            {
                SelectedMed.EditMedSection = "Details";
                await Navigation.PushAsync(new AddSupplement(AllUserMedications, SelectedMed), false);
            }
            else
            {
                await Navigation.PushAsync(new SingleSupplement(AllUserMedications, SelectedMed), false);
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

            if (index == 0)
            {
                if (IsLoading == true)
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
                    if (CurrentMedications.Count == 0)
                    {
                        AllUserMedsList.IsVisible = false;
                        noActivemedlbl.IsVisible = true;
                    }
                    else
                    {
                        SegmentDetails.Text = "Supplements you have currently scheduled";
                        AllUserMedsList.IsVisible = true;
                        noActivemedlbl.IsVisible = false;
                    }
                }
            }
            else if (index == 1)
            {
                noActivemedlbl.IsVisible = false;
                AllUserMedsList.IsVisible = false;
                if (AsRequiredMedications.Count == 0)
                {
                    noARmedlbl.IsVisible = true;
                }
                else
                {
                    SegmentDetails.Text = "Supplements you take as you require them";
                    noARmedlbl.IsVisible = false;
                    AsRequiredList.IsVisible = true;

                }

                CompletedMedsList.IsVisible = false;
                noCompletedmedlbl.IsVisible = false;
            }
            else if (index == 2)
            {
                noActivemedlbl.IsVisible = false;
                AllUserMedsList.IsVisible = false;
                AsRequiredList.IsVisible = false;
                noARmedlbl.IsVisible = false;

                if (CompletedMedications.Count == 0)
                {
                    noCompletedmedlbl.IsVisible = true;
                }
                else
                {
                    SegmentDetails.Text = "Supplements you are no long taking";
                    noCompletedmedlbl.IsVisible = false;
                    CompletedMedsList.IsVisible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private async void AsRequiredList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            usersupplement SelectedMed = e.DataItem as usersupplement;

            if (SelectedMed.status == "Pending")
            {
                SelectedMed.EditMedSection = "Details";
                await Navigation.PushAsync(new AddSupplement(AllUserMedications, SelectedMed), false);
            }
            else
            {
                await Navigation.PushAsync(new SingleSupplement(AllUserMedications, SelectedMed), false);
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
            await MopupService.Instance.PushAsync(new Infopopup("supp") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}