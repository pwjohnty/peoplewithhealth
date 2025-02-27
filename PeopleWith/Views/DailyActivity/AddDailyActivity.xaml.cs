using CommunityToolkit.Mvvm.Messaging;
using Maui.FreakyControls.Extensions;
using Microsoft.Datasync.Client;
using Microsoft.Maui.Layouts;
using Mopups.PreBaked.AbstractClasses;
using Mopups.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;

namespace PeopleWith;

public partial class AddDailyActivity : ContentPage
{
    public ObservableCollection<dailyactivity> AllActivities = new ObservableCollection<dailyactivity>();
    public ObservableCollection<dailyactivity> FilterResults = new ObservableCollection<dailyactivity>();
    public ObservableCollection<dailyactivity> FilterTabsList = new ObservableCollection<dailyactivity>();
    public ObservableCollection<activefrequency> timeslistitems = new ObservableCollection<activefrequency>(); 
    public userdailyactivity NewuserActivity = new userdailyactivity();
    public ObservableCollection<usersymptom> AllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> SelectedUserSymptoms = new ObservableCollection<usersymptom>();
    List<string> SymptomList = new List<string>();
    List<string> SelectedDays = new List<string>();
    public TimeSpan InitalTime = new TimeSpan();
    public TimeSpan NormalAdd = new TimeSpan();
    public bool Reminders = true;
    public bool AddtoPlan = false; 
    //public userinvestigation InvestPassed = new userinvestigation();
    //public notesuserfeedback NotesPassed = new notesuserfeedback();

    List<string> freqlist = new List<string>();
    List<string> timesperdaylist = new List<string>();
    List<string> weekdayslist = new List<string>();
    List<string> minuteslist = new List<string>();

    public bool isEdit = false;
    public bool NoteUpdate = false;
    private bool FilterTabClicked = false;
    //20 for Add Process can be changed 
    public double ProgressNum = 16.67;  

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
    public AddDailyActivity()
    {
        InitializeComponent();
        GetActivityInfo();
        populateListviewsData();
    }

    async private void GetActivityInfo()
    {
        try
        {
            Activityloading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get Investigation Details 
            var getActivityTask = database.GetActivityDetails();
            AllActivities = await getActivityTask;

            //Get symptom Data
            var getSymptomsTask = database.GetUserSymptomAsync();
            AllUserSymptoms = await getSymptomsTask;

            AllUserSymptoms = AllUserSymptoms.Where(x => x.deleted == false).OrderBy(c => c.symptomtitle).ToObservable();

            symptomlistview.ItemsSource = AllUserSymptoms;

            //FilterTabs 
            ActivtyListview.ItemsSource = AllActivities.OrderBy(s => s.activitytitle);
            var count = AllActivities.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = AllActivities;

            //Add Classiciation Filters 
            var distinctinvest = AllActivities
                .GroupBy(s => s.ShortGroup)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.ShortGroup);

            FilterTabsList = new ObservableCollection<dailyactivity>(distinctinvest);

            // Insert "All" at the beginning of the list
            var AddAll = new dailyactivity
            {
                ShortGroup = "All"
            };
            FilterTabsList.Insert(0, AddAll);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "ShortGroup";
            Filterstack.IsVisible = true;
            Activityloading.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void populateListviewsData()
    {
        try
        {
            freqlist.Add("One Off");
            freqlist.Add("Daily");
            freqlist.Add("Weekly");
            freqlist.Add("Monthly");
            freqlist.Add("Quarterly");
            freqlist.Add("Yearly");
            
            freqlistview.ItemsSource = freqlist;

            weekdayslist.Add("Sun");
            weekdayslist.Add("Mon");
            weekdayslist.Add("Tue");
            weekdayslist.Add("Wed");
            weekdayslist.Add("Thu");
            weekdayslist.Add("Fri");
            weekdayslist.Add("Sat");

            whichdays.ItemsSource = weekdayslist;

            timesperdaylist.Add("One");
            timesperdaylist.Add("Two");
            timesperdaylist.Add("Three");
            timesperdaylist.Add("Four");
            timesperdaylist.Add("Five");
            timesperdaylist.Add("Six");
            timesperdaylist.Add("Seven");
            timesperdaylist.Add("Eight");

            timesperday.ItemsSource = timesperdaylist;



            //minuteslist.Add("15 Minutes");
            //minuteslist.Add("30 Minutes");
            //minuteslist.Add("45 Minutes");
            //minuteslist.Add("60 Minutes");
            //minuteslist.Add("75 Minutes");
            //minuteslist.Add("90 Minutes");
            //comboBox.ItemsSource = minuteslist;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterHideTapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (Filterstack.IsVisible == false)
            {
                Filterstack.IsVisible = true;
            }
            else
            {
                Filterstack.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (FilterTabClicked) return;

            var Characters = searchbar.Text.ToString();
            var filteredInvest = new ObservableCollection<dailyactivity>(FilterResults.Where(s => s.activitytitle.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.activitytitle);
            ActivtyListview.ItemsSource = filteredInvest;
            var count = filteredInvest.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";

            if (count == "0")
            {
                NoResultslbl.IsVisible = true;
                ActivtyListview.IsVisible = false;
            }
            else
            {
                ActivtyListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
            }

            //If FilterTabs item is Selected - UnSelect it 
            if (string.IsNullOrEmpty(searchbar.Text) || searchbar.Text == "")
            {
                FilterTabs.SelectedItem = FilterTabsList[0];
            }
            else
            {
                if (FilterTabs.SelectedItem != null)
                {
                    FilterTabs.SelectedItem = null;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterTabs_ChipClicked(object sender, EventArgs e)
    {
        try
        {
            var tappedFrame = sender as SfChip;
            var item = tappedFrame.Text;
            FilterTabClicked = true;

            if (item == "All")
            {
                var count = FilterResults.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                ActivtyListview.ItemsSource = FilterResults.OrderBy(s => s.activitytitle);
                ActivtyListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;
            }
            else
            {

                var filteredInvest = new ObservableCollection<dailyactivity>(FilterResults.Where(s => s.ShortGroup.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.activitytitle);
                var count = filteredInvest.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                ActivtyListview.ItemsSource = filteredInvest;
                ActivtyListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;

            }


            Device.BeginInvokeOnMainThread(() =>
            {
                FilterTabClicked = false;
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                backbtn.IsEnabled = false;

                //Add Title Stack (FirstStack) 
                if (TitleStack.IsVisible == true)
                {
                    if (ActivtyListview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Daily Activity", "Please select a Daily Activity from this list to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }
                    backbtn.Text = "Back";
                    HandleTitletoDateTime(); 
                }
                else if(DateTimeStack.IsVisible == true)
                {
                    HandleDateTimetoFrequency();
                }
                else if (FrequencyStack.IsVisible == true)
                {
                    if(freqlistview.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Activity Frequency", "Please select a Frequency from this list to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }

                    //Check item is weekly 
                    if(NewuserActivity.frequency == "Weekly")
                    {
                        if (whichdays.SelectedItem == null)
                        {
                            Vibration.Vibrate();
                            await DisplayAlert("Select days of the week", "Please select which days from this list to progress", "Ok", "Cancel");
                            nextbtn.IsEnabled = true;
                            backbtn.IsEnabled = true;
                            return;
                        }

                    }

                    if (timesperday.SelectedItem == null)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select times per day", "Please select how many times a day from this list to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }

                    HandleFrequencytoAddDetails();
                }
                else if (AddDetailspage.IsVisible == true)
                {
                    if (yesbtn.BackgroundColor == Colors.White && nobtn.BackgroundColor == Colors.White)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select same time everyday", "Please select 'Yes' or 'No' to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }

                    if (truebtn.BackgroundColor == Colors.White && falsebtn.BackgroundColor == Colors.White)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Add to Planner", "Please select 'Yes' or 'No' to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }

                    if (truebtn.BackgroundColor == Colors.White && falsebtn.BackgroundColor == Colors.White)
                    {
                        Vibration.Vibrate();
                        await DisplayAlert("Select Add to Planner", "Please select 'Yes' or 'No' to progress", "Ok", "Cancel");
                        nextbtn.IsEnabled = true;
                        backbtn.IsEnabled = true;
                        return;
                    }

                    HandleAddDetailstoNotesSection(); 

                }
                else if(NotesSection.IsVisible == true)
                {
                    HandleNotestoConfirm();
                }
                else if(ConfirmPageStack.IsVisible == true)
                {
                    //Do Nothing
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
            //Enable both Buttons if anything ever happens 
            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;

            NotasyncMethod(Ex);
        }
    }

    private async void UpdateProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress += ProgressNum;

            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch( Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void RetrogressProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress -= ProgressNum;

            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }




    private async void HandleTitletoDateTime()
    {
        try
        {
            //Set Pages
            TitleStack.IsVisible = false;
            DateTimeStack.IsVisible = true;

            //Set Title on DateTimeStack 
            if (!String.IsNullOrEmpty(NewuserActivity.activitytitle))
            {
                DateTimeTitle.Text = NewuserActivity.activitytitle;
            }

            UpdateProgress();

            //Enable Navigation buttons Again 
            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HandleDateTimetoFrequency()
    {
        try
        {
            //Set Pages
            DateTimeStack.IsVisible = false;
            FrequencyStack.IsVisible = true;

            //Set Title on Freq Stack 
            if (!String.IsNullOrEmpty(NewuserActivity.activitytitle))
            {
                FrequencyTitle.Text = NewuserActivity.activitytitle;
            }

            UpdateProgress();

            //Enable Navigation buttons Again 
            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HandleFrequencytoAddDetails()
    {
        try
        {
            //Set Pages
            FrequencyStack.IsVisible = false;
            AddDetailspage.IsVisible = true;

            //Set Title on DateTimeStack 
            if (!String.IsNullOrEmpty(NewuserActivity.activitytitle))
            {
                DetailsTitle.Text = NewuserActivity.activitytitle;
            }

            UpdateProgress();

            //Enable Navigation buttons Again 
            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HandleAddDetailstoNotesSection()
    {
        try
        {
            //Set Pages
            AddDetailspage.IsVisible = false;
            NotesSection.IsVisible = true;

            //Set Title on DateTimeStack 
            if (!String.IsNullOrEmpty(NewuserActivity.activitytitle))
            {
                AddDetailsTitle.Text = NewuserActivity.activitytitle;
            }

            UpdateProgress();

            //Enable Navigation buttons Again 
            nextbtn.IsEnabled = true;
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HandleNotestoConfirm()
    {
        try
        {
            //Set Pages
            NotesSection.IsVisible = false;
            ConfirmPageStack.IsVisible = true;

            //Set Title on DateTimeStack 
            if (!String.IsNullOrEmpty(NewuserActivity.activitytitle))
            {
                ConfirmPageTitle.Text = NewuserActivity.activitytitle;
            }

            //Populate Data 
            //Display Name 
            if (!string.IsNullOrEmpty(displaynameentry.Text))
            {
                confirmdisplaynamelbl.Text = displaynameentry.Text;
                //Add Display Name to Add NewUserActiivty
            }
            else
            {
                confirmdisplaynamelbl.Text = "--";
            }

            //Activity Notes
            if (!string.IsNullOrEmpty(notesentry.Text))
            {
                confirmnoteslbl.Text = notesentry.Text;
                //Add Display Notes to Add NewUserActiivty
            }
            else
            {
                confirmnoteslbl.Text = "--";
            }

            //Reminder Notifications
            if (Reminders == true)
            {
                Reminderlbl.Text = "Yes";
                //Add Display Notes to Add NewUserActiivty
            }
            else
            {
                Reminderlbl.Text = "No";
            }

            //Add to Planner
            if (AddtoPlan == true)
            {
                Plannerlbl.Text = "Yes";
                //Add Display Notes to Add NewUserActiivty
            }
            else
            {
                Plannerlbl.Text = "No";
            }

            //Set Start Date 
            StartDatelbl.Text = startdatepicker.Date.ToString("dd/MM/yy");

            //Set End Date 
            if (enddatecheck.IsChecked == true)
            {
                EndDatelbl.Text = enddatepicker.Date.ToString("dd/MM/yy");
            }
            else
            {
                EndDatelbl.Text = "Ongoing";
            }

            if (AllUserSymptoms.Count == 0)
            {
                ImpactedSymptomsStack.IsVisible = false;
            }
            else
            {
                ImpactedSymptomsStack.IsVisible = true;
                if (SelectedUserSymptoms.Count == 0)
                {
                    ConfirmSymptoms.IsVisible = false;
                    NoSympyomslbl.IsVisible = true;
                }
                else
                {
                    ConfirmSymptoms.IsVisible = true;
                    NoSympyomslbl.IsVisible = false;
                    ConfirmSymptoms.ItemsSource = SelectedUserSymptoms;
                }
            }

            //Confirm Times 

            ConfirmTimes.ItemsSource = timeslistitems; 



            UpdateProgress();

            //Enable Navigation buttons Again 
            nextbtn.IsEnabled = true;
            nextbtn.IsVisible = false; 
            backbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HandleAddNewActivity()
    {
        try
        {
            //Update NewUserActivity With Final Details 

            //UserID 
            NewuserActivity.userid = Helpers.Settings.UserKey;

            //Title && Activity ID (Already Added)

            //Activity Frequency 
            if (NewuserActivity.activityfrequencylist == null)
            {
                NewuserActivity.activityfrequencylist = new ObservableCollection<activefrequency>();
            }

            foreach (var item in timeslistitems)
            {
                NewuserActivity.activityfrequencylist.Add(item);
            }

            //Reminders Stored here for Now
            bool CheckReminder = Reminderlbl.Text == "Yes";
            if (CheckReminder)
            {
                NewuserActivity.activityoption = "On";
            }
            else
            {
                NewuserActivity.activityoption = "Off";
            }

            if (NewuserActivity.activitysymptomslist == null)
            {
                NewuserActivity.activitysymptomslist = new ObservableCollection<activesymptoms>();
            }

            foreach (var item in SelectedUserSymptoms)
            {
                var Addsym = new activesymptoms();
                Addsym.symptomid = item.symptomid;
                Addsym.symptomtitle = item.symptomtitle;
                NewuserActivity.activitysymptomslist.Add(Addsym);
            }

            // Join symptoms into a single string (symptomid|symptomtitle_symptomid|symptomtitle_ ...)
            //NewuserActivity.activitysymptoms = string.Join("_", SelectedUserSymptoms.Select(item => $"{item.symptomid}|{item.symptomtitle}"));
            NewuserActivity.activityplanner = Plannerlbl.Text == "Yes";

            //Start Date 
            NewuserActivity.startdate = StartDatelbl.Text;

            //EndDate 
            if(EndDatelbl.Text == "Ongoing")
            {
                //Leave Null
            }
            else
            {
                NewuserActivity.enddate = EndDatelbl.Text;
            }

            //Active (Pre-Set to "Active") 
            NewuserActivity.active = true;

            //Notes 
            if (NewuserActivity.notes == null)
            {
                NewuserActivity.noteslist = new ObservableCollection<activenotes>();
            }
            var notesoptions = new activenotes
            {
                displayname = string.IsNullOrWhiteSpace(displaynameentry.Text) ? null : displaynameentry.Text,
                notes = string.IsNullOrWhiteSpace(notesentry.Text) ? null : notesentry.Text
            };

            NewuserActivity.noteslist.Add(notesoptions);

            //Add Notes
            if(NewuserActivity.noteslist.Count > 0)
            {
                NewuserActivity.notes = JsonConvert.SerializeObject(NewuserActivity.noteslist);
             
            }
            //Add frequency

            if(NewuserActivity.activityfrequencylist.Count > 0)
            {
                NewuserActivity.activityfrequency = JsonConvert.SerializeObject(NewuserActivity.activityfrequencylist);
            }

            //add Symptoms

            if(NewuserActivity.activitysymptomslist.Count > 0)
            {
                NewuserActivity.activitysymptoms = JsonConvert.SerializeObject(NewuserActivity.activitysymptomslist);
            }


            //Add New Activity 

            APICalls database = new APICalls();

            NewuserActivity = await database.PostUserActiivty(NewuserActivity);

            await MopupService.Instance.PushAsync(new PopupPageHelper("Daily Activty Added") { });
            await Task.Delay(1500);

            await MopupService.Instance.PopAllAsync(false);
            await Navigation.PushAsync(new AllDailyActivity());
            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllDailyActivity);
            if (pageToRemoves != null)
            {
                Navigation.RemovePage(pageToRemoves);
            }
            Navigation.RemovePage(this);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void backbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            nextbtn.IsEnabled = false;
            backbtn.IsEnabled = false;

            if (ConfirmPageStack.IsVisible == true)
            {
                nextbtn.IsVisible = true;
                ConfirmPageStack.IsVisible = false;
                NotesSection.IsVisible = true;
                RetrogressProgress();

            }
            else if (NotesSection.IsVisible == true)
            {
                NotesSection.IsVisible = false;
                AddDetailspage.IsVisible = true;
                RetrogressProgress();
            }
            else if (AddDetailspage.IsVisible == true)
            {
                AddDetailspage.IsVisible = false;
                FrequencyStack.IsVisible = true;
                RetrogressProgress();
            }
            else if (FrequencyStack.IsVisible == true)
            {
                FrequencyStack.IsVisible = false;
                DateTimeStack.IsVisible = true;
                RetrogressProgress();
            }
            else if (DateTimeStack.IsVisible == true)
            {
                backbtn.Text = "Cancel";
                DateTimeStack.IsVisible = false;
                TitleStack.IsVisible = true;
                RetrogressProgress();
            }
            else if (TitleStack.IsVisible == true)
            {
                //Close Page
                Navigation.RemovePage(this);
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ActivtyListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedItem = e.DataItem as dailyactivity;
            if (SelectedItem != null)
            {
                //Add Title & ID to NewUserActivity
                NewuserActivity.activitytitle = SelectedItem.activitytitle;
                NewuserActivity.activityid = SelectedItem.activityid;
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void startdatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {

            if (enddatecheck.IsChecked == true)
            {
                enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);
            }
           
        }
        catch (Exception Ex)
        {
           //Leave Empty
        }
    }

    private void enddatecheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (!e.Value)
            {
                //Unchecked
                enddategrid.Opacity = 0.2;
                enddatepicker.IsEnabled = false;
                NewuserActivity.enddate = null;
                Ongoinglbl.IsVisible = true;
                enddatepicker.IsVisible = false; 
            }
            else
            {
                //ischecked
                enddategrid.Opacity = 1;
                enddatepicker.IsEnabled = true;
                enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);
                Ongoinglbl.IsVisible = false;
                enddatepicker.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void AddDietBtn_Clicked(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void freqlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as string;

            //Set All Items Isvisible = False; 
            WhichDaysStack.IsVisible = false;
            HowManyTimesStack.IsVisible = false;
            SameForAllStack.IsVisible = false;
            SetTimesStack.IsVisible = false;
            SelectedDays.Clear();

            //Clear All Selected Items 
            if(whichdays.SelectedItem != null)
            {
                whichdays.SelectedItem = null;
            }

            if(timesperday.SelectedItem != null)
            {
                timesperday.SelectedItem = null; 
            }

            timeslistview.ItemsSource = null;

            if (Selecteditem == "Weekly")
            {
                WhichDaysStack.IsVisible = true;
              
            }
            else
            {
                HowManyTimesStack.IsVisible = true;
            }

            NewuserActivity.frequency = Selecteditem; 

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void whichdays_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as string;

            //Set Items Isvisible = False; 
            HowManyTimesStack.IsVisible = false;
            SameForAllStack.IsVisible = false; 
            SetTimesStack.IsVisible = false;

            //Clear Selected Items 
            if (timesperday.SelectedItem != null)
            {
                timesperday.SelectedItem = null;
            }

            timeslistview.ItemsSource = null;

            HowManyTimesStack.IsVisible = true;
            if(SelectedDays.Count == 0)
            {
                SelectedDays.Add(Selecteditem); 
            }
            else
            {
                //Add/ Remove items 
                if (SelectedDays.Contains(Selecteditem))
                {
                    SelectedDays.Remove(Selecteditem);
                }
                else
                {
                    SelectedDays.Add(Selecteditem);
                }
            }


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void timesperday_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as string;

            NewuserActivity.SelectedTimes = ParseTime(Selecteditem); 
            //Set Items Isvisible = False; 
            SameForAllStack.IsVisible = false; 
            SetTimesStack.IsVisible = false;

            //Clear Selected Items 
            timeslistview.ItemsSource = null;

            if (SelectedDays.Count != 0)
            {
                if(SelectedDays.Count > 1)
                {
                    SameForAllStack.IsVisible = true; 
                }
                else
                {
                    SetTimesStack.IsVisible = true;
                    QuickAddTimesStack.IsVisible = true;
                    PopulateTimelist();
                }
            }
            else
            {
                SetTimesStack.IsVisible = true;
                QuickAddTimesStack.IsVisible = true;
                PopulateTimelist(); 
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private async void PopulateTimelist()
    {
        try
        {
            var SelectedTimes = NewuserActivity.SelectedTimes;

            timeslistitems.Clear();

            if (!String.IsNullOrEmpty(NewuserActivity.frequency))
            {
                var GetFreq = NewuserActivity.frequency;
                if (String.IsNullOrEmpty(MinutesEntry.Text))
                {
                    //Set to Normal Interval
                    InitalTime = QuickAddTime.Time;
                    NormalAdd = TimeSpan.FromMinutes(90);
                    InitalTime = TimeSpan.FromHours(8);
                }
                else
                {
                    var GetEntry = Int32.Parse(MinutesEntry.Text);
                    NormalAdd = TimeSpan.FromMinutes(GetEntry);
                    InitalTime = QuickAddTime.Time;
                }
               
                if (GetFreq == "Weekly")
                {
                    var SetTime = InitalTime;
                    for (int k = 0; k < SelectedDays.Count(); k++)
                    {

                        for (int i = 0; i < SelectedTimes; i++)
                        {
                            Random random = new Random();
                            int newid = random.Next(100000, 100000001);
                            var ActivityTime = new activefrequency
                            {
                                id = newid.ToString(),
                                itemno = (i + 1).ToString(),
                                deleted = false,
                                day = SelectedDays[k],
                                frequency = NewuserActivity.frequency,
                                Timespan = SetTime,
                                time = SetTime.ToString(@"hh\:mm")
                            };

                            SetTime = SetTime.Add(NormalAdd);
                            timeslistitems.Add(ActivityTime);
                        }
                    }
                }
                else
                {
                    var SetTime = InitalTime;
                    for (int i = 0; i < SelectedTimes; i++)
                    {
                        Random random = new Random();
                        int newid = random.Next(100000, 100000001);

                        var ActivityTime = new activefrequency
                        {
                            id = newid.ToString(),
                            itemno = (i + 1).ToString(),
                            deleted = false,
                            frequency = NewuserActivity.frequency,
                            Timespan = SetTime,
                            time = SetTime.ToString(@"hh\:mm")
                        };

                        SetTime = SetTime.Add(NormalAdd);
                        timeslistitems.Add(ActivityTime);
                    }
                }

            }
            else
            {
                //Get Selected item 
            }
    

           

            timeslistview.ItemsSource = timeslistitems;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    int ParseTime(string day)
    {
        return day switch
        {
            "One" => 1,
            "Two" => 2,
            "Three" => 3,
            "Four" => 4,
            "Five" => 5,
            "Six" => 6,
            "Seven" => 7,
            "Eight" => 8,
            _ => throw new ArgumentException("Invalid int format")
        };
    }

    private void timeslistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        //Fill in 
    }

    private void SameforAll_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Set Both to Gray 
            btnyes.BorderColor = Color.FromArgb("#fce9d9");
            btnyes.BackgroundColor = Colors.White;
            btnyes.TextColor = Colors.Gray;

            btnno.BorderColor = Color.FromArgb("#fce9d9");
            btnno.BackgroundColor = Colors.White;
            btnno.TextColor = Colors.Gray;

            var GetCommand = (sender) as Button;
            var BtnPressed = GetCommand.CommandParameter.ToString();

            if(BtnPressed == "Yes")
            {
                btnyes.BorderColor = Colors.Transparent;
                btnyes.BackgroundColor = Color.FromArgb("#fce9d9");
                btnyes.TextColor = Color.FromArgb("#77212E"); 
            }
            else
            {
                btnno.BorderColor = Colors.Transparent;
                btnno.BackgroundColor = Color.FromArgb("#fce9d9");
                btnno.TextColor = Color.FromArgb("#77212E");
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void MinutesEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Work out if possible first 
        var ValueString = e.NewTextValue;
        if (e.NewTextValue.Contains("."))
        {
            ValueString = e.NewTextValue.Replace(".", "");
            MinutesEntry.Text = ValueString;
        }
        PopulateTimelist(); 
    }

    private void enableNotifications_Clicked(object sender, EventArgs e)
    {
        try
        {

            //Set Both to Gray 
            enablebtn.BorderColor = Color.FromArgb("#fce9d9");
            enablebtn.BackgroundColor = Colors.White;
            enablebtn.TextColor = Colors.Gray;

            disablebtn.BorderColor = Color.FromArgb("#fce9d9");
            disablebtn.BackgroundColor = Colors.White;
            disablebtn.TextColor = Colors.Gray;

            var GetCommand = (sender) as Button;
            var BtnPressed = GetCommand.CommandParameter.ToString();

            if (BtnPressed == "Yes")
            {
                enablebtn.BorderColor = Colors.Transparent;
                enablebtn.BackgroundColor = Color.FromArgb("#fce9d9");
                enablebtn.TextColor = Color.FromArgb("#77212E");
                Reminders = true;
            }
            else
            {
                disablebtn.BorderColor = Colors.Transparent;
                disablebtn.BackgroundColor = Color.FromArgb("#fce9d9");
                disablebtn.TextColor = Color.FromArgb("#77212E");
                Reminders = false;
            }

            //Set Next item 
            AddtoPlanner.IsVisible = true;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SameTimeQuestion_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Set Both to Gray 
            yesbtn.BorderColor = Color.FromArgb("#fce9d9");
            yesbtn.BackgroundColor = Colors.White;
            yesbtn.TextColor = Colors.Gray;

            nobtn.BorderColor = Color.FromArgb("#fce9d9");
            nobtn.BackgroundColor = Colors.White;
            nobtn.TextColor = Colors.Gray;

            var GetCommand = (sender) as Button;
            var BtnPressed = GetCommand.CommandParameter.ToString();

            if (BtnPressed == "Yes")
            {
                yesbtn.BorderColor = Colors.Transparent;
                yesbtn.BackgroundColor = Color.FromArgb("#fce9d9");
                yesbtn.TextColor = Color.FromArgb("#77212E");
            }
            else
            {
                nobtn.BorderColor = Colors.Transparent;
                nobtn.BackgroundColor = Color.FromArgb("#fce9d9");
                nobtn.TextColor = Color.FromArgb("#77212E");
            }

            //Set Next item 
            AddtoPlanner.IsVisible = true; 

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void AddtoPlanner_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Set Both to Gray 
            truebtn.BorderColor = Color.FromArgb("#fce9d9");
            truebtn.BackgroundColor = Colors.White;
            truebtn.TextColor = Colors.Gray;

            falsebtn.BorderColor = Color.FromArgb("#fce9d9");
            falsebtn.BackgroundColor = Colors.White;
            falsebtn.TextColor = Colors.Gray;

            var GetCommand = (sender) as Button;
            var BtnPressed = GetCommand.CommandParameter.ToString();

            if (BtnPressed == "Yes")
            {
                truebtn.BorderColor = Colors.Transparent;
                truebtn.BackgroundColor = Color.FromArgb("#fce9d9");
                truebtn.TextColor = Color.FromArgb("#77212E");
                AddtoPlan = true;
}
            else
            {
                falsebtn.BorderColor = Colors.Transparent;
                falsebtn.BackgroundColor = Color.FromArgb("#fce9d9");
                falsebtn.TextColor = Color.FromArgb("#77212E");
                AddtoPlan = false;
            }

            AffectonSymptoms.IsVisible = true; 

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ConfirmBtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                HandleAddNewActivity();
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
            
        }
        catch (Exception Ex)
        {

        }
    }

    private void symptomlistview_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {

            var Selecteditem = e.AddedItem as usersymptom;
            var Addusersymptom = new usersymptom();

            if (Selecteditem == null)
            {
                Selecteditem = e.RemovedItem as usersymptom;
            }
            Addusersymptom.symptomid = Selecteditem.symptomid;
            Addusersymptom.symptomtitle = Selecteditem.symptomtitle;
          
            var itemtoadd = Selecteditem.symptomid + "|" + Selecteditem.symptomtitle;

            if (SelectedUserSymptoms.Count == 0)
            {
                SelectedUserSymptoms.Add(Addusersymptom);
            }
            else
            {
                //Add/ Remove items 
                if (SelectedUserSymptoms.Contains(Addusersymptom))
                {
                    SelectedUserSymptoms.Remove(Addusersymptom);
                }
                else
                {
                    SelectedUserSymptoms.Add(Addusersymptom);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}