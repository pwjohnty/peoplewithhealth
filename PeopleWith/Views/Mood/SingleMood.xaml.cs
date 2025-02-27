using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleMood : ContentPage
{
    public ObservableCollection<usermood> AlluserMoods = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> MoodPassed = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> allsamemoods = new ObservableCollection<usermood>();
    userfeedback userfeedbacklistpassed = new userfeedback();
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

    public SingleMood()
    {
        InitializeComponent();
    }

    public SingleMood(ObservableCollection<usermood> singlemood, ObservableCollection<usermood> AllMoods, ObservableCollection<usermood> samemoods, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            AlluserMoods = AllMoods;
            MoodPassed = singlemood;
            allsamemoods = samemoods;
            userfeedbacklistpassed = userfeedbacklist;

            Moodimg.Source = MoodPassed[0].source;
            MoodTitle.Text = MoodPassed[0].title;
            MoodDate.Text = MoodPassed[0].datetime;
            if (string.IsNullOrEmpty(MoodPassed[0].notes))
            {
                MoodNotes.Text = "No Notes Added";
            }
            else
            {
                MoodNotes.Text = "Notes: " + MoodPassed[0].notes;
            }


            AllMoodView.ItemsSource = samemoods.OrderByDescending(f => DateTime.Parse(f.datetime));

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

        this.userfeedbacklistpassed = userfeedbacklistpassed;
    }

    async private void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                EditBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddMood(AlluserMoods, MoodPassed[0].id));
                EditBtn.IsEnabled = true;
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

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await DisplayAlert("Mood Information", "No information or resources available for this Mood", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
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
                    var PassedMood = MoodPassed[0];
                    PassedMood.deleted = true; 


                    APICalls database = new APICalls();
                    await database.DeleteUserMood(PassedMood);
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Deleted") { });
                    await Task.Delay(1500);

                    foreach (var item in AlluserMoods)
                    {
                        if (item.id == PassedMood.id)
                        {
                            item.deleted = true;
                        }
                    }

                    await MopupService.Instance.PopAllAsync(false);
                    await Navigation.PushAsync(new AllMood(AlluserMoods));

                    var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllMood);

                    if (pageToRemove != null)
                    {
                        Navigation.RemovePage(pageToRemove);
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

    private async void AddDataBtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            AddDataBtn.IsEnabled = false;
            await Navigation.PushAsync(new AddMood(AlluserMoods, "Add", userfeedbacklistpassed, MoodPassed[0].title));
            AddDataBtn.IsEnabled = true;

        }
        catch(Exception ex)
        {

        }
    }

    private async void AllMoodView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var selecteditem = e.DataItem as usermood;
            await Navigation.PushAsync(new AddMood(selecteditem, userfeedbacklistpassed), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}