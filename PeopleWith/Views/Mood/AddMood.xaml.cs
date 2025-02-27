using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using Mopups.Services;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;

namespace PeopleWith;
public partial class AddMood : ContentPage
{
    string SelectedTime;
    string SelectedDate;
    string SelectedMood;
    //Change to Mood when added 
    public ObservableCollection<usermood> AlluserMoods = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> MoodtoAdd = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> AddedMood = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> MoodtoUpdate = new ObservableCollection<usermood>();
    usermood MoodPassed = new usermood(); 
    public ObservableCollection<moodlist> AllMoods { get; set; }
    userfeedback userfeedbacklistpassed = new userfeedback();
    public moodlist GetMoodlist { get; set; }
    string EditAdd;
    //DatePickerViewModel ViewModel = new DatePickerViewModel();
    int Selectedindex;
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

    public AddMood()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }  
    }

    public AddMood(usermood PassMood, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            MoodPassed = PassMood;
            userfeedbacklistpassed = userfeedbacklist;
            howlbl.IsVisible = false;
            MoodListview.IsVisible = false;

            if (!String.IsNullOrEmpty(MoodPassed.notes))
            {
                Notes.Text = MoodPassed.notes;
            }

            var GetDateTime = DateTime.Parse(MoodPassed.datetime);
            adddatepicker.Date = GetDateTime;
            addtimepicker.Time = GetDateTime.TimeOfDay;
            AddMoodBtn.Text = "Update Mood";

            AddTitle.Text = MoodPassed.title;

            Deletebtn.IsVisible = true;
            Deletelbl.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddMood(ObservableCollection<usermood> MoodsPassed, string AddEdit)
    {
        try
        {
            InitializeComponent();
            GetMoodlist = new moodlist();
            AllMoods = GetMoodlist.GetList();
            AlluserMoods = MoodsPassed;

            foreach(var item in AlluserMoods)
            {
                if(item.id == AddEdit)
                {
                    MoodPassed = item; 
                }
            }
            EditAdd = AddEdit;

            MoodListview.ItemsSource = AllMoods;

            if (EditAdd == "Add")
            {
                adddatepicker.Date = DateTime.Now;
                adddatepicker.MaximumDate = DateTime.Now;
                addtimepicker.Time = DateTime.Now.TimeOfDay; 
            }
            else
            {
                foreach (var item in AlluserMoods)
                {
                    if (item.id == AddEdit)
                    {

                        var GetDateTime = DateTime.Parse(item.datetime);
                        adddatepicker.Date = GetDateTime;
                        addtimepicker.Time = GetDateTime.TimeOfDay;
                        adddatepicker.MaximumDate = DateTime.Now;
                        //Datelbl.Text = GetDateTime.ToString("dd MMM");
                        //Timelbl.Text = GetDateTime.ToString("HH:mm");

                        //ViewModel.SelectedDate = GetDateTime.ToString("dd/MM/yyyy");
                        var gettime = GetDateTime.ToString("HH:mm:ss");
                        //ViewModel.SelectedTime = TimeSpan.Parse(gettime);

                        //Schedulepopup.BindingContext = ViewModel;
                        //Timepopup.BindingContext = ViewModel;

                        var selected = item.title;

                        foreach (var x in AllMoods)
                        {
                            if (x.Text == selected)
                            {
                                MoodListview.SelectedItem = x;

                            }
                        }

                        for (int i = 0; i < AllMoods.Count; i++)
                        {

                            if (AllMoods[i].Text == selected)
                            {
                                Selectedindex = i;
                                MoodListview.ItemsLayout.ScrollToRowIndex(Selectedindex, true);
                            }

                        }


                        if (string.IsNullOrEmpty(item.notes))
                        {

                        }
                        else
                        {
                            Notes.Text = item.notes;

                        }

                        AddMoodBtn.Text = "Update Mood";

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddMood(ObservableCollection<usermood> MoodsPassed, string AddEdit, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            GetMoodlist = new moodlist();
            AllMoods = GetMoodlist.GetList();
            AlluserMoods = MoodsPassed;
            EditAdd = AddEdit;
            userfeedbacklistpassed = userfeedbacklist;

            MoodListview.ItemsSource = AllMoods;

            if (EditAdd == "Add")
            {
                adddatepicker.Date = DateTime.Now;
                addtimepicker.Time = DateTime.Now.TimeOfDay;
            }
            else
            {
                foreach (var item in AlluserMoods)
                {
                    if (item.id == AddEdit)
                    {

                        var GetDateTime = DateTime.Parse(item.datetime);
                        adddatepicker.Date = GetDateTime;
                        addtimepicker.Time = GetDateTime.TimeOfDay;
                        //Datelbl.Text = GetDateTime.ToString("dd MMM");
                        //Timelbl.Text = GetDateTime.ToString("HH:mm");

                        //ViewModel.SelectedDate = GetDateTime.ToString("dd/MM/yyyy");
                        var gettime = GetDateTime.ToString("HH:mm:ss");
                        //ViewModel.SelectedTime = TimeSpan.Parse(gettime);

                        //Schedulepopup.BindingContext = ViewModel;
                        //Timepopup.BindingContext = ViewModel;

                        var selected = item.title;

                        foreach (var x in AllMoods)
                        {
                            if (x.Text == selected)
                            {
                                MoodListview.SelectedItem = x;

                            }
                        }

                        for (int i = 0; i < AllMoods.Count; i++)
                        {

                            if (AllMoods[i].Text == selected)
                            {
                                Selectedindex = i;
                                MoodListview.ItemsLayout.ScrollToRowIndex(Selectedindex, true);
                            }

                        }


                        if (string.IsNullOrEmpty(item.notes))
                        {

                        }
                        else
                        {
                            Notes.Text = item.notes;

                        }

                        AddMoodBtn.Text = "Update Mood";

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddMood(ObservableCollection<usermood> MoodsPassed, string AddEdit, userfeedback userfeedbacklist, string moodfromsingleview)
    {
        try
        {
            InitializeComponent();
            GetMoodlist = new moodlist();
            AllMoods = GetMoodlist.GetList();
            AlluserMoods = MoodsPassed;
            EditAdd = AddEdit;
            userfeedbacklistpassed = userfeedbacklist;

            MoodListview.ItemsSource = AllMoods;

           // var mood = AllMoods.Where(x => x.Text == moodfromsingleview).FirstOrDefault();

            SelectedMood = moodfromsingleview;

            howlbl.IsVisible = false;
            MoodListview.IsVisible = false;

            if (EditAdd == "Add")
            {
                adddatepicker.Date = DateTime.Now;
                addtimepicker.Time = DateTime.Now.TimeOfDay;
            }
            else
            {
                foreach (var item in AlluserMoods)
                {
                    if (item.id == AddEdit)
                    {

                        var GetDateTime = DateTime.Parse(item.datetime);
                        adddatepicker.Date = GetDateTime;
                        addtimepicker.Time = GetDateTime.TimeOfDay;
                        //Datelbl.Text = GetDateTime.ToString("dd MMM");
                        //Timelbl.Text = GetDateTime.ToString("HH:mm");

                        //ViewModel.SelectedDate = GetDateTime.ToString("dd/MM/yyyy");
                        var gettime = GetDateTime.ToString("HH:mm:ss");
                        //ViewModel.SelectedTime = TimeSpan.Parse(gettime);

                        //Schedulepopup.BindingContext = ViewModel;
                        //Timepopup.BindingContext = ViewModel;

                        var selected = item.title;

                        foreach (var x in AllMoods)
                        {
                            if (x.Text == selected)
                            {
                                MoodListview.SelectedItem = x;

                            }
                        }

                        for (int i = 0; i < AllMoods.Count; i++)
                        {

                            if (AllMoods[i].Text == selected)
                            {
                                Selectedindex = i;
                                MoodListview.ItemsLayout.ScrollToRowIndex(Selectedindex, true);
                            }

                        }


                        if (string.IsNullOrEmpty(item.notes))
                        {

                        }
                        else
                        {
                            Notes.Text = item.notes;

                        }

                        AddMoodBtn.Text = "Update Mood";

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }




    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Schedulepopup.IsOpen = true;
            Mainstack.Opacity = 0.2;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Timepopup.IsOpen = true;
            Mainstack.Opacity = 0.2;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void AddMoodBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddMoodBtn.IsEnabled = false;
                //SuccessScreen.IsVisible = true;
                //Success.IsAnimationEnabled = true;
                var Userid = Helpers.Settings.UserKey;
                SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
                SelectedTime = addtimepicker.Time.ToString(@"hh\:mm");
                MoodtoUpdate.Clear();
                //Update DB Call 
                APICalls database = new APICalls();
                if (EditAdd == "Add")
                {
                    //Add New Mood
                    var NewMood = new usermood();
                    if (string.IsNullOrEmpty(SelectedDate) || SelectedDate == null)
                    {
                        var Date = DateTime.Now;
                        SelectedDate = Date.ToString("dd/MM/yyyy");

                    }
                    if (string.IsNullOrEmpty(SelectedTime) || SelectedTime == null)
                    {
                        var time = DateTime.Now;
                        SelectedTime = time.ToString("HH:mm:ss");
                    }

                    if (string.IsNullOrEmpty(SelectedMood) || SelectedMood == null)
                    {
                        SelectedMood = AlluserMoods[0].title;
                    }

                    NewMood.userid = Userid;

                    NewMood.title = SelectedMood;

                    NewMood.datetime = SelectedDate + " " + SelectedTime;

                    NewMood.notes = Notes.Text;

                    MoodtoAdd.Add(NewMood);
                    var AddedMood = await database.PostUserMoodAsync(MoodtoAdd);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Added") { });
                    await Task.Delay(1500);

                    foreach (var item in AddedMood)
                    {
                        AlluserMoods.Add(item);
                    }

                    var newsym = new feedbackdata();
                    newsym.id = AddedMood[0].id;
                    newsym.value = NewMood.notes;
                    newsym.datetime = NewMood.datetime;
                    newsym.action = "update";
                    newsym.label = NewMood.title;

                    if (userfeedbacklistpassed.moodfeedbacklist == null)
                    {
                        userfeedbacklistpassed.moodfeedbacklist = new ObservableCollection<feedbackdata>();
                    }

                    userfeedbacklistpassed.moodfeedbacklist.Add(newsym);

                    string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.moodfeedbacklist);
                    userfeedbacklistpassed.moodfeedback = newsymJson;


                    await database.UserfeedbackUpdateMoodData(userfeedbacklistpassed);


                }
                else
                {
                    if(AddMoodBtn.Text == "Update Mood")
                    {
                        //Update Single Mood 
                        var UpdateDateTime = SelectedDate + " " + SelectedTime;
                        MoodPassed.datetime = UpdateDateTime;

                        if (string.IsNullOrEmpty(Notes.Text))
                        {
                            MoodPassed.notes = null; 

                        }
                        else
                        {
                            MoodPassed.notes = Notes.Text; 
                        }
                        if (!string.IsNullOrEmpty(SelectedMood))
                        {
                            MoodPassed.title = SelectedMood;
                        }
                       

                        MoodtoUpdate.Add(MoodPassed);
                        await database.PutMoodAsync(MoodtoUpdate);

                        await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Updated") { });
                        await Task.Delay(1500);

                        //Update UserMoodFeedback
                       foreach(var item in userfeedbacklistpassed.moodfeedbacklist)
                        {
                            if(item.id != null)
                            {
                                if(item.id == MoodPassed.id)
                                {
                                    if (string.IsNullOrEmpty(Notes.Text))
                                    {
                                        item.value = null;
                                       
                                    }
                                    else
                                    {
                                        item.value = Notes.Text;
                                    }
                                    
                                    item.datetime = MoodPassed.datetime;
                                }
                            }
                        }

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.moodfeedbacklist);
                        userfeedbacklistpassed.moodfeedback = newsymJson;

                        await database.UserfeedbackUpdateMoodData(userfeedbacklistpassed);

                    }
                    else
                    {
                        //Edit Mood

                        foreach (var item in AlluserMoods)
                        {
                            if (item.id == EditAdd)
                            {
                                if (string.IsNullOrEmpty(SelectedDate) || SelectedDate == null)
                                {
                                    var GetDate = DateTime.Parse(item.datetime);
                                    SelectedDate = GetDate.ToString("dd/MM/yyyy");
                                }

                                if (string.IsNullOrEmpty(SelectedTime) || SelectedTime == null)
                                {
                                    var GetTime = DateTime.Parse(item.datetime);
                                    SelectedTime = GetTime.ToString("HH:mm:ss");
                                }

                                if (string.IsNullOrEmpty(SelectedMood) || SelectedMood == null)
                                {
                                    SelectedMood = item.title;
                                }


                                item.datetime = SelectedDate + " " + SelectedTime;
                                item.title = SelectedMood;
                                if (Notes.Text == null)
                                {
                                    item.notes = null;
                                }
                                else
                                {
                                    item.notes = Notes.Text;
                                }
                                item.source = SelectedMood.ToLower() + ".png";
                                MoodtoUpdate.Add(item);
                                await database.PutMoodAsync(MoodtoUpdate);

                                await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Updated") { });
                                await Task.Delay(1500);

                                foreach (var x in userfeedbacklistpassed.moodfeedbacklist)
                                {
                                    if (x.id != null)
                                    {
                                        if (x.id == MoodPassed.id)
                                        {
                                            if (string.IsNullOrEmpty(Notes.Text))
                                            {
                                                x.value = null;

                                            }
                                            else
                                            {
                                                x.value = Notes.Text;
                                            }

                                            x.datetime = MoodPassed.datetime;
                                        }
                                    }
                                }

                                string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.moodfeedbacklist);
                                userfeedbacklistpassed.moodfeedback = newsymJson;

                                await database.UserfeedbackUpdateMoodData(userfeedbacklistpassed);


                            }
                        }
                    }
                }
                await MopupService.Instance.PopAllAsync(false);
                AddMoodBtn.IsEnabled = true;

                //Push Through new Added Data
                await Navigation.PushAsync(new AllMood(userfeedbacklistpassed));

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllMood);
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is SingleMood);
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                Navigation.RemovePage(this);

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

    private void Schedulepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SelectDatePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.DatePickerSelectionChangedEventArgs e)
    {
        try
        {
            var Date = e.NewValue;
            //Datelbl.Text = Date.ToString("dd MMM");
            var split = Date.ToString().Split(' ');
            SelectedDate = split[0];
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Timepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {   
            NotasyncMethod(Ex);
        }
    }

    private void TimePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.TimePickerSelectionChangedEventArgs e)
    {
        try
        {
            //DateTime datetime = DateTime.Parse(Datelbl.Text);
            //DateTime SelectedDate = datetime + e.NewValue;
            //if (SelectedDate <= DateTime.Now)
            //{
            //    var Time = e.NewValue.ToString();

            //    var split = Time.Split(':');
            //    Timelbl.Text = split[0] + ":" + split[1];
            //    var getTime = split[0] + ":" + split[1] + ":" + "00";
            //    SelectedTime = getTime;
            //}
            //else
            //{
            //    int secondsToVibrate = Random.Shared.Next(1, 1);
            //    TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

            //    Vibration.Default.Vibrate(vibrationLength);
            //    return;
            //}
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
            var Mood = e.DataItem as moodlist;
            SelectedMood = Mood.Text;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                Deletebtn.IsEnabled = false;
                bool Delete = await DisplayAlert("Delete Mood", "Are you sure you would like the delete this Mood? Once deleted it cannot be retrieved", "Delete", "Cancel");
                if (Delete == true)
                {

                    MoodPassed.deleted = true;

                    APICalls database = new APICalls();
                    await database.DeleteUserMood(MoodPassed);
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Deleted") { });
                    await Task.Delay(1500);

                    foreach (var item in AlluserMoods)
                    {
                        if (item.id == MoodPassed.id)
                        {
                            item.deleted = true;
                        }
                    }

                    foreach (var item in userfeedbacklistpassed.moodfeedbacklist)
                    {
                        if (item.id != null)
                        {
                            if (item.id == MoodPassed.id)
                            {
                                item.action = "deleted";
                            }
                        }
                    }

                    string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.moodfeedbacklist);
                    userfeedbacklistpassed.moodfeedback = newsymJson;

                    await database.UserfeedbackUpdateMoodData(userfeedbacklistpassed);

                    await MopupService.Instance.PopAllAsync(false);
                    //Not There any more 
                    await Navigation.PushAsync(new AllMood(userfeedbacklistpassed));

                    var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllMood);

                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleMood);

                    if (pageToRemove != null)
                    {
                        Navigation.RemovePage(pageToRemove);
                    }
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);

                    Deletebtn.IsEnabled = true;
                    return;
                }
                else
                {
                    Deletebtn.IsEnabled = true;
                    return;
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
}