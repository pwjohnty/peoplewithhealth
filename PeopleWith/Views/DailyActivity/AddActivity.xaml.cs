using Mopups.Pages;
using Mopups.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Maui.FreakyControls.Extensions;
using Syncfusion.Maui.ListView;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.Picker;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;


namespace PeopleWith;

public partial class AddActivity : ContentPage
{

    public ObservableCollection<dailyactivity> AllActivities = new ObservableCollection<dailyactivity>();
    public ObservableCollection<dailyactivity> FilterTabsList = new ObservableCollection<dailyactivity>();
    public ObservableCollection<dailyactivity> FilterItemsList = new ObservableCollection<dailyactivity>();
    public ObservableCollection<usersymptom> AllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<userdailyactivity> AllUserActivities = new ObservableCollection<userdailyactivity>();
    public ActivityFeedback FeedbacktoAdd = new ActivityFeedback();
    public List<string> Listoftimes = new List<string>();
    public List<string> activequality = new List<string>();
    public string AddorEdit = string.Empty; 
    userdailyactivity NewUserActivity = new userdailyactivity();
    userdailyactivity ExistingActivity = new userdailyactivity();
    public List<string> HowditgoList = new List<string>(); 
    public ObservableCollection<moodlist> AllMoods { get; set; }
    public moodlist GetMoodlist { get; set; }
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

    //Add of New Activity
    public AddActivity(ObservableCollection<userdailyactivity> UserAllActivities)
    {
        InitializeComponent();
        AllUserActivities = UserAllActivities;
        GetActivityInfo();
    }

    //Edit/Duplicate Process 
    public AddActivity(ObservableCollection<userdailyactivity> UserAllActivities, userdailyactivity PassedActivity, String EditAdd)
    {
        InitializeComponent();
        AllUserActivities = UserAllActivities;
        ExistingActivity = PassedActivity; 
        AddorEdit = EditAdd;
        GetActivityInfo();
    }

    async private void GetActivityInfo()
    {
        try
        {
            if (String.IsNullOrEmpty(AddorEdit))
            {
                Loadinglbl.Text = "Loading Add Activity...";
            }
            else if (AddorEdit == "Duplicate")
            {
                Loadinglbl.Text = "Loading Duplicate Activity...";
            }
            else if (AddorEdit == "Edit")
            {
                Loadinglbl.Text = "Loading Edit Activity...";
            }
            else if (AddorEdit == "Feedback")
            {
                Loadinglbl.Text = "Loading Activity Feedback...";
            }

            ActivityLoading.IsVisible = true;
            //Activityloading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get Investigation Details 
            var getActivityTask = database.GetActivityDetails();
            AllActivities = await getActivityTask;

            //Get symptom Data
            var getSymptomsTask = database.GetUserSymptomAsync();
            AllUserSymptoms = await getSymptomsTask;

            AllUserSymptoms = AllUserSymptoms.Where(x => x.deleted == false).OrderBy(c => c.symptomtitle).ToObservable();

            //symptomlistview.ItemsSource = AllUserSymptoms;

            //Add Classiciation Filters 
            var Filter = AllActivities
                .GroupBy(s => s.ShortGroup)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.ShortGroup);

            FilterTabsList = new ObservableCollection<dailyactivity>(Filter);
            FilterTabsListview.ItemsSource = FilterTabsList;

            //Get Mood List
            GetMoodlist = new moodlist();
            AllMoods = GetMoodlist.GetList();
            MoodListview.ItemsSource = AllMoods;
            //Activityloading.IsVisible = false;

            //Populate DateTimeEntry
            var TimeNow = DateTime.Now.ToString("HH:mm");
            Datelbl.Text = "Today";
            Timelbl.Text = TimeNow;

            activequality.Add("Excellent");
            activequality.Add("Good");
            activequality.Add("Mediocre");
            activequality.Add("Poor");
            Activityquality.ItemsSource = activequality;

            Listoftimes.Add("+15m");
            Listoftimes.Add("+30m");
            Listoftimes.Add("+45m");
            Listoftimes.Add("+60m");
            Listoftimes.Add("+75m");
            Listoftimes.Add("+90m");
            Incrementlist.ItemsSource = Listoftimes;

        if (!string.IsNullOrEmpty(AddorEdit))
            {
                PreSelectItems();
            }

            ActivityLoading.IsVisible = false;
            MainDataStack.IsVisible = true;
        }

        catch (Exception Ex)
        {
            ActivityLoading.IsVisible = false;
            MainDataStack.IsVisible = true;
            NotasyncMethod(Ex);
        }
    }

    async private void PreSelectItems()
    {
        try
        {
            //Preload all Data 
            var Duration = ExistingActivity.startdate.Substring(10);
            var Date = ExistingActivity.startdate.Substring(0, 8);
            //DateTime && Set Selected Date time to
            if(Date == DateTime.Now.ToString("dd/MM/yy"))
            {
                Datelbl.Text = "Today";
            }
            else
            {
                Datelbl.Text = Date;
            }
           
            Timelbl.Text = Duration;

           


            //Category Section 
            var SelectedCat = AllActivities.Where(x => x.activitytitle == ExistingActivity.activitytitle).FirstOrDefault();
            var CategSelected = FilterTabsList.FirstOrDefault(item => item.ShortGroup == SelectedCat.ShortGroup);

            FilterTabsListview.SelectedItem = CategSelected;
            var CatIndex = FilterTabsList.IndexOf(CategSelected);

            FilterTabsListview.ScrollTo(FilterTabsList[CatIndex], ScrollToPosition.Center, true);

            var Filter = AllActivities
             .Where(s => s.ShortGroup == SelectedCat.ShortGroup)
             .ToList().OrderBy(g => g.activitytitle);

            FilterItemsList = new ObservableCollection<dailyactivity>(Filter);

            SelectActivityListview.ItemsSource = FilterItemsList;

            //Activity titile 
            var SelectedAT = FilterItemsList.Where(item => item.activitytitle == ExistingActivity.activitytitle).FirstOrDefault();

            SelectActivityListview.SelectedItem = SelectedAT;

            var ActIndex = FilterItemsList.IndexOf(SelectedAT);

            SelectActivityListview.ScrollTo(FilterItemsList[ActIndex], ScrollToPosition.Center, true);

            //Duration Should never be null 
            var SplitDuration = ExistingActivity.ActivityFeedbackList.Duration.Split(":");
            hoursentry.Text = SplitDuration[0];
            minsentry.Text = SplitDuration[1];


            //Dont Add Feedback for Duplicate 
            //if (AddorEdit != "Duplicate")
            //{

                //update Feedbacktoadd 
                FeedbacktoAdd.Mood = ExistingActivity.ActivityFeedbackList.Mood;
                FeedbacktoAdd.Outcome = ExistingActivity.ActivityFeedbackList.Outcome;
                FeedbacktoAdd.Duration = ExistingActivity.ActivityFeedbackList.Duration;
                FeedbacktoAdd.Completed = ExistingActivity.ActivityFeedbackList.Completed;

                if (!String.IsNullOrEmpty(ExistingActivity.ActivityFeedbackList.Completed))
                {
                    if (ExistingActivity.ActivityFeedbackList.Completed == "Completed")
                    {
                        btnyes.Background = Color.FromArgb("#fce9d9");
                        btnyes.BorderColor = Colors.Transparent;
                        btnyes.TextColor = Color.FromArgb("#991B1B");
                        ShowFeedback.IsVisible = true;
                    }
                    else
                    {
                        btnno.Background = Color.FromArgb("#fce9d9");
                        btnno.BorderColor = Colors.Transparent;
                        btnno.TextColor = Color.FromArgb("#991B1B");
                        ShowFeedback.IsVisible = false;
                    }
                }

                if (!String.IsNullOrEmpty(ExistingActivity.ActivityFeedbackList.Mood))
                {
                    var Selectedmood = AllMoods.Where(item => item.Text == ExistingActivity.ActivityFeedbackList.Mood).FirstOrDefault();

                    MoodListview.SelectedItem = Selectedmood;

                    var MoodIndex = AllMoods.IndexOf(Selectedmood);

                    MoodListview.ScrollTo(AllMoods[MoodIndex], ScrollToPosition.Center, true);
                }

                if (!String.IsNullOrEmpty(ExistingActivity.ActivityFeedbackList.Outcome))
                {
                    Activityquality.SelectedItem = ExistingActivity.ActivityFeedbackList.Outcome;
                }

                if (!String.IsNullOrEmpty(ExistingActivity.notes))
                {
                    Notes.Text = ExistingActivity.notes;
            }
            //}

            if (AddorEdit == "Edit")
            {
                AddActivitybtn.Text = "Edit Activity";

                ActivitySelectStack.IsVisible = true;
                DurationStack.IsVisible = true;
                AddActivitybtn.IsVisible = true;
            }
            else if (AddorEdit == "Feedback")
            {
                AddActivitybtn.Text = "Update Feedback";

                DateTimePicker.IsVisible = false;
                FilterTabStack.IsVisible = false;
                ActivitySelectStack.IsVisible = false;
                DurationStack.IsVisible = false;
                CheckDateToShowFeedback();
                AddActivitybtn.IsVisible = true;
                DailyActivitylbl.Text = ExistingActivity.activitytitle;
            }
            else if (AddorEdit == "Duplicate")
            {
                AddActivitybtn.Text = "Duplicate Activity";
                DurationStack.IsVisible = true;
                AddActivitybtn.IsVisible = true;

                FilterTabStack.IsVisible = false;
                ActivitySelectStack.IsVisible = false;

                NewUserActivity.activityid = ExistingActivity.activityid;
                NewUserActivity.activitytitle = ExistingActivity.activitytitle;
                DailyActivitylbl.Text = ExistingActivity.activitytitle;
            }

        }
        catch (Exception Ex)
        {
        }
    }

    private void CheckDateToShowFeedback()
    {
        try
        {
            //Gets Current Selected Time 
            string selectedDate = Datelbl.Text == "Today" ? DateTime.Today.ToString("dd/MM/yy") : Datelbl.Text;

            // Safely parse hours and minutes
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;
            // Create a TimeSpan for duration
            var duration = new TimeSpan(hours, minutes,0);

            // Parse selected DateTime safely
            if (!DateTime.TryParseExact($"{selectedDate} {Timelbl.Text}", "dd/MM/yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDateTime))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            // Add duration to selected time
            var finalDateTime = selectedDateTime + duration;
            var currentDateTime = DateTime.Now;

            // Show feedback if the date is in the past OR it's an "Add Feedback" action
            FeedbackStack.IsVisible = finalDateTime < currentDateTime || AddorEdit == "Feedback";
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private async void Close_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Close Clicked
            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterTabsListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as dailyactivity;
            var Filter = AllActivities
               .Where(s => s.ShortGroup == Selecteditem.ShortGroup)
               .ToList().OrderBy(g => g.activitytitle);

            FilterItemsList = new ObservableCollection<dailyactivity>(Filter);

            SelectActivityListview.ItemsSource = FilterItemsList;
            ActivitySelectStack.IsVisible = true; 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SelectActivityListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

        try
        {
            var Selecteditem = e.DataItem as dailyactivity;

            //Add New
            NewUserActivity.activitytitle = Selecteditem.activitytitle;
            NewUserActivity.activityid = Selecteditem.activityid;

            //Update Exisiting 
            ExistingActivity.activitytitle = Selecteditem.activitytitle;
            NewUserActivity.activityid = Selecteditem.activityid;

            DurationStack.IsVisible = true;

            CheckDateToShowFeedback();

            AddActivitybtn.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AddActivity_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Auto Add These Details 
            NewUserActivity.userid = Helpers.Settings.UserKey;
            NewUserActivity.activityplanner = true;
            NewUserActivity.active = true;
            //Was used to Know if Notifications Were to be enabled/ disabled
            NewUserActivity.activityoption = "On";
            //Activity Title Already Added 

            //DateTime Always populated 
            var DateTimeString = string.Empty;
            if (Datelbl.Text == "Today")
            {
                var Date = DateTime.Now.ToString("dd/MM/yy");
                DateTimeString = Date + ", " + Timelbl.Text;
            }
            else
            {
                var Date = DateTime.Parse(Datelbl.Text).ToString("dd/MM/yy");
                DateTimeString = Date + ", " + Timelbl.Text;
            }

            NewUserActivity.startdate = DateTimeString;
            ExistingActivity.startdate = DateTimeString;

            //What Kind of activity Selected item 
            if (FilterTabsListview.SelectedItem == null)
            {
                Vibration.Vibrate();
                await DisplayAlert("Select Category", "Please select a category before continuing.", "OK");
                return;
            }

            //Select Activity item 
            if (SelectActivityListview.SelectedItem == null)
            {
                Vibration.Vibrate();
                await DisplayAlert("Select Activity", "Please select a activity before continuing.", "OK");
                return;
            }

            //Duration not equal to null 

            if (string.IsNullOrEmpty(hoursentry.Text) && string.IsNullOrEmpty(minsentry.Text))
            {
                Vibration.Vibrate();
                await DisplayAlert("Enter Duration", "Please enter a duration before continuing.", "OK");
                return;
            }

            if (!string.IsNullOrEmpty(Notes.Text))
            {
                NewUserActivity.notes = Notes.Text;
                ExistingActivity.notes = Notes.Text;
            }

            //Activity Feedback Ensure Not Null 
            if (NewUserActivity.ActivityFeedbackList == null)
            {
                NewUserActivity.ActivityFeedbackList = new ActivityFeedback();
            }

            if (ExistingActivity.ActivityFeedbackList == null)
            {
                ExistingActivity.ActivityFeedbackList = new ActivityFeedback();
            }

            //Mood and Outcome Already Added if Selcted 
            FeedbacktoAdd.Duration = hoursentry.Text + ":" + minsentry.Text;
            NewUserActivity.ActivityFeedbackList = FeedbacktoAdd;
            ExistingActivity.ActivityFeedbackList = FeedbacktoAdd;

            //if not Empty Then Convert to string for DB (Should never be null)
            NewUserActivity.feedback = JsonConvert.SerializeObject(NewUserActivity.ActivityFeedbackList);
            ExistingActivity.feedback = JsonConvert.SerializeObject(ExistingActivity.ActivityFeedbackList);

            //<**> Add the Following <**>
            //Add in Symptoms Later 
            //Update UserFeedback for dailyActivity on Dash 
            //Activity Symptoms 
            if (NewUserActivity.ActivitySymptomsList == null)
            {
                NewUserActivity.ActivitySymptomsList = new ObservableCollection<ActivitySymptoms>();
            }

            if (ExistingActivity.ActivitySymptomsList == null)
            {
                ExistingActivity.ActivitySymptomsList = new ObservableCollection<ActivitySymptoms>();
            }
            //foreach (var item in SelectedUserSymptoms)
            //{
            //    var Addsym = new ActivitySymptoms();
            //    Addsym.symptomid = item.symptomid;
            //    Addsym.symptomtitle = item.symptomtitle;
            //    NewUserActivity.ActivitySymptomsList.Add(Addsym);
            //}

            //if not Empty Then Convert to string for DB 
            //if (NewUserActivity.ActivitySymptomsList.Count > 0)
            //{
            //    NewUserActivity.activitysymptoms = JsonConvert.SerializeObject(NewUserActivity.ActivitySymptomsList);
            //}

            APICalls database = new APICalls();
            //Edit Exisiting Activity 

            //Update Feedback 
            if (String.IsNullOrEmpty(AddorEdit) || AddorEdit == "Duplicate")
            {
                //Add New Activity && Duplicate Activity 

                NewUserActivity = await database.PostUserActiivty(NewUserActivity);

                //Add New Activity 
                AllUserActivities.Add(NewUserActivity);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Daily Activity Added"));
                await Navigation.PushAsync(new ActivitySchedule(AllUserActivities));
              
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ActivitySchedule);
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                if (!string.IsNullOrEmpty(AddorEdit))
                {
                    var RemovePage = Navigation.NavigationStack.FirstOrDefault(x => x is SingleActivity);
                    if (RemovePage != null)
                    {
                        Navigation.RemovePage(RemovePage);
                    }
                }

                Navigation.RemovePage(this);
            }
            else if (AddorEdit == "Feedback")
            {
                await database.UpdateUserActivity(ExistingActivity);

                foreach (var item in AllUserActivities)
                {
                    if (item.id == ExistingActivity.id)
                    {
                        item.notes = ExistingActivity.notes;

                        item.feedback = JsonConvert.SerializeObject(ExistingActivity.ActivityFeedbackList);
                        item.ActivityFeedbackList = ExistingActivity.ActivityFeedbackList;
                    }
                }

                await MopupService.Instance.PushAsync(new PopupPageHelper("Feedback Added"));
                await Navigation.PushAsync(new ActivitySchedule(AllUserActivities));

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ActivitySchedule);
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                var RemovePage = Navigation.NavigationStack.FirstOrDefault(x => x is SingleActivity);
                if (RemovePage != null)
                {
                    Navigation.RemovePage(RemovePage);
                }
                Navigation.RemovePage(this);


            }
            else if (AddorEdit == "Edit")
            {
                await database.UpdateActivityDetails(ExistingActivity);

                foreach (var item in AllUserActivities)
                {
                    if (item.id == ExistingActivity.id)
                    {
                        item.activitytitle = ExistingActivity.activitytitle;
                        item.activityid = ExistingActivity.activityid;
                        item.startdate = ExistingActivity.startdate;
                        item.notes = ExistingActivity.notes;

                        item.feedback = JsonConvert.SerializeObject(ExistingActivity.ActivityFeedbackList);
                        item.ActivityFeedbackList = ExistingActivity.ActivityFeedbackList;
                    }
                }

                await MopupService.Instance.PushAsync(new PopupPageHelper("Activity Edited"));

                await Navigation.PushAsync(new ActivitySchedule(AllUserActivities));

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ActivitySchedule);
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                var RemovePage = Navigation.NavigationStack.FirstOrDefault(x => x is SingleActivity);
                if (RemovePage != null)
                {
                    Navigation.RemovePage(RemovePage);
                }
                Navigation.RemovePage(this);
            }

            // Ensures page fully loaded 
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Delay(1000); 
                await MopupService.Instance.PopAllAsync(false);
            });

            //Old Update on Add 
            //WeakReferenceMessenger.Default.Send(new AddNewActivity(NewUserActivity));

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void MoodListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedItem = e.DataItem as moodlist;
            FeedbacktoAdd.Mood = SelectedItem.Text; 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void DateTime_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var DateTimePass = Datelbl.Text + " " + Timelbl.Text;
            var GetDateTime = new TaskCompletionSource<string>();

            // Open the popup and pass the TaskCompletionSource
            await MopupService.Instance.PushAsync(new DateTimePopup(GetDateTime, DateTimePass));

            string selectedDate = await GetDateTime.Task;

            var SplitDate = selectedDate.Split(',');
            Datelbl.Text = SplitDate[0];
            Timelbl.Text = SplitDate[1];

            if (DurationStack.IsVisible == true)
            {
                CheckDateToShowFeedback();
            }          
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void hoursentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //hours entry text changed
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                // Validate the value is within the range (0-59)
                if (int.TryParse(newText, out int minutes))
                {
                    if (minutes > 23)
                    {
                        newText = "00"; // Set to max value
                    }
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }

            CheckDateToShowFeedback();

        }
        catch (Exception ex)
        {

        }
    }

    private void minsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                // Validate the value is within the range (0-59)
                if (int.TryParse(newText, out int minutes))
                {
                    if (minutes > 59)
                    {
                        newText = "00"; // Set to max value
                    }
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }

            CheckDateToShowFeedback();

        }
        catch (Exception ex)
        {

        }
    }

    private void incrementminutes_Clicked(object sender, EventArgs e)
    {
        try
        {
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            var GetCommand = (sender) as Button;
            var StringTime = GetCommand.CommandParameter.ToString();
            int GetMins = Int32.Parse(StringTime);
            minutes += GetMins;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");

            CheckDateToShowFeedback();

        }
        catch (Exception ex)
        {

        }
    }

    private void Activityquality_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedItem = e.DataItem as string;
            FeedbacktoAdd.Outcome = SelectedItem;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Completed_Clicked(object sender, EventArgs e)
    {
        try
        {
            var GetCommand = (sender) as Button;
            var ShowHide = GetCommand.CommandParameter.ToString();

            btnyes.Background = Colors.Transparent;
            btnyes.BorderColor = Colors.LightGray;
            btnyes.TextColor = Colors.Gray;

            btnno.Background = Colors.Transparent;
            btnno.BorderColor = Colors.LightGray;
            btnno.TextColor = Colors.Gray;

            if (ShowHide == "Yes")
            {
                btnyes.Background = Color.FromArgb("#fce9d9");
                btnyes.BorderColor = Colors.Transparent;
                btnyes.TextColor = Color.FromArgb("#991B1B");

                ShowFeedback.IsVisible = true;
                FeedbacktoAdd.Completed = "Completed"; 
            }
            else
            {
                btnno.Background = Color.FromArgb("#fce9d9");
                btnno.BorderColor = Colors.Transparent;
                btnno.TextColor = Color.FromArgb("#991B1B");

                ShowFeedback.IsVisible = false;
                FeedbacktoAdd.Completed = "Not Completed";
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Duration_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var DurationtoPass = "00 00";
            var GetDuration = new TaskCompletionSource<string>();

            // Open the popup and pass the TaskCompletionSource
            await MopupService.Instance.PushAsync(new ActivityDuration(GetDuration, DurationtoPass));

            string selectedDuration = await GetDuration.Task;

            Durationlbl.Text = selectedDuration;

            //if (DurationStack.IsVisible == true)
            //{
            //    CheckDateToShowFeedback();
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Incrementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            var SelectedItem = e.DataItem as string;

            var StringTime = SelectedItem.Replace("+", "").Replace("m", "");
            if (int.TryParse(StringTime, out int GetMins))
            {
                minutes += GetMins;

                // Handle overflow into hours correctly
                hours += minutes / 60;  // Add extra hours from overflow
                minutes = minutes % 60; // Keep minutes in range (0-59)

                // Optional: Limit hours to 24-hour format (reset to 0 after 24)
                if (hours >= 24)
                {
                    hours = 0;
                }

                // Update the Entries with the new values
                hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
                minsentry.Text = minutes.ToString("D2");
            }


            CheckDateToShowFeedback();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }



    //private void SwapSection_Tapped(object sender, TappedEventArgs e)
    //{
    //    try
    //    {

    //        Sectionone.Background = Colors.LightGray;
    //        Sectionone.BackgroundColor = Colors.LightGray;
    //        Imageone.Source = "durationeditlg.png";

    //        Sectiontwo.Background = Colors.LightGray;
    //        Sectiontwo.BackgroundColor = Colors.LightGray;
    //        Imagetwo.Source = "durationaddlg.png";

    //        var GetCommand = (TappedEventArgs)e;
    //        string Passed = (string)GetCommand.Parameter;

    //        if (!string.IsNullOrEmpty(Passed))
    //        {
    //            //Section One 
    //            if (Passed == "1")
    //            {
    //                Sectionone.Background = Color.FromArgb("#991B1B");
    //                Sectionone.BackgroundColor = Color.FromArgb("#991B1B");
    //                Imageone.Source = "durationeditr.png";
    //                Showbtns.IsVisible = false;
    //            }
    //            //Section Two
    //            else if (Passed == "2")
    //            {
    //                Sectiontwo.Background = Color.FromArgb("#991B1B");
    //                Sectiontwo.BackgroundColor = Color.FromArgb("#991B1B");
    //                Imagetwo.Source = "durationaddr.png";
    //                Showbtns.IsVisible = false;
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}
}