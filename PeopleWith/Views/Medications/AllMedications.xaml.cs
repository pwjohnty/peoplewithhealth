using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllMedications : ContentPage
{
	APICalls aPICalls = new APICalls();
    public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<usermedication> AddinAsRequired = new ObservableCollection<usermedication>();
    public AllMedications()
	{
		InitializeComponent();
		getusermedications();

	}

    public AllMedications(ObservableCollection<usermedication> AllUsermeds)
    {
        InitializeComponent();

		//AllUserMedications.Clear();

        AllUserMedications = AllUsermeds;

        if(AllUserMedications.Count == 0)
        {
            nodatastack.IsVisible = true;
            datastack.IsVisible = false; 
        }
        else
        {
            nodatastack.IsVisible = false;
            datastack.IsVisible = true;

            AllUserMedsList.ItemsSource = AllUserMedications;

            WorkOutNextDue();
        }

      

    }


    async void getusermedications()
	{
		try
		{
            var getMedicationsTask = aPICalls.GetUserMedicationsAsync();

            var delayTask = Task.Delay(1000);

            if (await Task.WhenAny(getMedicationsTask, delayTask) == delayTask)
            {
                await MopupService.Instance.PushAsync(new GettingReady("Loading Medications") { });
            }

            AllUserMedications = await getMedicationsTask;

            if(AllUserMedications.Count == 0)
            {
                nodatastack.IsVisible = true;
                datastack.IsVisible = false;
                await MopupService.Instance.PopAllAsync(false);
            }
            else
            {
                AllUserMedsList.ItemsSource = AllUserMedications;

                await MopupService.Instance.PopAllAsync(false);

                WorkOutNextDue();
            }
        }
		catch(Exception ex)
		{

		}
	}

    async void WorkOutNextDue()
    {
        try
        {
            //Change if taken/

            foreach (var item in AllUserMedications)
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

                        DateTime currentTime = DateTime.Now;
                        List<DateTime> times = new List<DateTime>();
                        var freq = item.frequency.Split('|');
                        //Daily 
                        if (freq[0] == "Daily")
                        {
                            foreach (var x in item.TimeDosage)
                            {
                                var parts = x.Split('|');
                                if (parts.Length > 0)
                                {
                                    if (DateTime.TryParseExact(parts[0], "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                                    {
                                        times.Add(parsedTime);
                                    }
                                }
                            }

                            // Find the next time that is greater than the current time
                            var nextTimeDue = times.Where(t => t > currentTime).OrderBy(t => t).FirstOrDefault();

                            if (nextTimeDue == default(DateTime))
                            {
                                times = times.OrderBy(t => t).ToList();
                                var firstTimeForTomorrow = times.First();

                                item.NextTime = "Tomorrow " + firstTimeForTomorrow.ToString("HH:mm");

                                // Find the corresponding dosage for the first time
                                foreach (var p in item.TimeDosage)
                                {
                                    var split = p.Split('|');
                                    if (split.Length == 2 && DateTime.TryParseExact(split[0], "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime timePart))
                                    {
                                        if (timePart == firstTimeForTomorrow)
                                        {
                                            item.NextDosage = split[1];
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Set the NextTime 
                                item.NextTime = "Today " + nextTimeDue.ToString("HH:mm");

                                // Find the Correct dosage for the nextTimeDue
                                foreach (var p in item.TimeDosage)
                                {
                                    var split = p.Split('|');
                                    if (split.Length == 2 && DateTime.TryParseExact(split[0], "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime timePart))
                                    {
                                        if (timePart == nextTimeDue)
                                        {
                                            item.NextDosage = split[1];
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //Weekly 
                        else if (freq[0] == "Weekly" || freq[0] == "Weekly ")
                        {
                            foreach (var x in item.TimeDosage)
                            {
                                var parts = x.Split('|');
                                if (parts.Length == 3)
                                {
                                    var targetDay = ParseDay(parts[2]);
                                    if (DateTime.TryParseExact(parts[0], "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                                    {
                                        // Calculate the start date and time
                                        var startDate = DateTime.Parse(item.startdate);
                                        var startDateAndTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, parsedTime.Hour, parsedTime.Minute, 0);

                                        int daysUntilTarget = ((int)targetDay - (int)DateTime.Now.DayOfWeek + 7) % 7;
                                        DateTime nextOccurrence = DateTime.Now.AddDays(daysUntilTarget).Date.Add(parsedTime.TimeOfDay);

                                        // If the time for today has already passed, move to the next week
                                        if (nextOccurrence < DateTime.Now)
                                        {
                                            nextOccurrence = nextOccurrence.AddDays(7);
                                        }

                                        // Set the next time and dosage for the item
                                        item.NextTime = nextOccurrence.ToString("ddd HH:mm");
                                        item.NextDosage = parts[1]; // Set the dosage for this time

                                        // Break after setting the first matching time
                                        break;
                                    }
                                }
                            }
                        }

                        //days Interval
                        else if (freq[0] == "Days Interval")
                        {
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
                                if (firstTimeDosage.Length >= 2 && DateTime.TryParseExact(firstTimeDosage[0], "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                                {

                                    DateTime nextDueDateTime = new DateTime(nextDueDate.Year, nextDueDate.Month, nextDueDate.Day, parsedTime.Hour, parsedTime.Minute, 0);

                                    if (nextDueDateTime < DateTime.Now)
                                    {
                                        nextDueDateTime = nextDueDateTime.AddDays(daysInterval);
                                    }

                                    item.NextTime = nextDueDateTime.ToString("ddd HH:mm"); // Next due day and time
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
                }
            }

                foreach (var item in AllUserMedications)
                {
                    if (item.NextTime == "As Required")
                    {
                        //Put to Bottom of list Later
                        AddinAsRequired.Add(item);
                    }
                    else if (item.NextTime.Contains("Today"))
                    {
                        var Check = item.NextTime.Split(' ');
                        if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                        {
                            item.DatetimeOrder = DateTime.Now.Date.Add(parsedTime.TimeOfDay);
                        }
                    }
                    else if (item.NextTime.Contains("Tomorrow"))
                    {
                        var Check = item.NextTime.Split(' ');
                        if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                        {
                            item.DatetimeOrder = DateTime.Now.Date.AddDays(1).Add(parsedTime.TimeOfDay);
                        }
                    }
                    else
                    {
                        var Check = item.NextTime.Split(' ');
                        if (Check.Length == 2 && DateTime.TryParse(Check[1], out DateTime parsedTime))
                        {
                            var GetDay = ParseDay(Check[0]);

                            int daysUntilTarget = ((int)GetDay - (int)DateTime.Now.DayOfWeek + 7) % 7;
                            DateTime nextDayOccurrence = DateTime.Now.Date.AddDays(daysUntilTarget).Add(parsedTime.TimeOfDay);

                            item.DatetimeOrder = nextDayOccurrence;
                        }
                    }
                }

            var sortedList = AllUserMedications.Where(md => md.NextTime != "As Required").OrderBy(md => md.DatetimeOrder).ToList();
            // Add "As Required" items to the bottom of the list
            sortedList.AddRange(AddinAsRequired);
            AllUserMedsList.ItemsSource = sortedList;
        }
        catch (Exception ex)
        {

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

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
            usermedication NullInstance = new usermedication(); 
			await Navigation.PushAsync(new AddMedication(AllUserMedications, NullInstance), false);
		}
		catch(Exception ex)
		{

		}
    }

    async private void AllUserMedsList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            usermedication SelectedMed = e.DataItem as usermedication; 
            await Navigation.PushAsync(new SingleMedication(AllUserMedications, SelectedMed), false);
        }
        catch (Exception Ex)
        {

        }
    }
}