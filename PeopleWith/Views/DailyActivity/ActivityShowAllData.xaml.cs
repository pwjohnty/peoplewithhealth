//using CommunityToolkit.Maui.Core.Extensions;
//using CommunityToolkit.Mvvm.Messaging;
//using Maui.FreakyControls.Extensions;
//using Syncfusion.Maui.DataSource.Extensions;
//using System.Collections.ObjectModel;
//using System.Globalization;
//using System.Threading.Tasks;
//using System.Windows.Input;

//namespace PeopleWith;

//public partial class ActivityShowAllData : ContentPage
//{

//	public ObservableCollection<RecordSelect> Records = new ObservableCollection<RecordSelect>();
//    public userdailyactivity SelectedActivity = new userdailyactivity();
//    public ObservableCollection<ActivityFeedback> AllRecords = new ObservableCollection<ActivityFeedback>();
//    public ObservableCollection<ActivityFeedback> AllRecordedList = new ObservableCollection<ActivityFeedback>();
//    public ObservableCollection<ActivityFeedback> AllNotRecordedList = new ObservableCollection<ActivityFeedback>();
//    public ObservableCollection<ActivityFeedback> FilteredRecords = new ObservableCollection<ActivityFeedback>();
//    public ObservableCollection<Timeline> TimelineItems { get; set; } = new ObservableCollection<Timeline>();
//    public DateTime Startdate = new DateTime();
//    public DateTime? EndDate = null;
//    public DateTime currentDateTime = DateTime.Now;
//    public int daystoadd = 0;
//    public int monthsToAdd = 0;
//    public int yearsToAdd = 0;

//    public event EventHandler<bool> ConnectivityChanged;
//    //Crash Handler
//    CrashDetected crashHandler = new CrashDetected();
//    async public void NotasyncMethod(Exception Ex)
//    {
//        try
//        {
//            await crashHandler.SentryCrashDetected(Ex);
//            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
//        }
//        catch (Exception ex)
//        {
//            //Dunno 
//        }
//    }
    

//    public ActivityShowAllData(userdailyactivity ActivitySelected)
//	{
//		InitializeComponent();

//        SelectedActivity = ActivitySelected; 

//        Titlelbl.Text = SelectedActivity.activitytitle;

//        SelectedActivity.frequency = SelectedActivity.activityfrequencylist.FirstOrDefault()?.frequency;

//        var one = new RecordSelect();
//        one.title = "All";
//        one.dot = Colors.LightBlue;
//		Records.Add(one);

//        var two = new RecordSelect();
//        two.title = "Recorded";
//        two.dot = Colors.LightGreen;
//        Records.Add(two);

//        var three = new RecordSelect();
//        three.title = "Not Recorded";
//        three.dot = Colors.LightGray;
//        Records.Add(three);

//        //Get All Records
//        ItemstoShow();
//        //Populate Timeline 
//        CalculateTimeLine();

//        RecordsListview.ItemsSource = Records;
//        ActivityRecords.ItemsSource = AllRecords.OrderByDescending(x=>DateTime.Parse(x.Scheduledatetime));
//    }
//    public class RecordSelect
//    {
//        public string title { get; set; }
//        public Color dot { get; set; }
//    }

//    public class Timeline
//    {
//        public string DisplayText { get; set; }
//        public string DateRange { get; set; }
//        public DateTime Order { get; set; }
//        public string Type { get; set; }
//    }

//    private async void ItemstoShow()
//    {
//        try
//        {
//            Startdate = DateTime.Parse(SelectedActivity.startdate);
//            if (!String.IsNullOrEmpty(SelectedActivity.enddate))
//            {
//                EndDate = DateTime.Parse(SelectedActivity.enddate);
//            }

//            if (SelectedActivity.frequency == "One Off")
//            {
//                // Do Nothing
//            }
//            else if (SelectedActivity.frequency == "Daily")
//            {
//                daystoadd = 1;
//            }
//            else if (SelectedActivity.frequency == "Weekly")
//            {
//                daystoadd = 7;
//            }
//            else if (SelectedActivity.frequency == "Monthly")
//            {
//                monthsToAdd = 1;
//            }
//            else if (SelectedActivity.frequency == "Quarterly")
//            {
//                monthsToAdd = 3;
//            }
//            else if (SelectedActivity.frequency == "Yearly")
//            {
//                yearsToAdd = 1;
//            }


//            for (DateTime date = Startdate.Date; date <= currentDateTime.Date;)
//            {
//                foreach (var item in SelectedActivity.activityfrequencylist)
//                {
//                    if (!TimeSpan.TryParse(item.time, out TimeSpan itemTime))
//                    {
//                        continue; 
//                    }

//                    DateTime scheduleDateTime = date.Add(itemTime);

//                    // If EndDate is set and scheduleDateTime exceeds it, skip
//                    if (EndDate.HasValue && scheduleDateTime > EndDate.Value)
//                    {
//                        continue;
//                    }

//                    // If it's today but time is still in the future, skip
//                    if (EndDate is not null && scheduleDateTime > EndDate.Value)
//                    {
//                        continue;
//                    }

//                    var newItem = new ActivityFeedback
//                    {
//                        id = item.id,
//                        Scheduledatetime = scheduleDateTime.ToString("HH:mm, dd/MM/yy")
//                    };

//                    var getfeedbackitem = SelectedActivity.activityfeedbacklist
//                        .Where(x => x.id == newItem.id)
//                        .Where(p => DateTime.Parse(p.Scheduledatetime) == scheduleDateTime)
//                        .FirstOrDefault();

//                    if (getfeedbackitem != null)
//                    {
//                        // Recorded Item
//                        newItem.Recorded = "Recorded";
//                        newItem.datetimerecorded = getfeedbackitem.datetimerecorded;
//                        newItem.SetColour = Colors.LightGreen;
//                        AllRecordedList.Add(newItem); 
//                    }
//                    else
//                    {
//                        // Not Recorded Item
//                        newItem.Recorded = "Not Recorded";
//                        newItem.datetimerecorded = "Not Recorded";
//                        newItem.SetColour = Colors.LightGray;
//                        AllNotRecordedList.Add(newItem);
//                    }

//                    AllRecords.Add(newItem);
//                }

//                // Update date for the next iteration
//                if (monthsToAdd > 0)
//                {
//                    date = date.AddMonths(monthsToAdd);
//                }
//                else if (yearsToAdd > 0)
//                {
//                    date = date.AddYears(yearsToAdd);
//                }
//                else
//                {
//                    date = date.AddDays(daystoadd);
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }


//    private void CalculateTimeLine()
//    {
//        try
//        {
//            TimelineItems.Clear();
//            if (AllRecords == null || AllRecords.Count == 0)
//                return;

//            DateTime maxDate = AllRecords.Max(x => DateTime.Parse(x.Scheduledatetime));
//            DateTime minDate = AllRecords.Min(x => DateTime.Parse(x.Scheduledatetime));
//            double totalDays = (maxDate.Date - minDate.Date).TotalDays;

//            string typeString;
//            FilterTimeLine.IsVisible = totalDays > 7;

//            if (totalDays <= 7)
//            {
//                typeString = "Day";
//                foreach (var item in AllRecords.OrderBy(d => DateTime.Parse(d.Scheduledatetime)))
//                {
//                    DateTime dateTime = DateTime.Parse(item.Scheduledatetime);
//                    TimelineItems.Add(new Timeline
//                    {
//                        DisplayText = dateTime.ToString("dd/MM/yy"),
//                        DateRange = dateTime.ToString("dd/MM/yy"),
//                        Order = dateTime,
//                        Type = "Day"
//                    });
//                }
//            }
//            else if (totalDays > 7 && totalDays < 30)
//            {
//                typeString = "Week";
//                var groupedByWeek = AllRecords
//                    .OrderBy(d => DateTime.Parse(d.Scheduledatetime))
//                    .GroupBy(d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
//                        DateTime.Parse(d.Scheduledatetime), CalendarWeekRule.FirstDay, DayOfWeek.Monday));

//                foreach (var group in groupedByWeek)
//                {
//                    DateTime firstDate = DateTime.Parse(group.First().Scheduledatetime);
//                    DateTime lastDate = DateTime.Parse(group.Last().Scheduledatetime);
//                    TimelineItems.Add(new Timeline
//                    {
//                        DisplayText = $"Week of {firstDate:dd MMM yy}",
//                        DateRange = $"{firstDate:dd/MM/yy} | {lastDate:dd/MM/yy}",
//                        Order = firstDate,
//                        Type = "Week"
//                    });
//                }
//            }
//            else
//            {
//                typeString = "Month";
//                var groupedByMonth = AllRecords
//                    .OrderBy(d => DateTime.Parse(d.Scheduledatetime))
//                    .GroupBy(d => new { DateTime.Parse(d.Scheduledatetime).Year, DateTime.Parse(d.Scheduledatetime).Month });

//                foreach (var group in groupedByMonth)
//                {
//                    DateTime firstDate = DateTime.Parse(group.First().Scheduledatetime);
//                    TimelineItems.Add(new Timeline
//                    {
//                        DisplayText = firstDate.ToString("MMMM yyyy"),
//                        DateRange = firstDate.ToString("MMMM yyyy"),
//                        Order = firstDate,
//                        Type = "Month"
//                    });
//                }
//            }

//            // Sort and update UI
//            FilterTimeLine.ItemsSource = TimelineItems.OrderByDescending(d => d.Order);
//            var firstTimelineItem = TimelineItems.OrderByDescending(d => d.Order).FirstOrDefault()?.Order;
//        }
//        catch (Exception ex)
//        {
//            NotasyncMethod(ex);
//        }
//    }



//    private async void RecordsListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
//    {
//        try
//        {
//            var Selecteditem = e.DataItem as RecordSelect;
//            var Item = Selecteditem.title;

//            ActivityRecords.ItemsSource = null;

//            if (Item == "All")
//            {
//                ActivityRecords.ItemsSource = AllRecords.OrderByDescending(x => DateTime.Parse(x.Scheduledatetime));
//            }
//            else if(Item == "Recorded")
//            {
//                ActivityRecords.ItemsSource = AllRecordedList.OrderByDescending(x => DateTime.Parse(x.Scheduledatetime));
//            }
//            else if(Item == "Not Recorded")
//            {
//                ActivityRecords.ItemsSource = AllNotRecordedList.OrderByDescending(x => DateTime.Parse(x.Scheduledatetime));
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private void FilterTimeLine_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
//    {

//    }
//}