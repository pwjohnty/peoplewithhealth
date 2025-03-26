using System.Collections.ObjectModel;
using System.Text;
using Mopups.Services;
using Syncfusion.Maui.ListView;

namespace PeopleWith;

public partial class SingleActivity : ContentPage
{
    public userdailyactivity PassedActivity = new userdailyactivity();
    public ObservableCollection<userdailyactivity> AllUserActivity = new ObservableCollection<userdailyactivity>(); 
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
    public SingleActivity()
    {
        InitializeComponent();
    }

    public SingleActivity(userdailyactivity activitypassed, ObservableCollection<userdailyactivity> AllActivity)
    {
        InitializeComponent();
        PassedActivity = activitypassed;
        AllUserActivity = AllActivity;

        //Set Data for page 
        ActivityName.Text = PassedActivity.activitytitle;
        ActivityCategory.Source = PassedActivity.Typeimg;
        StartDatelbl.Text = PassedActivity.startdate;
        Durationlbl.Text = PassedActivity.convertedduration;




        if (DateTime.Parse(PassedActivity.startdate).Date <= DateTime.Now.Date)
        {
            FeedbackStack.IsVisible = true;
            bool CheckifEmpty = false;
            if (String.IsNullOrEmpty(PassedActivity.notes))
            {
                Noteslbl.Text = "No Notes Added";
                CheckifEmpty = true;
            }
            else
            {
                Noteslbl.Text = PassedActivity.notes;
            }
            if (string.IsNullOrEmpty(PassedActivity.ActivityFeedbackList.Mood))
            {
                NoMood.IsVisible = true;
                MoodImg.IsVisible = false;
                Moodlbl.IsVisible = false;
                NoMood.Text = "No Mood Added";
                CheckifEmpty = true;
            }
            else
            {
                NoMood.IsVisible = false;
                MoodImg.IsVisible = true;
                Moodlbl.IsVisible = true;
                MoodImg.Source = PassedActivity.moodimg;
                Moodlbl.Text = PassedActivity.ActivityFeedbackList.Mood;
            }

            if (string.IsNullOrEmpty(PassedActivity.ActivityFeedbackList.Outcome))
            {
                Outcomelbl.Text = "No Outcome Added";
                CheckifEmpty = true;
            }
            else
            {
                Outcomelbl.Text = PassedActivity.ActivityFeedbackList.Outcome;
            }

            //if (CheckifEmpty)
            //{
            //    // If any Empty (Set is visible to true)
            //    AddFeedbackBtn.IsVisible = true; 
            //}
        }
    }

    private async void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            EditBtn.IsEnabled = false; 
            string AddEdit = "Edit";
            await Navigation.PushAsync(new AddActivity(AllUserActivity, PassedActivity, AddEdit));
            EditBtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            EditBtn.IsEnabled = true;
            NotasyncMethod(Ex);
        }
    }

    async private void Information_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await DisplayAlert("Activity Information", "No information or resources available for this Activity", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                DeleteBtn.IsEnabled = false;

                bool Delete = await DisplayAlert("Delete Activity", "Are you sure you would like the delete this Activity? Once deleted it cannot be retrieved", "Continue", "Cancel");
                if (Delete == true)
                {
                    //Delete Item 
                    PassedActivity.deleted = true;
                    APICalls database = new APICalls();

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Actvity Deleted") { });
                    await database.DeleteUserActivity(PassedActivity);

                    await Navigation.PushAsync(new ActivitySchedule());
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is ActivitySchedule);
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                    DeleteBtn.IsEnabled = true;

                    // Ensures page fully loaded 
                    MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Task.Delay(1000);
                        await MopupService.Instance.PopAllAsync(false);
                    });
                }
                else
                {
                    DeleteBtn.IsEnabled = true;
                    return;
                }

            }
            else
            {
                DeleteBtn.IsEnabled = true;
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            showallbtn.IsEnabled = false;
            //await Navigation.PushAsync(new AddActivity(AllUserActivity, PassedActivity, AddEdit));
            showallbtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            showallbtn.IsEnabled = true;
            NotasyncMethod(Ex);
        }
    }

    private async void DuplicateBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            DuplicateBtn.IsEnabled = false; 
            string AddEdit = "Duplicate";
            await Navigation.PushAsync(new AddActivity(AllUserActivity, PassedActivity, AddEdit));
            DuplicateBtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            DuplicateBtn.IsEnabled = true;
            NotasyncMethod(Ex);
        }

    }


    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            string AddEdit = "Feedback";
            await Navigation.PushAsync(new AddActivity(AllUserActivity, PassedActivity, AddEdit));
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}