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
    public ObservableCollection<moodlist> AllMoods { get; set; }
    public moodlist GetMoodlist { get; set; }
    string EditAdd;
    //DatePickerViewModel ViewModel = new DatePickerViewModel();
    int Selectedindex;


    public AddMood()
    {
        InitializeComponent();
    }

    public AddMood(ObservableCollection<usermood> MoodsPassed, string AddEdit)
    {
        try
        {
            InitializeComponent();
            GetMoodlist = new moodlist();
            AllMoods = GetMoodlist.GetList();
            AlluserMoods = MoodsPassed;
            EditAdd = AddEdit;

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
            //Add Crash log 
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
            //add Crash log
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
            //add Crash log
        }

    }

    async private void AddMoodBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //SuccessScreen.IsVisible = true;
            //Success.IsAnimationEnabled = true;
            var Userid = Helpers.Settings.UserKey;
            SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
            SelectedTime = addtimepicker.Time.ToString(@"hh\:mm");
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
                AddedMood = await database.PostUserMoodAsync(MoodtoAdd);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Added") { });
                await Task.Delay(1500);

                foreach (var item in AddedMood)
                {
                    AlluserMoods.Add(item);
                }
            }
            else
            {
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


                    }
                }
            }
            await MopupService.Instance.PopAllAsync(false);

            //Push Through new Added Data
            await Navigation.PushAsync(new AllMood(AlluserMoods));

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

            //SuccessScreen.IsVisible = false;
            //Success.IsAnimationEnabled = false;
        }
        catch (Exception ex)
        {
            //Add Crash Log
        }

    }

    private void Schedulepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {   //Add Crash Log

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
            //Add Crash Log
        }
    }

    private void Timepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {   //Add Crash Log

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
            //Add Crash log
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
            //Add Crash Log
        }

    }
}