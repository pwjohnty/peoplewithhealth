//using System.Collections.ObjectModel;
//using Mopups.Services;
//using Syncfusion.Maui.ListView;

//namespace PeopleWith;

//public partial class SingleDailyActivity : ContentPage
//{
//    public userdailyactivity SelectedActivity = new userdailyactivity();
//    public ActivityPlanner SelectedPlanner= new ActivityPlanner();
//    public ObservableCollection<activefrequency> weeklyjoin = new ObservableCollection<activefrequency>();
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
//    public SingleDailyActivity(userdailyactivity Activity)
//    {
//        InitializeComponent();
//        //SelectedPlanner = SelectedItem;
//        SelectedActivity = Activity;


//        //Set Data 
//        Titlelbl.Text = SelectedActivity.activitytitle;
//        Displaylbl.Text = SelectedActivity.displayname;

//        if (String.IsNullOrEmpty(Displaylbl.Text) || Displaylbl.Text == "")
//        {
//            Displaylbl.IsVisible = false; 
//        }

//        SDlbl.Text = SelectedActivity.startdate;
//        if (string.IsNullOrEmpty(SelectedActivity.enddate))
//        {
//            EDlbl.Text = "Ongoing";
//        }
//        else
//        {
//            EDlbl.Text = SelectedActivity.enddate;
//        }

//        var GetNotes = SelectedActivity.noteslist?.FirstOrDefault(x => !string.IsNullOrEmpty(x.notes))?.notes;

//        if (String.IsNullOrEmpty(GetNotes))
//        {
//            Noteslbl.Text = "No Notes Added";
//        }
//        else
//        {
//            Noteslbl.Text = GetNotes;
//        }

//        var GetFrequency = SelectedActivity.activityfrequencylist.FirstOrDefault()?.frequency;
//        freqllb.Text = GetFrequency;
//        SelectedActivity.frequency = GetFrequency;


//        if (SelectedActivity.frequency == "One Off")
//        {
//            Repeatlbl.Text = SelectedActivity.frequency;
//        }
//        else
//        {
//            if (SelectedActivity.frequency == "Daily")
//            {
//                Repeatlbl.Text = "Every Day starting from " + SelectedActivity.startdate;
//            }
//            else if (SelectedActivity.frequency == "Weekly")
//            {
//                Repeatlbl.Text = "Every Week starting from " + SelectedActivity.startdate;
//            }
//            else if (SelectedActivity.frequency == "Monthly")
//            {
//                Repeatlbl.Text = "Every Month starting from " + SelectedActivity.startdate;
//            }
//            else if (SelectedActivity.frequency == "Quarterly")
//            {
//                Repeatlbl.Text = "Every 3 Month starting from " + SelectedActivity.startdate;
//            }
//            else if (SelectedActivity.frequency == "Yearly")
//            {
//                Repeatlbl.Text = "Every Year starting from " + SelectedActivity.startdate;
//            }
//        }

//        if (SelectedActivity.activityoption == "On")
//        {
//            Notiflbl.Text = "Notifications Enabled"; 
//        }
//        else
//        {
//            Notiflbl.Text = "Notifications Disabled";
//        }

//        //populate Schedule View 
//        //bool CheckWeeklyJoin = SelectedActivity.activityfrequencylist.Any()
//        int i = 1; 
//        foreach (var item in SelectedActivity.activityfrequencylist)
//        {

//            bool week = item.frequency == "Weekly";
//            item.weeklyitem = week;
//            item.Timespan = TimeSpan.Parse(item.time);
//            item.itemno = i.ToString(); 
//            i++; 
//        }

//        var GetItem = SelectedActivity.activityfrequencylist;
//        Scheduletimes.ItemsSource = GetItem;

     
//    }

//    public async void showallbtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            showallbtn.IsEnabled = false; 
//            await Navigation.PushAsync(new ActivityShowAllData(SelectedActivity), false);
//            showallbtn.IsEnabled = true;
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void DeleteBtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            //Connectivity Changed 
//            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
//            if (accessType == NetworkAccess.Internet)
//            {
//                DeleteBtn.IsEnabled = false;


//                bool Delete = await DisplayAlert("Delete Activity", "Are you sure you would like the delete this Activity? Once deleted it cannot be retrieved", "Continue", "Cancel");
//                if (Delete == true)
//                {
//                    //Delete Item 
//                    SelectedActivity.deleted = true;
//                    APICalls database = new APICalls();
//                    await database.DeleteUserActivity(SelectedActivity);


//                    await MopupService.Instance.PushAsync(new PopupPageHelper("Actvity Deleted") { });
//                    await Task.Delay(5000);

//                    await MopupService.Instance.PopAllAsync(false);

//                    await Navigation.PushAsync(new AllDailyActivity());
//                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllDailyActivity);
//                    if (pageToRemoves != null)
//                    {
//                        Navigation.RemovePage(pageToRemoves);
//                    }
//                    Navigation.RemovePage(this);
//                    DeleteBtn.IsEnabled = true;
//                }
//                else
//                {
//                    DeleteBtn.IsEnabled = true;
//                    return;
//                }

//            }
//            else
//            {
//                var isConnected = accessType == NetworkAccess.Internet;
//                ConnectivityChanged?.Invoke(this, isConnected);
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    //private void EditBtn_Clicked(object sender, EventArgs e)
//    //{
//    //    try
//    //    {
//    //        //Edit Item 
//    //    }
//    //    catch (Exception Ex)
//    //    {
//    //        NotasyncMethod(Ex);
//    //    }
//    //}

//    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
//    {
//        try
//        {

//            if(ScheduleView.IsVisible == false)
//            {
//                ScheduleView.IsVisible = true;
//                Schedulelbl.Text = "Hide Scedule"; 
//                ExpandArrow.RotateTo(90); 
//            }
//            else
//            {
//                ScheduleView.IsVisible = false;
//                Schedulelbl.Text = "View Scedule";
//                ExpandArrow.RotateTo(270);
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }
//}