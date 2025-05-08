using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Messaging;
using Maui.FreakyControls.Extensions;
using Mopups.Services;
using Newtonsoft.Json;
using Syncfusion.Maui.ListView;

namespace PeopleWith;

public partial class ActivitySchedule : ContentPage
{

    public ObservableCollection<userdailyactivity> AllUserActivity = new ObservableCollection<userdailyactivity>();
    public ObservableCollection<dailyactivity> AllActivities = new ObservableCollection<dailyactivity>();
    public ObservableCollection<userdailyactivity> ScheduleItems = new ObservableCollection<userdailyactivity>();
    public ObservableCollection<UserActivityGroup> ActivityGroup = new ObservableCollection<UserActivityGroup>();
    public bool Selecteddatechange = false;
    public userdailyactivity SelectedActivity = new userdailyactivity();
    List<DateTime> dateList = new List<DateTime>();
    public DateTime dateforschedule = new DateTime();
    public APICalls database = new APICalls();
    public List<Schedule> changeddatesforlistview = new List<Schedule>();
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

    public ActivitySchedule()
    {
        try
        {
            InitializeComponent();
            GetActivityInfo();
            NovoConsentData();
        }
        catch (Exception Ex)
        {
            ActivityLoading.IsVisible = false;
            NotasyncMethod(Ex);
        }
    }

    public ActivitySchedule(ObservableCollection<userdailyactivity> UpdatedActivities)
    {
        try
        {
            InitializeComponent();
            //Update Listview on Add of New Activity 
            AllUserActivity = UpdatedActivities;
            GetActivityInfo();
            NovoConsentData();
        }
        catch (Exception Ex)
        {
            ActivityLoading.IsVisible = false;
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


    public class UserActivityGroup
    {
        public DateTime Date { get; set; }
        public ObservableCollection<userdailyactivity> Activities { get; set; }

        public UserActivityGroup(DateTime date, IEnumerable<userdailyactivity> activities)
        {
            Date = date;
            Activities = new ObservableCollection<userdailyactivity>(
        activities.OrderBy(a => DateTime.Parse(a.startdate)) // Ensure sorting by Startdate
        );
        }
    }


    private async void Preloaddata()
    {
        try
        {

            TodayDaylbl.Text = DateTime.Now.ToString("dd"); 
            // Get today's date
            DateTime today = DateTime.Today;

            for (int i = -30; i <= 3; i++)
            {
                dateList.Add(today.AddDays(i));
            }

            foreach (var item in dateList)
            {
                var newitem = new Schedule();

                newitem.Day = item.Day.ToString();
                newitem.Date = item.Date.ToString("ddd");

                if (item.Date > DateTime.Now.Date)
                {
                    newitem.Bgcolour = Colors.Transparent;
                    newitem.Bordercolour = Color.FromArgb("#fce9d9");
                    newitem.Op = 0.5;
                }
                else
                {
                    newitem.Bgcolour = Color.FromArgb("#fce9d9");
                    newitem.Bordercolour = Colors.Transparent;
                    newitem.Op = 1;
                }

                changeddatesforlistview.Add(newitem);
            }

            ActivityDates.ItemsSource = changeddatesforlistview;

            // Find today's date in the list

            var indexForToday = dateList.IndexOf(today);

            // Check if today's date is in the list
            if (indexForToday >= 0)
            {
                Selecteddatechange = true;
                // Set the selected item to today's date
                ActivityDates.SelectedItem = changeddatesforlistview[indexForToday];

                var dateforlabel = dateList[indexForToday];

                datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");
                dateforschedule = dateList[indexForToday];

                // Scroll to today's date and try to center it
                ActivityDates.ScrollTo(changeddatesforlistview[indexForToday], ScrollToPosition.Center, true);
            }

            //var orderbytime = ScheduleList.OrderBy(x => TimeSpan.Parse(x.time)).ToList();
            //Old Update Listview 
            //WeakReferenceMessenger.Default.Register<AddNewActivity>(this, (r, m) =>
            //{
            //    AllUserActivity.Add((userdailyactivity)m.Value);
            //    WorkoutItemsDue();
            //});
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void GetActivityInfo()
    {
        try
        {

            ActivityLoading.IsVisible = true;
            //Get User Activity Here
            APICalls database = new APICalls();

            //Get Investigation Details 
            var getActivityTask = database.GetActivityDetails();
            AllActivities = await getActivityTask;

            //Get All User Activity
            var GetActivityTask = database.GetUserActivityAsync();
            AllUserActivity = await GetActivityTask;

            Preloaddata();
            editActivtyData();
            WorkoutItemsDue();

            ActivityLoading.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void editActivtyData()
    {
        try
        {

            //Add Activity Title && Image Source to Dictionary (Issue With Duplicate Water Polo)
            var ActivityLookup = AllActivities
    .DistinctBy(x => x.activitytitle)
    .ToDictionary(x => x.activitytitle, x => x.Source);

            foreach (var item in AllUserActivity)
            {
                string dur = item.ActivityFeedbackList.Duration.Replace(":", "h") + "m";

                // Split into hours and minutes
                string[] parts = dur.Split('h');
                string hours = parts[0];
                string minutes = parts.Length > 1 ? parts[1].Replace("m", "") : "0";

                // Convert to integers for proper formatting
                int h = int.Parse(hours);
                int m = int.Parse(minutes);

                // Construct final duration string
                string formattedDuration = (h > 0 ? $"{h}h" : "") + (m > 0 ? $"{m}m" : "");

                // Default zero if empty
                if (string.IsNullOrEmpty(formattedDuration))
                {
                    formattedDuration = "0m";
                }

                //Shorten activity title 

                if (item.activitytitle.Length > 25)
                {
                    var SubString = item.activitytitle.Substring(0, 25) + "...";
                    item.shorttitle = SubString;
                }
                else
                {
                    item.shorttitle = item.activitytitle;
                }

                // Assign the formatted duration
                dur = formattedDuration;

                item.convertedduration = dur;
                if (!String.IsNullOrEmpty(item.ActivityFeedbackList.Mood))
                {
                    item.moodimg = item.ActivityFeedbackList.Mood.ToLower() + ".png";
                }
                item.Date = DateTime.Parse(item.startdate).Date;

                ActivityLookup.TryGetValue(item.activitytitle, out var source);
                item.Typeimg = source;

                var splitstart = item.startdate.Split(" ");

                item.Time = splitstart[1];
            }

            TimelineListview.ItemsSource = AllUserActivity.OrderByDescending(x => DateTime.Parse(x.startdate));

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void WorkoutItemsDue()
    {
        try
        {

            // Clear the existing schedule items
            ScheduleItems.Clear();

            // Use dateforschedule as the selected date for populating the schedule
            DateTime selectedDate = dateforschedule;
            String SelectedDateString = selectedDate.ToString("dd/MM/yy");

            var Filter = new ObservableCollection<userdailyactivity>(AllUserActivity.Where(s => s.startdate.Contains(SelectedDateString, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.startdate).Select(item =>
            {
                return item;
            });


            foreach (var item in Filter)
            {
                ScheduleItems.Add(item);
            }

            if (ScheduleItems.Count > 0)
            {
                ActivityPlannerStack.IsVisible = true;
                EmptyStack.IsVisible = false;
                ActivityPlanner.ItemsSource = ScheduleItems;
                ActivityPlanner.HeightRequest = ScheduleItems.Count * 50 + 100;
                AddTaskStack.IsVisible = false;
            }
            else
            {
                ActivityPlannerStack.IsVisible = false;
                EmptyStack.IsVisible = true;
                AddTaskStack.IsVisible = false;
            }

            PlannerHeader.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private void UpdateDates(string DateSelected)
    {
        try
        {
            // Ensure input date is valid
            if (!DateTime.TryParse(DateSelected, out DateTime selectedDate))
            {
                Console.WriteLine("Invalid date format");
                return;
            }

            dateList.Clear();
            changeddatesforlistview.Clear();

            // Populate the date list from -30 to +30 days relative to selectedDate
            for (int i = -30; i <= 30; i++)
            {
                dateList.Add(selectedDate.AddDays(i));
            }

            foreach (var item in dateList)
            {
                var newitem = new Schedule
                {
                    Day = item.Day.ToString(),
                    Date = item.ToString("ddd"),
                    Bgcolour = (item.Date > DateTime.Now.Date) ? Colors.Transparent : Color.FromArgb("#fce9d9"),
                    Bordercolour = (item.Date > DateTime.Now.Date) ? Color.FromArgb("#fce9d9") : Colors.Transparent,
                    Op = (item.Date > DateTime.Now.Date) ? 0.5 : 1
                };

                changeddatesforlistview.Add(newitem);
            }

            // Update UI
            ActivityDates.ItemsSource = null; // Reset to force refresh
            ActivityDates.ItemsSource = changeddatesforlistview;

            // Find index of selected date
            var indexForSelected = dateList.IndexOf(selectedDate);

            if (indexForSelected >= 0)
            {
                Selecteddatechange = true;
                ActivityDates.SelectedItem = changeddatesforlistview[indexForSelected];

                datelbl.Text = selectedDate.ToString("dddd, dd MMMM");
                dateforschedule = selectedDate;

                // Scroll to selected date
                ActivityDates.ScrollTo(changeddatesforlistview[indexForSelected], ScrollToPosition.Center, true);
            }

            WorkoutItemsDue();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        if (e.Direction == SwipeDirection.Left)
        {
            dateforschedule.AddDays(1);
        }
        else if (e.Direction == SwipeDirection.Right)
        {
            dateforschedule.AddDays(-1);
        }
        WorkoutItemsDue();
    }


    private async void AddBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            AddBtn.IsEnabled = false;
            await Navigation.PushAsync(new AddActivity(AllUserActivity));
            AddBtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("activity") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ActivityDates_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as Schedule;

            var itemsSource = (sender as SfListView).ItemsSource as IList<Schedule>;
            int index = itemsSource.IndexOf(item);

            var dateforlabel = dateList[index];

            dateforschedule = dateList[index];

            datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");

            //if(DateTime.Now.ToString("dd/MM/yy") != dateforschedule.ToString("dd/MM/yy"))
            //{
            //    TodayDay.IsVisible = true; 
            //}
            //else
            //{
            //    TodayDay.IsVisible = false; 
            //}

            WorkoutItemsDue();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            // Create TaskCompletionSource to get the result
            var tcs = new TaskCompletionSource<string>();

            // Open the popup and pass TaskCompletionSource
            await MopupService.Instance.PushAsync(new ActivityCalendar(tcs));

            // Wait for the result
            string selectedDate = await tcs.Task;

            if (!string.IsNullOrEmpty(selectedDate))
            {
                //Update Dates
                UpdateDates(selectedDate);

                //if(DateTime.Parse(selectedDate).Date == DateTime.Now.Date)
                //{
                //    TodayDay.IsVisible = false;
                //}
                //else
                //{
                //    TodayDay.IsVisible = true;
                //}
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    async private void Listviewbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            TimelineLoading.IsVisible = true;
            PlannerStack.IsVisible = false;

            if(EmptyStack.IsVisible == true)
            {
                EmptyStack.IsVisible = false; 
            }
            await Task.Delay(1000);

            TimelineStack.IsVisible = true;

            if (AllUserActivity.Count == 0)
            {
                TimelineStack.IsVisible = false;
                PlannerStack.IsVisible = true;
                TimelineLoading.IsVisible = false;
                EmptyStack.IsVisible = true;
                await DisplayAlert("Activity Timeline", "No activities found. Please add some to access this feature", "Close");
            }
            else
            {
                TimelineStack.IsVisible = true;
                EmptyStack.IsVisible = false;
            }

            TimelineLoading.IsVisible = false;
        }
        catch (Exception Ex)
        {
            TimelineLoading.IsVisible = false;
            NotasyncMethod(Ex);
        }
    }

    private void todaybtn_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
            //Stops Repeat Clicking when Already Showing Today's date
            if (dateforschedule.Date != DateTime.Now.Date)
            {
                string selectedDate = DateTime.Now.ToString("dd/MM/yyyy");
                UpdateDates(selectedDate);
                //TodayDay.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void timelinebtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            PlannerStack.IsVisible = true;
            TimelineStack.IsVisible = false;
           

            //Set Schedule items 
            if (ScheduleItems.Count > 0)
            {
                ActivityPlannerStack.IsVisible = true;
                EmptyStack.IsVisible = false;

            }
            else
            {
                ActivityPlannerStack.IsVisible = false;
                EmptyStack.IsVisible = true;
                AddTaskStack.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TimelineListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var selectedItem = e.DataItem as userdailyactivity;
            if(selectedItem != null)
            {
                //Push to SingleActivity 
                await Navigation.PushAsync(new SingleActivity(selectedItem, AllUserActivity), false);
            }         
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async void ActivityPlanner_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var selectedItem = e.DataItem as userdailyactivity;
            //Push to SingleActivity 
            if (selectedItem != null)
            {
                //Push to SingleActivity 
                await Navigation.PushAsync(new SingleActivity(selectedItem, AllUserActivity), false);
            }        
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private async void BottomSheetButtonTapped(object sender, TappedEventArgs e)
    //{
    //    try
    //    {
    //        var param = ((TappedEventArgs)e).Parameter;
    //        if (param == "Delete")
    //        {
    //            return;
    //            bool Delete = await DisplayAlert("Delete Activity", "Are you sure you would like the delete this Activity? Once deleted it cannot be retrieved", "Continue", "Cancel");
    //            if (Delete == true)
    //            {
    //                //Delete Item 
    //                SelectedActivity.deleted = true;
    //                APICalls database = new APICalls();
    //                await database.DeleteUserActivity(SelectedActivity);

    //                await MopupService.Instance.PushAsync(new PopupPageHelper("Actvity Deleted") { });
    //                await Task.Delay(4000);

    //                await MopupService.Instance.PopAllAsync(false);
    //                EditDelete.IsOpen = false;

    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }
    //        else if (param == "Edit")
    //        {
    //            string AddEdit = "Edit";
    //            await Navigation.PushAsync(new AddActivity(AllUserActivity,SelectedActivity, AddEdit));
    //        }
    //        else if (param == "Duplicate")
    //        {
    //            string AddEdit = "Add";
    //            await Navigation.PushAsync(new AddActivity(AllUserActivity, SelectedActivity, AddEdit));
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}
}